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

        public MineralWorker()
        {
            workerThread = new Thread(Behaviour);
            Position = new Vector2(100, 100);
            resourceBeingHeld = false;
            speed = 50;


        }

        public override void LoadContent(ContentManager content)
        {
            sprite = content.Load<Texture2D>("worker");
        }

        public void Behaviour()
        {
            while(true)
            {
                if (resourceBeingHeld == false)
                {
                    //Because the worker does not have a resource
                    //Search the GameWorld for a Mineral Deposit
                    //Use ToList() to create a copy of gameObjects to prevent changes to a list being looped by another thread
                    //Alternatively use a lock
                    foreach (GameObject gameObject in GameWorld.GameObjectsProp.ToList())
                    {
                        if (gameObject is Mineral)
                        {
                            destination = gameObject.Position;
                        }
                    }
                    Move();
                }


            }
        }
    }
}
