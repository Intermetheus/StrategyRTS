using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace StrategyRTS
{
    /// <summary>
    /// Subclass of ResourceDeposit
    /// </summary>
    class Mineral : ResourceDeposit
    {
        private Semaphore mineralSemaphore = new Semaphore(5, 5);

        /// <summary>
        /// Limits the amount of threads that can access this class
        /// <para>Only a limited amount of workers can use a mine at the same time</para>
        /// </summary>
        public Semaphore MineralSemaphore { get => mineralSemaphore; set => mineralSemaphore = value; }

        /// <summary>
        /// Sets the initial values
        /// </summary>
        public Mineral()
        {
            scale = 1;
            position = new Vector2(50, 50);
        }

        /// <summary>
        /// Loads the minerals sprite
        /// </summary>
        /// <param name="content"></param>
        public override void LoadContent(ContentManager content)
        {
            sprite = content.Load<Texture2D>("mineral");
        }

    }
}
