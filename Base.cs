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
        private int mineralAmount;
        private string name;
        private Texture2D[] workerSprites;

        public Base()
        {
            scale = 1;
            baseThread = new Thread(Update);
            name = "playerBase";
            MineralAmount = 0;
            position = new Vector2(400, 400);
        }

        public void StartThread()
        {
            baseThread.Start();
        }

        public int MineralAmount { get => mineralAmount; set => mineralAmount = value; }

        public override void LoadContent(ContentManager content)
        {
            workerSprites = new Texture2D[2];
            workerSprites[0] = content.Load<Texture2D>("worker");
            workerSprites[1] = content.Load<Texture2D>("worker");


            sprite = content.Load<Texture2D>("base");
        }

        public void Update()
        {
            while (true)
            {
                if (CollisionBox().Contains(GameWorld.MouseStateProp.Position)
                    //&& GameWorld.MouseStateProp.LeftButton == ButtonState.Pressed
                    )
                {
                    SpawnWorker(new MineralWorker(workerSprites[0]));
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

        public void SpawnWorker(Unit WorkerType)
        {
            GameWorld.Instantiate(WorkerType);
        }

    }
}
