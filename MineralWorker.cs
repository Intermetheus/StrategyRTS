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
        

        public MineralWorker()
        {
            workerThread = new Thread(Behaviour);
            Position = new Vector2(400, 400);
            resourceBeingHeld = false;
            speed = 250;
            canMove = true;
        }

        public override void LoadContent(ContentManager content)
        {
            sprite = content.Load<Texture2D>("worker");
        }

        public override void OnCollision(GameObject other)
        {
            if (other is Mineral)
            {
                if (!resourceBeingHeld)
                {
                    
                    //Extract()
                    resourceBeingHeld = true;
                    Enter();
                }
            }
            if (other is Base)
            {
                //Deposit
                if (resourceBeingHeld)
                {
                    resourceBeingHeld = false;

                }
            }
        }

        public void Behaviour()
        {
            while(true)
            {
                if (!resourceBeingHeld)
                {
                    //Because the worker does not have a resource
                    //Search the GameWorld for a Mineral Deposit
                    //Use ToList() to create a copy of gameObjects to prevent changes to a list being looped by another thread
                    //Alternatively use a lock
                    SetDestination<Mineral>();
                    
                }

                if (resourceBeingHeld)
                {
                    //Base destination, adding later 🦥 <--sloth emoji
                    //perhaps make a method called SetDestination(gameobject)
                    SetDestination<Base>();
                }

                if (canMove)
                {
                    Move();
                }
            }

            
        }

        private void Enter()
        {
            canMove = false;
            
            mineralSemaphore.WaitOne();
            Thread.Sleep(1000);
            canMove = true;
            mineralSemaphore.Release();
        }


    }
}
