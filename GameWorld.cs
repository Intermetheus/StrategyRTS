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

        private static readonly object createWorkerLock = new object();

        private static List<GameObject> gameObjects = new List<GameObject>();
        private static List<GameObject> newGameObjects = new List<GameObject>();
        private static List<GameObject> removeGameObjects = new List<GameObject>();

        private static List<UI> UIObjects = new List<UI>();

        private static Base myBase = new Base();

        private static ButtonState leftMouseButton = ButtonState.Released;   // Tracks the current state of the left mouse button

        /// <summary>
        /// Allows easier access to the gameObjects List
        /// </summary>
        public static List<GameObject> GameObjectsProp { get => gameObjects; set => gameObjects = value; }

        /// <summary>
        /// Allows easier access to the GameTime Class
        /// </summary>
        public static GameTime GameTimeProp { get => gameTime; set => gameTime = value; }

        /// <summary>
        /// Allows easier access to the Base Class
        /// </summary>
        public static Base MyBase { get => myBase; set => myBase = value; }

        /// <summary>
        /// Allows easier to the MouseState Struct
        /// </summary>
        public static MouseState MouseStateProp { get => mouseState; set => mouseState = value; }

        /// <summary>
        /// Allows easier access to the SpriteFont
        /// </summary>
        public static SpriteFont Arial { get => arial; set => arial = value; }

        /// <summary>
        /// Allows easier access to the newGameObjects List
        /// </summary>
        public static List<GameObject> NewGameObjects { get => newGameObjects; set => newGameObjects = value; }

        /// <summary>
        /// Allows easier access to the lock object
        /// <para>Prevents multiple threads from creating race conditions</para>
        /// </summary>
        public static object CreateWorkerLock => createWorkerLock;

        /// <summary>
        /// Sets the applications size to 1600x900
        /// </summary>
        public GameWorld()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1600;
            graphics.PreferredBackBufferHeight = 900;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        /// <summary>
        /// Adds the ResourceDeposits, UI and the Base to the gameObjects List
        /// </summary>
        protected override void Initialize()
        {
            Mineral myMineral = new Mineral();
            Gas myGas = new Gas();
            newGameObjects.Add(myMineral);
            newGameObjects.Add(myGas);

            newGameObjects.Add(MyBase);

            for (int i = 0; i < 2; i++)
            {
                newGameObjects.Add(new MineralWorker());
            }

            for (int i = 0; i < 0; i++)
            {
                newGameObjects.Add(new TimedGasWorker());
            }

            UIObjects.Add(new ConstructWorkerButton());
            UIObjects.Add(new ResourceCounter());

            base.Initialize();
        }

        /// <summary>
        /// Runs the LoadContent() Method on every object in UIObject and gameObject.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Arial = Content.Load<SpriteFont>("arial");

            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.LoadContent(Content);
            }

            foreach (UI UIObject in UIObjects)
            {
                UIObject.LoadContent(Content);
            }
        }

        /// <summary>
        /// Runs the Update() method on the UIObjects. Adds/Removes new objects to the game. Checks collisions between objects.
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            GameTimeProp = gameTime;

            mouseState = Mouse.GetState();
            
            //This area is locked in order to prevent destination array from being changed. Therefore, creating a race condition.
            //In the Unit Class the SetDestination() method loops through the GameObjects list.
            //The game will crash if this list is changed while that loop occurs.
            lock(CreateWorkerLock)
            {
                gameObjects.AddRange(NewGameObjects);
            
                foreach (GameObject newGameObject in NewGameObjects.ToList())
                {
                    newGameObject.LoadContent(Content);

                    if (newGameObject is Unit)
                    {
                        //Call StartThread in Unit, even though we are accessing it from GameObject type
                        newGameObject.GetType().InvokeMember("StartThread", System.Reflection.BindingFlags.InvokeMethod, null, newGameObject, null);
                    }
                    if (newGameObject is Base)
                    {
                        //Call StartThread in Unit, even though we are accessing it from GameObject type
                        newGameObject.GetType().InvokeMember("StartThread", System.Reflection.BindingFlags.InvokeMethod, null, newGameObject, null);
                    }
                }

                NewGameObjects.Clear();
            }

            //This area is locked for similliar reasons as above.
            //If an object is removed while SetDestination() is copying the gameObjects List.
            //The program will return a nullReferenceException because gameObject was null.
            lock (CreateWorkerLock)
            {
                foreach (GameObject gameObject in removeGameObjects)
                {
                    gameObjects.Remove(gameObject);
                }
            }

            //Check collisions
            foreach (GameObject gameObject in gameObjects)
            {
                foreach (GameObject other in gameObjects)
                {
                    gameObject.CheckCollision(other);
                }
            }

            foreach (UI UIObject in UIObjects)
            {
                UIObject.Update(gameTime);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Runs the relevant Draw() Methods on gameObjects.
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //ConstructWorkerButton.drawMutex.WaitOne();
            spriteBatch.Begin(SpriteSortMode.FrontToBack);

            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.Draw(spriteBatch);
            }

            foreach (UI UIObject in UIObjects)
            {
                UIObject.Draw(spriteBatch);
            }

#if DEBUG
            //Draws numbers on the workers to identify them.
            //These numbers will be 0 unless they are made using a specific constructor.
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

            //ConstructWorkerButton.drawMutex.ReleaseMutex();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Adds newly instantiated gameobjects to gameObject list
        /// </summary>
        /// <param name="gameObject">Gameobject to be added</param>
        public static void Instantiate(GameObject gameObject)
        {
            NewGameObjects.Add(gameObject);
        }

        /// <summary>
        /// Removes gameObjects from gameObject list
        /// </summary>
        /// <param name="gameObject">GameObject to be removed</param>
        public static void Destroy(GameObject gameObject)
        {
            removeGameObjects.Add(gameObject);
        }

        /// <summary>
        /// Registers when the left mouse button is being released
        /// </summary>
        /// <returns></returns>
        public static bool LeftMouseButtonReleased()
        {
            if (leftMouseButton == ButtonState.Pressed && Mouse.GetState().LeftButton == ButtonState.Released)
            {
                leftMouseButton = Mouse.GetState().LeftButton;
                return true;
            }
            else
            {
                leftMouseButton = Mouse.GetState().LeftButton;
                return false;
            }
        }
    }
}
