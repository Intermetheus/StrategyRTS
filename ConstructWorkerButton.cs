using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace StrategyRTS
{
    class ConstructWorkerButton : UI
    {
        private Texture2D sprite;
        private Rectangle bounds;
        private bool isHovering;
        public static Mutex drawMutex = new Mutex();

        public ConstructWorkerButton(SpriteBatch spriteBatch, ContentManager content)
        {
            sprite = content.Load<Texture2D>("Build_Button");
            position = new Vector2(1600 / 2 - 300, 900 - 50);
            bounds = new Rectangle((int)position.X, (int)position.Y, sprite.Width, sprite.Height);
            base.spriteBatch = spriteBatch;
            UIThread = new Thread(ThreadMethod);
            Start();
        }

        public override void ThreadMethod()
        {
            while (threadAlive)
            {
                drawMutex.WaitOne();
                spriteBatch.Begin(SpriteSortMode.FrontToBack);

                spriteBatch.Draw(sprite, position, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);

                spriteBatch.End();
                drawMutex.ReleaseMutex();

                if (bounds.Contains(GameWorld.MouseStateProp.Position))
                {
                    isHovering = true;
                }
                else
                {
                    isHovering = false;
                }
            }
        }
    }
}
