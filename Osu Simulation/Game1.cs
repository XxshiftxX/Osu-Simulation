using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace Osu_Simulation
{
    public partial class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D noteTexture;
        Texture2D longNoteStartTexture;
        Texture2D stageLeftTexture;
        Texture2D stageRightTexture;
        Texture2D stageHintTexture;

        SoundEffect NoteSound;

        delegate void LoopDelegate();
        LoopDelegate loopDelegate;

        bool ao = false;
        bool so = false;

        Dictionary<Keys, bool> keyPressed = new Dictionary<Keys, bool>();

        SpriteFont font;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
            ReadOsuFile(@"C:\Users\dsm2017\AppData\Local\osu!\Songs\123456 Lite Show Magic - Crack traxxxx\Lite Show Magic - Crack traxxxx (LeiN-) [4K ADV].osu");
            noteTexture = this.Content.Load<Texture2D>("mania-note1");
            longNoteStartTexture = this.Content.Load<Texture2D>("mania-note1H");
            stageLeftTexture = this.Content.Load<Texture2D>("mania-stage-left");
            stageRightTexture = this.Content.Load<Texture2D>("mania-stage-right");
            stageHintTexture = this.Content.Load<Texture2D>("mania-stage-hint");
            font = this.Content.Load<SpriteFont>("ComboFont");
            NoteSound = this.Content.Load<SoundEffect>("normal-hitnormal");
            graphics.PreferredBackBufferHeight = 600;
            graphics.ApplyChanges();
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

            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                if(!keyPressed[Keys.D])
                {
                    KeyInput(0, System.DateTime.Now);
                    NoteSound.CreateInstance().Play();
                }

                keyPressed[Keys.D] = true;
            }
            if (Keyboard.GetState().IsKeyUp(Keys.D))
            {
                keyPressed[Keys.D] = false;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.F))
            {
                if (!keyPressed[Keys.F])
                {
                    KeyInput(1, System.DateTime.Now);
                    NoteSound.CreateInstance().Play();
                }

                keyPressed[Keys.F] = true;
            }
            if (Keyboard.GetState().IsKeyUp(Keys.F))
            {
                keyPressed[Keys.F] = false;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.J))
            {
                if (!keyPressed[Keys.J])
                {
                    KeyInput(2, System.DateTime.Now);
                    NoteSound.CreateInstance().Play();
                }

                keyPressed[Keys.J] = true;
            }
            if (Keyboard.GetState().IsKeyUp(Keys.J))
            {
                keyPressed[Keys.J] = false;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.K))
            {
                if (!keyPressed[Keys.K])
                {
                    KeyInput(3, System.DateTime.Now);
                    NoteSound.CreateInstance().Play();
                }

                keyPressed[Keys.K] = true;
            }
            if (Keyboard.GetState().IsKeyUp(Keys.K))
            {
                keyPressed[Keys.K] = false;
            }

            Health -= 0.3f;
            if (Health > 300)
                Health = 300;
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

#pragma warning disable CS0618 // 형식 또는 멤버는 사용되지 않습니다.

            Texture2D writer;
            spriteBatch.Begin();
            for (int i = 0; i < DisplayingHitObjects.Count; i++)
            {
                DisplayingHitObjects[i].Y += 17;
                if (DisplayingHitObjects[i].GetType)
                    writer = noteTexture;
                else
                    writer = longNoteStartTexture;

                spriteBatch.Draw(writer,
                    new Rectangle(DisplayingHitObjects[i].X, DisplayingHitObjects[i].Y, 70, 20),
                    new Rectangle(0, 0, noteTexture.Width, noteTexture.Height),
                    Color.White);
            }

            spriteBatch.End();
            spriteBatch.Begin();
            // X : 80, 150, 220, 290 
            // spriteBatch.Draw(noteTexture, new Rectangle(X, Y, 70, 20), new Rectangle(0, 0 ,noteTexture.Width, noteTexture.Height), Color.White);

            spriteBatch.Draw(stageLeftTexture, new Rectangle(20, 0, 60,600), new Rectangle(0, 0, stageLeftTexture.Width, stageLeftTexture.Height), Color.White);
            spriteBatch.Draw(stageRightTexture, new Rectangle(360, 0, 60,600), new Rectangle(0, 0, stageRightTexture.Width, stageRightTexture.Height), Color.White);
            spriteBatch.Draw(stageHintTexture, new Rectangle(80, 492, 280, 18), new Rectangle(0, 0, stageHintTexture.Width, stageHintTexture.Height), Color.White);
            spriteBatch.DrawString(font, $"{GamePoint}", new Vector2(400, 100), Color.White);
            spriteBatch.DrawString(font, $"{Combo} Combo!", new Vector2(180, 100), Color.White);
            spriteBatch.DrawString(font, judgeMessage, new Vector2(180, 300), Color.White);

            /*
            Texture2D rect = new Texture2D(graphics.GraphicsDevice, (int)Health, 20);
            Color[] data = new Color[(int)Health *20];
            for (int i = 0; i < data.Length; ++i) data[i] = Color.Red;
            rect.SetData(data);
            spriteBatch.Draw(rect, new Vector2(374, graphics.PreferredBackBufferHeight - 20), Color.Red);
            */

            spriteBatch.End();
            
            base.Draw(gameTime);

#pragma warning restore CS0618 // 형식 또는 멤버는 사용되지 않습니다.
        }
    }
}
