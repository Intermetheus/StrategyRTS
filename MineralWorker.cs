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
    class MineralWorker : Unit
    {
        private bool enterMine;
        private Random random;
        private Mineral enteredMineral;

        public Mineral EnteredMineral { get => enteredMineral; set => enteredMineral = value; }

        public MineralWorker()
        {
            StartValues();
        }
        /// <summary>
        /// Used for instantiating a Worker
        /// </summary>
        /// <param name="sprite"></param>
        public MineralWorker(Texture2D sprite)
        {
            this.sprite = sprite;
            StartValues();
        }

        public MineralWorker(int id)
        {
            this.id = id;
            StartValues();
        }

        private void StartValues()
        {
            random = new Random();
            workerThread = new Thread(Behaviour);
            Position = new Vector2(400+random.Next(0,50), 400);
            ResourceBeingHeld = false;
            speed = 200;
            canMove = true;
            enterMine = false;
            scale = 1;
        }

        public override void LoadContent(ContentManager content)
        {
            sprite = content.Load<Texture2D>("mineralWorker");
        }

        public override void OnCollision(GameObject other)
        {
            if (other is Mineral)
            {
                //Extract()
                if (!ResourceBeingHeld)
                {
                    EnteredMineral = (Mineral)other;
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
                    GameWorld.MyBase.AddMinerals(1);
                }
            }
        }

        public void Behaviour()
        {
            while(true)
            {
                if (!ResourceBeingHeld)
                {
                    SetDestination<Mineral>();
                }

                if (ResourceBeingHeld)
                {
                    SetDestination<Base>();
                }

                if (canMove)
                {
                    Move();
                }

                if (enterMine && enteredMineral != null)
                {
                    enterMine = false;
                    Enter();
                }
            }
        }

        private void Enter()
        {
            canMove = false;
            EnteredMineral.MineralSemaphore.WaitOne();
            scale = 0;
            Thread.Sleep(10000);
            EnteredMineral.MineralSemaphore.Release();
            ResourceBeingHeld = true;
            EnteredMineral = null;
            scale = 1;
            canMove = true;
        }
    }
}
