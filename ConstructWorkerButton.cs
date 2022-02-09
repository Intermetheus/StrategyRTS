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
        public bool isHovering;
        public static Mutex drawMutex = new Mutex();

        public ConstructWorkerButton()
        {
            //sprite = content.Load<Texture2D>("Build_Button");
            position = new Vector2(1600 / 2 - 300, 900 - 50);
            //base.spriteBatch = spriteBatch;
            //UIThread = new Thread(ThreadMethod);
            //UIThread.IsBackground = true;
            //Start();
        }

        public override void LoadContent(ContentManager content)
        {
            sprite = content.Load<Texture2D>("Build_Button");
            bounds = new Rectangle((int)position.X, (int)position.Y, sprite.Width, sprite.Height);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, position, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
        }

        public override void Update(GameTime gameTime)
        {
            if (Base.MineralAmount >= 5 && bounds.Contains(GameWorld.MouseStateProp.Position) && GameWorld.LeftMouseButtonReleased())
            {
                GameWorld.MyBase.RemoveMinerals(1);
                GameWorld.NewGameObjects.Add(new TimedGasWorker());
            }
        }


        //public override void ThreadMethod()
        //{
        //    while (threadAlive)
        //    {
        //        drawMutex.WaitOne();

        //        spriteBatch.Draw(sprite, position, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);

        //        drawMutex.ReleaseMutex();

        //        if (bounds.Contains(GameWorld.MouseStateProp.Position))
        //        {
        //            isHovering = true;
        //        }
        //        else
        //        {
        //            isHovering = false;
        //        }
        //    }
        //}
    }
}
