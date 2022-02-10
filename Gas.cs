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
    class Gas : ResourceDeposit
    {
        private Semaphore gasSemaphore = new Semaphore(3, 3);

        /// <summary>
        /// Limits the amount of threads that can access this class
        /// <para>Only a limited amount of workers can use a mine at the same time</para>
        /// </summary>
        public Semaphore GasSemaphore { get => gasSemaphore; set => gasSemaphore = value; }

        /// <summary>
        /// Sets initial values
        /// </summary>
        public Gas()
        {
            scale = 1;
            position = new Vector2(750, 50);
        }
        /// <summary>
        /// Loads the gas sprite
        /// </summary>
        /// <param name="content"></param>
        public override void LoadContent(ContentManager content)
        {
            sprite = content.Load<Texture2D>("gas");
        }

    }
}
