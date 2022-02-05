using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace StrategyRTS
{
    public abstract class GameObject
    {
        protected Vector2 position;
        protected Vector2 origin;
        protected Texture2D sprite;
        protected float layerDepth;
        protected Rectangle collisionBox;
        protected float scale;

        public Vector2 Position { get => position; set => position = value; }

        public abstract void LoadContent(ContentManager content);

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, position, null, Color.White, 0, origin, scale, SpriteEffects.None, layerDepth);
        }

        public void CheckCollision(GameObject other)
        {
            if (CollisionBox().Intersects(other.CollisionBox()))
            {
                OnCollision(other);
            }
        }

        public virtual Rectangle CollisionBox()
        {
            return new Rectangle((int)position.X, (int)position.Y, sprite.Width, sprite.Height);
        }

        public virtual void OnCollision(GameObject other)
        {

        }
    }
}
