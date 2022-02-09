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
    /// UI element, which displays resource amounts
    /// </summary>
    class ResourceCounter : UI
    {
        private string resourceAmount;

        public ResourceCounter()
        {
            position = new Vector2(20, 5);
            //base.spriteBatch = spriteBatch;
            //UIThread = new Thread(ThreadMethod);
            //Start();
        }

        public override void LoadContent(ContentManager content)
        {
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(GameWorld.Arial, "Minerals: " + Base.MineralAmount, position, Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
            spriteBatch.DrawString(GameWorld.Arial, "Gas: " + Base.GasAmount, position + new Vector2(0, 18), Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
        }

        //public override void ThreadMethod()
        //{
        //    while (threadAlive)
        //    {
        //        spriteBatch.DrawString(arial, "Minerals: " + MyBase.MineralAmount, position, Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
        //    }
        //}
    }
}
