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

        /// <summary>
        /// LoadContent() is abstract so that each subclass can load their own Content
        /// </summary>
        /// <param name="content"></param>
        public abstract void LoadContent(ContentManager content);

        /// <summary>
        /// Draw() is a virtual so it can be overrided
        /// </summary>
        /// <param name="spriteBatch"></param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, position, null, Color.White, 0, origin, scale, SpriteEffects.None, layerDepth);
        }

        /// <summary>
        /// Checks if this gameObject has a collision with another gameObject
        /// </summary>
        /// <param name="other">GameObject</param>
        public void CheckCollision(GameObject other)
        {
            if (CollisionBox().Intersects(other.CollisionBox()))
            {
                OnCollision(other);
            }
        }

        /// <summary>
        /// Creates a rectangle based on the size and position
        /// <para>Can be overriden to make custom collisionBoxes</para>
        /// </summary>
        /// <returns></returns>
        public virtual Rectangle CollisionBox()
        {
            return new Rectangle((int)position.X, (int)position.Y, sprite.Width, sprite.Height);
        }

        /// <summary>
        /// Runs code when an object has a collision
        /// </summary>
        /// <param name="other">GameObject</param>
        public virtual void OnCollision(GameObject other)
        {

        }
    }
}
