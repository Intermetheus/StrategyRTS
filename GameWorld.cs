using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

namespace StrategyRTS
{
    public class GameWorld : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private static List<GameObject> gameObjects = new List<GameObject>();
        private static List<GameObject> newGameObjects = new List<GameObject>();
        private static List<GameObject> removeGameObjects = new List<GameObject>();

        private static GameTime gameTime; //Get the value of gameTime without using Update(GameTime gameTime)

        public static List<GameObject> GameObjectsProp { get => gameObjects; set => gameObjects = value; }
        public static GameTime GameTimeProp { get => gameTime; set => gameTime = value; }

        public GameWorld()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Mineral myMineral = new Mineral();
            gameObjects.Add(myMineral);

            MineralWorker myWorker = new MineralWorker();
            gameObjects.Add(myWorker);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.LoadContent(Content);
            }
        }

        private bool threadsStarted = false; //REMOVE THIS

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            GameTimeProp = gameTime;

            if (!threadsStarted)
            {
                foreach (GameObject gameObject in gameObjects)
                {
                    if (gameObject is Unit)
                    {
                        //Call StartThread in Unit, even though we are accessing it from GameObject type
                        gameObject.GetType().InvokeMember("StartThread", System.Reflection.BindingFlags.InvokeMethod, null, gameObject, null);
                    }
                }
                threadsStarted = true;
            }

            gameObjects.AddRange(newGameObjects);
            newGameObjects.Clear();

            foreach (GameObject gameObject in removeGameObjects)
            {
                gameObjects.Remove(gameObject);
            }
            

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.FrontToBack);

            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.Draw(spriteBatch);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
