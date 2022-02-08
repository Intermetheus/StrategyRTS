using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace StrategyRTS
{
    class Gas : ResourceDeposit
    {
        private Semaphore gasSemaphore = new Semaphore(2, 2);

        public Semaphore GasSemaphore { get => gasSemaphore; set => gasSemaphore = value; }

        public Gas()
        {
            scale = 1;
            position = new Vector2(750, 50);
        }

        public override void LoadContent(ContentManager content)
        {
            sprite = content.Load<Texture2D>("gas");
        }

    }
}
