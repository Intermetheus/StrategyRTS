using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace StrategyRTS
{
    class Base : GameObject
    {
        public int mineralAmount;

        public Base()
        {
            mineralAmount = 0;
            position = new Vector2(400, 400);
        }

        public override void LoadContent(ContentManager content)
        {
            sprite = content.Load<Texture2D>("base");
        }
    }
}
