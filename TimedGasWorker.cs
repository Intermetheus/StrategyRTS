using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace StrategyRTS
{
    class TimedGasWorker : Unit
    {
        private bool enterMine;
        private Random random;
        private Gas enteredGas;
        private float aliveTime;
        private bool isAlive;
        private float startTime; //gameTime when the unit was created

        public Gas EnteredGas { get => enteredGas; set => enteredGas = value; }

        public TimedGasWorker()
        {
            StartValues();
        }
        /// <summary>
        /// Used for instantiating a Worker
        /// </summary>
        /// <param name="sprite"></param>
        public TimedGasWorker(Texture2D sprite)
        {
            this.sprite = sprite;
            StartValues();
        }

        public TimedGasWorker(int id)
        {
            this.id = id;
            StartValues();
        }

        private void StartValues()
        {
            isAlive = true;
            random = new Random();
            workerThread = new Thread(Behaviour);
            Position = new Vector2(400 + random.Next(0, 50), 400);
            ResourceBeingHeld = false;
            speed = 200;
            canMove = true;
            enterMine = false;
            scale = 1;
            aliveTime = 30000;
        }

        public override void StartThread()
        {
            workerThread.Start();
            startTime = (float)GameWorld.GameTimeProp.TotalGameTime.TotalMilliseconds;
        }

        public override void LoadContent(ContentManager content)
        {
            sprite = content.Load<Texture2D>("gasWorker");
        }

        public override void OnCollision(GameObject other)
        {
            if (other is Gas)
            {
                //Extract()
                if (!ResourceBeingHeld)
                {
                    EnteredGas = (Gas)other;
                    if (canMove)
                    {
                        enterMine = true;
                    }
                }
            }
            if (other is Base)
            {
                //Deposit
                if (ResourceBeingHeld)
                {
                    ResourceBeingHeld = false;
                    GameWorld.MyBase.AddGas(1);
                    if ((float)GameWorld.GameTimeProp.TotalGameTime.TotalMilliseconds > aliveTime + startTime)
                    {
                        isAlive = false;
                        GameWorld.Destroy(this);
                    }
                }
            }
        }

        public void Behaviour()
        {
            while (isAlive)
            {
                if (!ResourceBeingHeld)
                {
                    SetDestination<Gas>();
                }

                if (ResourceBeingHeld)
                {
                    SetDestination<Base>();
                }

                if (canMove)
                {
                    Move();
                }

                if (enterMine && enteredGas != null)
                {
                    enterMine = false;
                    Enter();
                }
            }
        }

        private void Enter()
        {
            canMove = false;
            enteredGas.GasSemaphore.WaitOne();
            scale = 0;
            Thread.Sleep(10000);
            enteredGas.GasSemaphore.Release();
            ResourceBeingHeld = true;
            enteredGas = null;
            scale = 1;
            canMove = true;
        }
    }
}
