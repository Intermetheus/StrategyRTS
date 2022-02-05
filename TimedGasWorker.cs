using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace StrategyRTS
{
    class TimedGasWorker : Unit
    {
        public override void LoadContent(ContentManager content)
        {
            sprite = content.Load<Texture2D>("worker");
        }
    }
}
