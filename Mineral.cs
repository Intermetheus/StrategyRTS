using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace StrategyRTS
{
    class Mineral : ResourceDeposit
    {
        public override void LoadContent(ContentManager content)
        {
            sprite = content.Load<Texture2D>("mineral");
        }
    }
}
