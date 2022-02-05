﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace StrategyRTS
{
    abstract class Unit : GameObject
    {
        public int gasConstructionCost;
        public int mineralConstructionCost;
        protected Thread workerThread;
        protected Vector2 velocity;
        protected Vector2 destination;
        protected float speed;
        protected bool resourceBeingHeld;
        protected bool canMove;

        public void StartThread()
        {
            workerThread.Start();
        }

        public void Move()
        {
            float deltaTime = (float)GameWorld.GameTimeProp.ElapsedGameTime.TotalSeconds;
            
            //Set the direction towrads the destination(this might be better to put somewhere else?)
            velocity = destination - position;
            velocity.Normalize();

            //Move to the destination
            position += velocity * deltaTime * speed;
            Thread.Sleep(1);
            Debug.WriteLine("workerPosition: " + position);
        }

        /// <summary>
        /// Set the destination of a worker to any gameObject in the Gameworld with a Type matching the parameter
        /// </summary>
        /// <param name="type"></param>
        protected void SetDestination<T>()
        {
            foreach (GameObject gameObject in GameWorld.GameObjectsProp.ToList())
            {
                if (gameObject.GetType() == typeof(T))
                {
                    destination = gameObject.Position;
                }
            }
        }

        public void Extract()
        {

        }

        public void Deposit()
        {

        }

        public void ConstructUnit()
        {

        }
    }
}
