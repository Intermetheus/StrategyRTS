using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace StrategyRTS
{
    /// <summary>
    /// Manages UI elements
    /// </summary>
    abstract class UI
    {
        protected Vector2 position;
        //protected SpriteBatch spriteBatch;

        public abstract void LoadContent(ContentManager content);
        public abstract void Draw(SpriteBatch spriteBatch);
        public virtual void Update(GameTime gameTime)
        {

        }

        //protected Thread UIThread;
        //protected bool threadAlive = true;

        //public abstract void ThreadMethod();

        //public void Start()
        //{
        //    UIThread.Start();
        //}
    }
}
