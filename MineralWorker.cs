using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace StrategyRTS
{
    class MineralWorker : Unit
    {
        public MineralWorker()
        {
            position = new Vector2(100, 100);
        }

        public override void LoadContent(ContentManager content)
        {
            sprite = content.Load<Texture2D>("worker");
        }
    }
}
