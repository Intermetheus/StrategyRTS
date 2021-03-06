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
    /// UI element, which can be clicked
    /// </summary>
    class ConstructWorkerButton : UI
    {
        private Texture2D sprite;
        private Rectangle bounds;
        private int gasWorkerCost = 5;
        //public static Mutex drawMutex = new Mutex();        

        public ConstructWorkerButton()
        {
            position = new Vector2(1600 / 2 - 400, 900 - 150);
            //base.spriteBatch = spriteBatch;
            //UIThread = new Thread(ThreadMethod);
            //UIThread.IsBackground = true;
            //Start();
        }

        /// <summary>
        /// Loads button sprite and creates rectangle based on sprite size, for clicking button
        /// </summary>
        /// <param name="content"></param>
        public override void LoadContent(ContentManager content)
        {
            sprite = content.Load<Texture2D>("Build_Button");
            bounds = new Rectangle((int)position.X, (int)position.Y, sprite.Width, sprite.Height);
        }

        /// <summary>
        /// Draws button and text explaining button function
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, position, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            spriteBatch.DrawString(GameWorld.Arial, $"New Gas Worker: {gasWorkerCost} Mineral", position + new Vector2(-100, 50), Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f); ;
        }

        /// <summary>
        /// Checks if button has been clicked and resources are sufficient, then spawns worker
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            if (Base.MineralAmount >= gasWorkerCost && bounds.Contains(GameWorld.MouseStateProp.Position) && GameWorld.LeftMouseButtonReleased())
            {
                GameWorld.MyBase.RemoveMinerals(gasWorkerCost);
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
