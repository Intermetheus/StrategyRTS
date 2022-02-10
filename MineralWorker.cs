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
    /// Subclass of Unit. Worker that only mines minerals
    /// </summary>
    class MineralWorker : Unit
    {
        private bool enterMine;
        private Random random;
        private Mineral enteredMineral;

        /// <summary>
        /// Creates a worker
        /// </summary>
        public MineralWorker()
        {
            StartValues();
        }

        /// <summary>
        /// Instantiate a mineral worker with a non-unique id(used to give them names)
        /// </summary>
        /// <param name="id"></param>
        public MineralWorker(int id)
        {
            this.id = id;
            StartValues();
        }

        /// <summary>
        /// Start values of a worker. Used for sharing start values between different constructors
        /// </summary>
        private void StartValues()
        {
            random = new Random();
            workerThread = new Thread(Behaviour);
            workerThread.IsBackground = true;
            Position = new Vector2(400+random.Next(0,50), 400);
            ResourceBeingHeld = false;
            speed = 200;
            canMove = true;
            enterMine = false;
            scale = 1;
        }

        /// <summary>
        /// Loads the mineralWorker sprite
        /// </summary>
        /// <param name="content"></param>
        public override void LoadContent(ContentManager content)
        {
            sprite = content.Load<Texture2D>("mineralWorker");
        }

        /// <summary>
        /// Checks if the mineral collides with a Mineral or the Base
        /// </summary>
        /// <param name="other">Only uses Mineral or Base Class</param>
        public override void OnCollision(GameObject other)
        {
            if (other is Mineral)
            {
                //Extract
                if (!ResourceBeingHeld)
                {
                    enteredMineral = (Mineral)other;
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
        /// <summary>
        /// Behaviour method is run by the workerThread
        /// </summary>
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

        /// <summary>
        /// Waits for access to the mineralSemaphore. Waits 10 seconds and relases.
        /// </summary>
        private void Enter()
        {
            canMove = false;
            enteredMineral.MineralSemaphore.WaitOne();
            scale = 0;
            Thread.Sleep(3000);
            enteredMineral.MineralSemaphore.Release();
            ResourceBeingHeld = true;
            enteredMineral = null;
            scale = 1;
            canMove = true;
        }
    }
}
