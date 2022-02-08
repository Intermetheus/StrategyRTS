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
    public class Base : GameObject
    {
        private Thread baseThread;
        private static readonly object lockObject = new object();
        private static int mineralAmount;
        private static int gasAmount;
        private string name;
        private Texture2D[] workerSprites;

        public Base()
        {
            scale = 1;
            baseThread = new Thread(BaseThreadUpdate);
            baseThread.IsBackground = true;
            Name = "playerBase";
            MineralAmount = 0;
            position = new Vector2(400, 400);
        }

        public void StartThread()
        {
            baseThread.Start();
        }

        public static int MineralAmount { get => mineralAmount; set => mineralAmount = value; }
        public string Name { get => name; set => name = value; }
        public static int GasAmount { get => gasAmount; set => gasAmount = value; }

        public override void LoadContent(ContentManager content)
        {
            workerSprites = new Texture2D[2];
            workerSprites[0] = content.Load<Texture2D>("mineralWorker");
            workerSprites[1] = content.Load<Texture2D>("gasWorker");


            sprite = content.Load<Texture2D>("base");
        }

        /// <summary>
        /// Base Thread
        /// </summary>
        public void BaseThreadUpdate()
        {
            while (true)
            {
                if (gasAmount >= 10)
                {
                    RemoveGas(1);
                    GameWorld.NewGameObjects.Add(new MineralWorker());
                }
            }
        }

        /// <summary>
        /// Adds minerals to the base. Only one thread at a time because it is locked.
        /// </summary>
        /// <param name="amount"></param>
        public void AddMinerals(int amount)
        {
            lock(lockObject)
            {
                mineralAmount += amount;
            }
        }

        public void AddGas(int amount)
        {
            lock (lockObject)
            {
                GasAmount += amount;
            }
        }

        public void RemoveMinerals(int amount)
        {
            lock (lockObject)
            {
                mineralAmount -= amount;
            }
        }

        public void RemoveGas(int amount)
        {
            lock (lockObject)
            {
                GasAmount -= amount;
            }
        }

        public void SpawnWorker(Unit WorkerType)
        {
            GameWorld.Instantiate(WorkerType);
        }

    }
}
