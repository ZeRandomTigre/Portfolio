using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ABAS
{
    class Player : Sprite
    {
        float shootTimer = 0; // delay between shots
        public static float playerPositionY = 0;
        public static float playerPositionX = 0;

        public static int score = 0;
        public static int lives = 3;
        public static int highScore = 0;
                
        public Player()
        {
            width = 55;
            height = 40;
            position.X = MainGame.self.Window.ClientBounds.Width / 2;
            position.Y = MainGame.self.Window.ClientBounds.Height - 80;
            speed = 500;
        }

        public override void OnUpdate()
        {
            KeyboardState keystate = Keyboard.GetState();

            // move left
            if (keystate.IsKeyDown(Keys.Left))
                position.X -= speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // move right
            if (keystate.IsKeyDown(Keys.Right))
                position.X += speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // left boundry
            if (position.X < 0 + width / 2)
                position.X = 0 + width / 2;
            
            // right boundry
            if (position.X > MainGame.self.Window.ClientBounds.Width - (width * (float)1.5f))
                position.X = MainGame.self.Window.ClientBounds.Width - (width * (float)1.5f);

            // shooting
            if (keystate.IsKeyDown(Keys.Space) && shootTimer <= 0)
            {
                // spawn notes
                MainGame.spawningSprites.Add(new Note(new Vector2 (position.X + 15, position.Y)));
                shootTimer = 0.2f; // delay of 0.2 seconds between shots
            }
            else if (shootTimer > 0)
                shootTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds; // timer counts down per second by 1

            // check if player collides with banjo - if so then banjo dies and player loses 1 life
            for (int i = 0; i < MainGame.banjos.Count; i++)
            {
                if (MainGame.banjos[i] is Banjo && MainGame.banjos[i].bounds.Intersects(bounds))
                {
                    Player.lives -= 1;
                    MainGame.banjos[i].SetHealth(0);
                    return;
                }
            }

            // check if player colldies with stumfire note - if so the note dies and player loses 1 life
            for (int i = 0; i < MainGame.sprites.Count; i++)
            {
                if (MainGame.sprites[i] is StrumFireNote && MainGame.sprites[i].bounds.Intersects(bounds))
                {
                    Player.lives -= 1;
                    MainGame.spawningSprites.Add(new Explosion(new Vector2(StrumFireNote.strumFireNotepositionX, StrumFireNote.strumFireNotepositionY)));
                    MainGame.sprites[i].SetHealth(0);
                    return;
                }
            }

            if (Player.lives == 0)
            {
                MainGame.spawningSprites.Add(new Explosion(new Vector2(playerPositionX, playerPositionY)));
                MainGame.deletingSprites.Add(this);
            }

            playerPositionX = GetPositionX();
            playerPositionY = GetPositionY();

            if (score > highScore)
                highScore = score;

        }
        
        public override void OnDraw()
        {
            spriteBatch.Draw(MainGame.textures["accordian"], bounds, null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0.1f); // draw player sprite
        }

        public override string GetSaveData()
        {
            return "Player|" + position.X + "|" + score + "|" + lives + "|" + highScore + "|";
        }

        public override void LoadFromData(string[] loadData)
        {
            position.X = float.Parse(loadData[1]);
            score = int.Parse(loadData[2]);
            lives = int.Parse(loadData[3]);
            highScore = int.Parse(loadData[4]);
        }
    }
}
