using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ABAS
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class MainGame : Game
    {
        public static MainGame self;
        public static GraphicsDeviceManager graphics;
        public static SpriteBatch spriteBatch;
        public static GameTime updateTime;
        public static Random rand = new Random();

        private float spawnDelay = 0;
        private float spawnRate = 1;

        SpriteFont font;
        SpriteFont mainMenuFont;


        public static List<Sprite> banjos = new List<Sprite>();
        public static List<Sprite> sprites = new List<Sprite>();
        public static List<Sprite> deletingSprites = new List<Sprite>();
        public static List<Sprite> spawningSprites = new List<Sprite>();
        public static Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();

        enum GameState
        {
            MainMenu,
            NewGame,
            LoadGame,
            GameOver,
        }
        GameState currentGameState = GameState.MainMenu;

        Button playButton;
        Button loadButton;
        Button mainMenuButton;
        Button gameOverButton;

        public MainGame()
        {
            self = this;
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            graphics.IsFullScreen = false;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
        }

        public void LoadGame()
        {
            if (File.Exists("save.txt"))
            {
                using (StreamReader sr = new StreamReader("save.txt"))
                {
                    string readIn = "";
                    string[] splitData;
                    while (!sr.EndOfStream)
                    {
                        readIn = sr.ReadLine();
                        splitData = readIn.Split('|');

                        if (splitData[0] == "Player")
                        {
                            Player player = new Player();
                            player.LoadFromData(splitData);
                            sprites.Add(player);
                        }

                        if (splitData[0] == "Banjo")
                        {
                            Banjo banjo = new Banjo();
                            banjo.LoadFromData(splitData);
                            banjos.Add(banjo);
                        }

                        if (splitData[0] == "Note")
                        {
                            Note note = new Note(new Vector2(0, 0));
                            note.LoadFromData(splitData);
                            sprites.Add(note);
                        }

                        if (splitData[0] == "StrumFireNote")
                        {
                            StrumFireNote strumFireNote = new StrumFireNote(new Vector2(0, 0), new Vector2(0, 0));
                            strumFireNote.LoadFromData(splitData);
                            sprites.Add(strumFireNote);
                        }
                    }
                }
            }
            else
                RestartGame();
        }

        public void SaveGame()
        {
            using (StreamWriter sw = new StreamWriter("save.txt", false))
            {
                for (int i = 0; i < sprites.Count; i++)
                {
                    string toSave = sprites[i].GetSaveData();
                    if (toSave != "")
                        sw.WriteLine(toSave);
                }

                for (int i = 0; i < banjos.Count; i++)
                {
                    string toSave = banjos[i].GetSaveData();
                    if (toSave != "")
                        sw.WriteLine(toSave);
                }
            }
        }

        public void RestartGame()
        {            
            sprites.Clear();
            banjos.Clear();

            sprites.Add(new Player());
            Player.lives = 3;
            Player.score = 0;
            spawnDelay = 0;
            spawnRate = 1;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            textures.Add("accordian",       Content.Load<Texture2D>("accordian"));                        
            textures.Add("plainbanjo",      Content.Load<Texture2D>("plainBanjo"));
            textures.Add("hunterbanjo",     Content.Load<Texture2D>("hunterBanjo"));
            textures.Add("deadlybanjo",     Content.Load<Texture2D>("deadlyBanjo"));
            textures.Add("note",            Content.Load<Texture2D>("note"));
            textures.Add("explosion",       Content.Load<Texture2D>("explosion"));
            textures.Add("background",      Content.Load<Texture2D>("Background"));

            font = Content.Load<SpriteFont>("font");
            mainMenuFont = Content.Load<SpriteFont>("mainMenuFont");

            playButton = new Button(mainMenuFont);
            playButton.setPosition(new Vector2(20, 60));

            loadButton = new Button(mainMenuFont);
            loadButton.setPosition(new Vector2(20, 120));

            mainMenuButton = new Button(mainMenuFont);

            gameOverButton = new Button(mainMenuFont);
            gameOverButton.setPosition(new Vector2(20, 60));
        }

        
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            updateTime = gameTime;

            KeyboardState keystate = Keyboard.GetState();
                        
            switch (currentGameState)
            {
                case GameState.MainMenu:
                    {
                        if (playButton.isClicked == true)
                        {
                            currentGameState = GameState.NewGame;
                            playButton.isClicked = false;
                            loadButton.isClicked = false;
                            mainMenuButton.isClicked = false;
                            RestartGame();
                        }
                        playButton.Update(keystate, Keys.Space);
                        if (loadButton.isClicked == true)
                        {
                            currentGameState = GameState.LoadGame;
                            playButton.isClicked = false;
                            loadButton.isClicked = false;
                            mainMenuButton.isClicked = false;
                            LoadGame();
                        }
                        loadButton.Update(keystate, Keys.L);

                        if (keystate.IsKeyDown(Keys.Escape))
                            this.Exit();
                        break;
                    }
                case GameState.NewGame:
                    {
                        if (spawnDelay > spawnRate)
                        {
                            spawnBanjo();
                            spawnDelay = 0;
                        }

                        spawnDelay += (float)updateTime.ElapsedGameTime.TotalSeconds;

                        for (int i = 0; i < spawningSprites.Count; i++)
                            sprites.Add(spawningSprites[i]);
                        spawningSprites.Clear();

                        for (int i = 0; i < sprites.Count; i++)
                            sprites[i].OnUpdate();

                        for (int i = 0; i < banjos.Count; i++)
                            banjos[i].OnUpdate();

                        for (int i = 0; i < deletingSprites.Count; i++)
                            sprites.Remove(deletingSprites[i]);

                        for (int i = 0; i < deletingSprites.Count; i++)
                            banjos.Remove(deletingSprites[i]);
                        deletingSprites.Clear();

                        if (Player.lives == 0)
                            currentGameState = GameState.GameOver;

                        if (mainMenuButton.isClicked == true)
                        {
                            SaveGame();
                            currentGameState = GameState.MainMenu;
                            playButton.isClicked = false;
                            loadButton.isClicked = false;
                            mainMenuButton.isClicked = false;
                        }
                        mainMenuButton.Update(keystate, Keys.P);

                        break;
                    }
                case GameState.LoadGame:
                    {
                        if (spawnDelay > spawnRate)
                        {
                            spawnBanjo();
                            spawnDelay = 0;
                        }

                        spawnDelay += (float)updateTime.ElapsedGameTime.TotalSeconds;

                        for (int i = 0; i < spawningSprites.Count; i++)
                            sprites.Add(spawningSprites[i]);
                        spawningSprites.Clear();

                        for (int i = 0; i < sprites.Count; i++)
                            sprites[i].OnUpdate();

                        for (int i = 0; i < banjos.Count; i++)
                            banjos[i].OnUpdate();

                        for (int i = 0; i < deletingSprites.Count; i++)
                            sprites.Remove(deletingSprites[i]);

                        for (int i = 0; i < deletingSprites.Count; i++)
                            banjos.Remove(deletingSprites[i]);
                        deletingSprites.Clear();

                        if (Player.lives == 0)
                            currentGameState = GameState.GameOver;
                        
                        if (mainMenuButton.isClicked == true)
                        {
                            SaveGame();
                            currentGameState = GameState.MainMenu;
                            playButton.isClicked = false;
                            loadButton.isClicked = false;
                            mainMenuButton.isClicked = false;
                        }
                        mainMenuButton.Update(keystate, Keys.P);

                        break;
                    }
                case GameState.GameOver:
                    {
                        if (spawnDelay > spawnRate)
                        {
                            spawnBanjo();
                            spawnDelay = 0;
                        }

                        spawnDelay += (float)updateTime.ElapsedGameTime.TotalSeconds;

                        for (int i = 0; i < spawningSprites.Count; i++)
                            sprites.Add(spawningSprites[i]);
                        spawningSprites.Clear();

                        for (int i = 0; i < sprites.Count; i++)
                            sprites[i].OnUpdate();

                        for (int i = 0; i < banjos.Count; i++)
                            banjos[i].OnUpdate();

                        for (int i = 0; i < deletingSprites.Count; i++)
                            sprites.Remove(deletingSprites[i]);

                        for (int i = 0; i < deletingSprites.Count; i++)
                            banjos.Remove(deletingSprites[i]);
                        deletingSprites.Clear();

                        if (gameOverButton.isClicked == true)
                        {
                            currentGameState = GameState.MainMenu;
                            playButton.isClicked = false;
                            loadButton.isClicked = false;
                            mainMenuButton.isClicked = false;
                        }
                        gameOverButton.Update(keystate, Keys.P);

                        if (File.Exists("save.txt"))
                        {
                            File.Delete("save.txt");
                        }
                        break;
                    }
            }            

            base.Update(gameTime);
        }

        public void spawnBanjo()
        {
            if (banjos.Count < 300)
                banjos.Add(new Banjo());
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            
            switch (currentGameState)
            {
                case GameState.MainMenu:
                    {
                        spriteBatch.Draw(textures["background"], new Rectangle(0, 0, MainGame.self.Window.ClientBounds.Width, MainGame.self.Window.ClientBounds.Height),
                             null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 1);
                        spriteBatch.DrawString(font, "High Score: " + Player.highScore.ToString(), new Vector2(20, 20), Color.White);
                        playButton.Draw(spriteBatch, "Press Space to start a new game");
                        loadButton.Draw(spriteBatch, "Press 'L' to load a previous game");
                        break;
                    }
                case GameState.NewGame:
                    {
                        spriteBatch.Draw(textures["background"], new Rectangle(0, 0, MainGame.self.Window.ClientBounds.Width, MainGame.self.Window.ClientBounds.Height),
                            null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 1);

                        spriteBatch.DrawString(font, "Score: " + Player.score.ToString(),
                            new Vector2(20, MainGame.self.Window.ClientBounds.Height - 40), Color.White);

                        spriteBatch.DrawString(font, "Lives: " + Player.lives.ToString(),
                            new Vector2(MainGame.self.Window.ClientBounds.Width - 120, MainGame.self.Window.ClientBounds.Height - 40), Color.White);
                        spriteBatch.DrawString(font, "High Score: " + Player.highScore.ToString(), new Vector2(20, 20), Color.White);

                        for (int i = 0; i < sprites.Count; i++)
                            sprites[i].OnDraw();

                        for (int i = 0; i < banjos.Count; i++)
                            banjos[i].OnDraw();
                        break;
                    }
                case GameState.LoadGame:
                    {
                        spriteBatch.Draw(textures["background"], new Rectangle(0, 0, MainGame.self.Window.ClientBounds.Width, MainGame.self.Window.ClientBounds.Height),
                            null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 1);

                        spriteBatch.DrawString(font, "Score: " + Player.score.ToString(),
                            new Vector2(20, MainGame.self.Window.ClientBounds.Height - 40), Color.White);

                        spriteBatch.DrawString(font, "Lives: " + Player.lives.ToString(),
                            new Vector2(MainGame.self.Window.ClientBounds.Width - 120, MainGame.self.Window.ClientBounds.Height - 40), Color.White);
                        spriteBatch.DrawString(font, "High Score: " + Player.highScore.ToString(), new Vector2(20, 20), Color.White);

                        for (int i = 0; i < sprites.Count; i++)
                            sprites[i].OnDraw();

                        for (int i = 0; i < banjos.Count; i++)
                            banjos[i].OnDraw();
                        break;
                    }
                case GameState.GameOver:
                    {
                        spriteBatch.Draw(textures["background"], new Rectangle(0, 0, MainGame.self.Window.ClientBounds.Width, MainGame.self.Window.ClientBounds.Height),
                             null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 1);
                        spriteBatch.DrawString(font, "Score: " + Player.score.ToString(),
                            new Vector2(20, MainGame.self.Window.ClientBounds.Height - 40), Color.White);
                        spriteBatch.DrawString(font, "Lives: " + Player.lives.ToString(),
                            new Vector2(MainGame.self.Window.ClientBounds.Width - 120, MainGame.self.Window.ClientBounds.Height - 40), Color.White);
                        spriteBatch.DrawString(font, "High Score: " + Player.highScore.ToString(), new Vector2(20, 20), Color.White);
                        gameOverButton.Draw(spriteBatch, "GameOver! Press 'P' to return to the main menu");

                        for (int i = 0; i < sprites.Count; i++)
                            sprites[i].OnDraw();

                        for (int i = 0; i < banjos.Count; i++)
                            banjos[i].OnDraw();
                        break;
                    }
            }

            base.Draw(gameTime);

            spriteBatch.End();
        }
    }
}
