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
        private static SpriteFont arial;
        private static GameTime gameTime; //Get the value of gameTime without using Update(GameTime gameTime)
        private static MouseState mouseState;

        private static List<GameObject> gameObjects = new List<GameObject>();
        private static List<GameObject> newGameObjects = new List<GameObject>();
        private static List<GameObject> removeGameObjects = new List<GameObject>();

        private static List<UI> UIObjects = new List<UI>();

        private static Base myBase = new Base();

        public static List<GameObject> GameObjectsProp { get => gameObjects; set => gameObjects = value; }
        public static GameTime GameTimeProp { get => gameTime; set => gameTime = value; }
        public static Base MyBase { get => myBase; set => myBase = value; }
        public static MouseState MouseStateProp { get => mouseState; set => mouseState = value; }
        public static SpriteFont Arial { get => arial; set => arial = value; }

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

            UIObjects.Add(new ConstructWorkerButton());
            UIObjects.Add(new ResourceCounter());

            base.Initialize();
        }

        protected override void LoadContent()
        {
            Arial = Content.Load<SpriteFont>("arial");
            spriteBatch = new SpriteBatch(GraphicsDevice);

            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.LoadContent(Content);
            }

            foreach (UI UIObject in UIObjects)
            {
                UIObject.LoadContent(Content);
            }
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

            foreach (UI UIObject in UIObjects)
            {
                UIObject.Draw(spriteBatch);
            }

            //spriteBatch.DrawString(Arial, "Minerals: " + MyBase.MineralAmount, new Vector2(20,20), Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);

#if DEBUG
            //Draws numbers on the workers to identify them
            foreach (GameObject gameObject in gameObjects)
            {
                if (gameObject is Unit)
                {
                    Unit mObject = (Unit)gameObject;
                    spriteBatch.DrawString(Arial, mObject.Id.ToString(), new Vector2(mObject.Position.X, mObject.Position.Y), Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
                }
                if (gameObject is Base)
                {
                    Base bObject = (Base)gameObject;
                    spriteBatch.DrawString(Arial, bObject.Name, new Vector2(bObject.Position.X - 20, bObject.Position.Y - 20), Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
                }
            }
#endif
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
