using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Osu_Simulation
{
    public partial class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D noteTexture;
        Texture2D stageLeftTexture;
        Texture2D stageRightTexture;
        Texture2D stageHintTexture;

        delegate void LoopDelegate();
        LoopDelegate loopDelegate;

        bool ao = false;
        bool so = false;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
            ReadOsuFile(@"D:\TestOsu.osu");
            noteTexture = this.Content.Load<Texture2D>("mania-note1");
            stageLeftTexture = this.Content.Load<Texture2D>("mania-stage-left");
            stageRightTexture = this.Content.Load<Texture2D>("mania-stage-right");
            stageHintTexture = this.Content.Load<Texture2D>("mania-stage-hint");
            PlayGame();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (loopDelegate != null)
                loopDelegate();

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
                

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
#pragma warning disable CS0618 // 형식 또는 멤버는 사용되지 않습니다.


            // X : 80, 150, 220, 290 
            // spriteBatch.Draw(noteTexture, new Rectangle(X, Y, 70, 20), new Rectangle(0, 0 ,noteTexture.Width, noteTexture.Height), Color.White);

            spriteBatch.Draw(stageLeftTexture, new Rectangle(20, 0, 60,490), new Rectangle(0, 0, stageLeftTexture.Width, stageLeftTexture.Height), Color.White);
            spriteBatch.Draw(stageRightTexture, new Rectangle(360, 0, 60,490), new Rectangle(0, 0, stageRightTexture.Width, stageRightTexture.Height), Color.White);
            spriteBatch.Draw(stageHintTexture, new Rectangle(80, 400, 280, 18), new Rectangle(0, 0, stageHintTexture.Width, stageHintTexture.Height), Color.White);
            

            
            for(int i = 0; i < DisplayingHitObjects.Count; i++)
            {
                if (DisplayingHitObjects[i].Y > graphics.PreferredBackBufferHeight + 20)
                {
                    DisplayingHitObjects.Remove(DisplayingHitObjects[i]);
                    continue;
                }

                spriteBatch.Draw(noteTexture, new Rectangle(DisplayingHitObjects[i].X, DisplayingHitObjects[i].Y, 70, 20), new Rectangle(0, 0, noteTexture.Width, noteTexture.Height), Color.White);
                DisplayingHitObjects[i].Y += 10;
            }

#pragma warning restore CS0618 // 형식 또는 멤버는 사용되지 않습니다.
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
