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
    /// <summary>
    /// A worker that mines gas and dies
    /// </summary>
    class TimedGasWorker : Unit
    {
        private bool enterMine;
        private Random random;
        private Gas enteredGas;
        private float aliveTime;
        private bool isAlive;
        private float startTime; //gameTime when the unit was created

        /// <summary>
        /// Creates a TimedGasWorker
        /// </summary>
        public TimedGasWorker()
        {
            StartValues();
        }

        /// <summary>
        /// Creates a timed gas worker with a number used for debugging
        /// </summary>
        /// <param name="id"></param>
        public TimedGasWorker(int id)
        {
            this.id = id;
            StartValues();
        }

        /// <summary>
        /// Makes it easier to set the same values in different constructors.
        /// </summary>
        private void StartValues()
        {
            isAlive = true;
            random = new Random();
            workerThread = new Thread(Behaviour);
            workerThread.IsBackground = true;
            Position = new Vector2(400 + random.Next(0, 50), 400);
            ResourceBeingHeld = false;
            speed = 200;
            canMove = true;
            enterMine = false;
            scale = 1;
            aliveTime = 30000;
        }

        //Overrides the StartThread from Unit. Because GasWorker also needs to know how much time will pass.
        public override void StartThread()
        {
            workerThread.Start();
            startTime = (float)GameWorld.GameTimeProp.TotalGameTime.TotalMilliseconds;
        }

        /// <summary>
        /// Loads the gasWorker sprite
        /// </summary>
        /// <param name="content"></param>
        public override void LoadContent(ContentManager content)
        {
            sprite = content.Load<Texture2D>("gasWorker");
        }

        /// <summary>
        /// Checks collision with Base or Gas
        /// </summary>
        /// <param name="other"></param>
        public override void OnCollision(GameObject other)
        {
            if (other is Gas)
            {
                //Extract
                if (!ResourceBeingHeld)
                {
                    enteredGas = (Gas)other;
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

        /// <summary>
        /// Runs the behaviour of the gasWorker through the workerThread
        /// </summary>
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

        /// <summary>
        /// Waits for access to the gasSemaphore
        /// </summary>
        private void Enter()
        {
            canMove = false;
            enteredGas.GasSemaphore.WaitOne();
            scale = 0;
            Thread.Sleep(3000);
            enteredGas.GasSemaphore.Release();
            ResourceBeingHeld = true;
            enteredGas = null;
            scale = 1;
            canMove = true;
        }
    }
}
