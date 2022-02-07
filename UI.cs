using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StrategyRTS
{
    abstract class UI
    {
        protected Vector2 position;
        protected Thread UIThread;
        protected bool threadAlive = true;
        protected SpriteBatch spriteBatch;

        public abstract void ThreadMethod();

        public void Start()
        {
            UIThread.Start();
        }
    }
}
