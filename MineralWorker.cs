using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace StrategyRTS
{
    class MineralWorker : Unit
    {
        static Semaphore mineralSemaphore = new Semaphore(0,3);

        private bool enterMine;
        private Random random;

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

        private void StartValues()
        {
            random = new Random();
            workerThread = new Thread(Behaviour);
            Position = new Vector2(400+random.Next(0,50), 400);
            resourceBeingHeld = false;
            speed = 200;
            canMove = true;
            enterMine = false;
        }

        public override void LoadContent(ContentManager content)
        {
            sprite = content.Load<Texture2D>("worker");
        }

        public override void OnCollision(GameObject other)
        {
            if (other is Mineral)
            {

                //Extract()
                if (!resourceBeingHeld)
                {                    
                    //Extract()
                    resourceBeingHeld = true;
                    enterMine = true;
                }
            }
            if (other is Base)
            {
                //Deposit
                if (resourceBeingHeld)
                {
                    resourceBeingHeld = false;
                    GameWorld.MyBase.AddMinerals(1);
                }
            }
        }

        public void Behaviour()
        {
            while(true)
            {
                if (!resourceBeingHeld)
                {
                    SetDestination<Mineral>();
                    
                }

                if (resourceBeingHeld)
                {
                    SetDestination<Base>();
                }

                if (canMove)
                {
                    Move();
                }

                if (enterMine)
                {
                    enterMine = false;
                    Enter();
                }
            }
        }

        private void Enter()
        {
            canMove = false;
            scale = 0;
            mineralSemaphore.Release();
            mineralSemaphore.WaitOne();
            Thread.Sleep(1000);
            scale = 1;
            canMove = true;
        }


    }
}
