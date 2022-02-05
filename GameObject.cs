using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace StrategyRTS
{
    abstract class GameObject
    {
        protected Vector2 position;
        protected Vector2 origin;
        protected Texture2D sprite;
        protected float layerDepth;
        protected Rectangle collisionBox;

        public abstract void LoadContent(ContentManager content);

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, position, null, Color.White, 0, origin, 1, SpriteEffects.None, layerDepth);
        }
    }
}
