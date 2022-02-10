using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace StrategyRTS
{
    /// <summary>
    /// Base Class that stores resources and has a Thread
    /// </summary>
    public class Base : GameObject
    {
        private Thread baseThread;
        private static readonly object lockObject = new object();
        private static int mineralAmount;
        private static int gasAmount;
        private string name;
        private Texture2D[] workerSprites;

        /// <summary>
        /// Allows easier access to the mineralAmount
        /// </summary>
        public static int MineralAmount { get => mineralAmount; set => mineralAmount = value; }

        /// <summary>
        /// Allows easier access to the gasAmount
        /// </summary>
        public static int GasAmount { get => gasAmount; set => gasAmount = value; }

        /// <summary>
        /// Allows easier access to the Name string
        /// </summary>
        public string Name { get => name; set => name = value; }


        /// <summary>
        /// Sets the initial values of the Base, and creates its Thread.
        /// </summary>
        public Base()
        {
            scale = 1;
            baseThread = new Thread(BaseThreadUpdate);
            baseThread.IsBackground = true;
            Name = "playerBase";
            MineralAmount = 0;
            GasAmount = 0;
            position = new Vector2(400, 400);
        }

        /// <summary>
        /// Starts the Thread
        /// </summary>
        public void StartThread()
        {
            baseThread.Start();
        }
        public override void LoadContent(ContentManager content)
        {
            workerSprites = new Texture2D[2];
            workerSprites[0] = content.Load<Texture2D>("mineralWorker");
            workerSprites[1] = content.Load<Texture2D>("gasWorker");


            sprite = content.Load<Texture2D>("base");
        }

        /// <summary>
        /// Base Thread. Creates a new MineralWorker if the player has more than 10 gas.
        /// </summary>
        public void BaseThreadUpdate()
        {
            while (true)
            {
                if (gasAmount >= 5)
                {
                    RemoveGas(5);
                    GameWorld.NewGameObjects.Add(new MineralWorker());
                }
            }
        }

        /// <summary>
        /// Adds minerals to the base
        /// </summary>
        /// <param name="amount">Amount of minerals</param>
        public void AddMinerals(int amount)
        {
            lock(lockObject)
            {
                mineralAmount += amount;
            }
        }

        /// <summary>
        /// Adds gas to the base
        /// </summary>
        /// <param name="amount">Amount of gas</param>
        public void AddGas(int amount)
        {
            lock (lockObject)
            {
                GasAmount += amount;
            }
        }

        /// <summary>
        /// Removes minerals from the base
        /// </summary>
        /// <param name="amount">Amount of minerals</param>
        public void RemoveMinerals(int amount)
        {
            lock (lockObject)
            {
                mineralAmount -= amount;
            }
        }

        /// <summary>
        /// Removes gas from the base
        /// </summary>
        /// <param name="amount">Amount of gas</param>
        public void RemoveGas(int amount)
        {
            lock (lockObject)
            {
                GasAmount -= amount;
            }
        }
    }
}
