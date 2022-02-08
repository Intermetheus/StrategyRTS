using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace StrategyRTS
{
    public class GameWorld : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private SpriteFont arial;
        private static GameTime gameTime; //Get the value of gameTime without using Update(GameTime gameTime)
        private static MouseState mouseState;

        private static List<GameObject> gameObjects = new List<GameObject>();
        private static List<GameObject> newGameObjects = new List<GameObject>();
        private static List<GameObject> removeGameObjects = new List<GameObject>();

        private static Base myBase = new Base();

        public static List<GameObject> GameObjectsProp { get => gameObjects; set => gameObjects = value; }
        public static GameTime GameTimeProp { get => gameTime; set => gameTime = value; }
        public static Base MyBase { get => myBase; set => myBase = value; }
        public static MouseState MouseStateProp { get => mouseState; set => mouseState = value; }

        public GameWorld()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1600;
            graphics.PreferredBackBufferHeight = 900;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Mineral myMineral = new Mineral();
            Gas myGas = new Gas();
            gameObjects.Add(myMineral);
            gameObjects.Add(myGas);

            gameObjects.Add(MyBase);

            for (int i = 0; i < 5; i++)
            {
                gameObjects.Add(new MineralWorker());
            }

            for (int i = 0; i < 5; i++)
            {
                gameObjects.Add(new TimedGasWorker());
            }

            base.Initialize();
        }

        protected override void LoadContent()
        {
            arial = Content.Load<SpriteFont>("arial");
            spriteBatch = new SpriteBatch(GraphicsDevice);

            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.LoadContent(Content);
            }
            new ConstructWorkerButton(spriteBatch, Content);
        }

        private bool threadsStarted = false; //starts threads of workers created in initialize()

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            GameTimeProp = gameTime;

            mouseState = Mouse.GetState();

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
                myBase.StartThread();
                threadsStarted = true;
            }

            gameObjects.AddRange(newGameObjects);
            newGameObjects.Clear();

            foreach (GameObject gameObject in removeGameObjects)
            {
                gameObjects.Remove(gameObject);
            }

            //Check collisions
            foreach (GameObject gameObject in gameObjects)
            {
                foreach (GameObject other in gameObjects)
                {
                    gameObject.CheckCollision(other);
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            ConstructWorkerButton.drawMutex.WaitOne();
            spriteBatch.Begin(SpriteSortMode.FrontToBack);

            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.Draw(spriteBatch);
            }

            spriteBatch.DrawString(arial, "Minerals: " + MyBase.MineralAmount, new Vector2(20,20), Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);

            spriteBatch.End();
            ConstructWorkerButton.drawMutex.ReleaseMutex();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Adds newly instantiated gameobjects to gameObject list
        /// </summary>
        /// <param name="gameObject">Gameobject to be added</param>
        public static void Instantiate(GameObject gameObject)
        {
            newGameObjects.Add(gameObject);
        }

        /// <summary>
        /// Removes gameObjects from gameObject list
        /// </summary>
        /// <param name="gameObject">GameObject to be removed</param>
        public static void Destroy(GameObject gameObject)
        {
            removeGameObjects.Add(gameObject);
        }
    }
}
