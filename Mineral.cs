using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace StrategyRTS
{
    class Mineral : ResourceDeposit
    {
        public Mineral()
        {
            position = new Vector2(50, 50);
        }
        public override void LoadContent(ContentManager content)
        {
            sprite = content.Load<Texture2D>("mineral");
        }

    }
}
