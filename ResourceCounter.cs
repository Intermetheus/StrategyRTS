using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StrategyRTS
{
    class ResourceCounter : UI
    {
        private string resourceAmount;

        public ResourceCounter(SpriteBatch spriteBatch)
        {
            position = new Vector2(20, 20);
            base.spriteBatch = spriteBatch;
            UIThread = new Thread(ThreadMethod);
            Start();
        }

        public override void ThreadMethod()
        {
            while (threadAlive)
            {
                //spriteBatch.DrawString(arial, "Minerals: " + MyBase.MineralAmount, position, Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
            }
        }
    }
}
