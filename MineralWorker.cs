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
            Position = new Vector2(400, 400);
            resourceBeingHeld = false;
            speed = 250;
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
                    resourceBeingHeld = true;

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
                    //Because the worker does not have a resource
                    //Search the GameWorld for a Mineral Deposit
                    //Alternatively use a lock
                    SetDestination<Mineral>();
                    Move();
                }

                if (resourceBeingHeld)
                {
                    //Base destination, adding later 🦥 <--sloth emoji
                    //perhaps make a method called SetDestination(gameobject)
                    SetDestination<Base>();
                    Move();
                }
            }
        }
    }
}
