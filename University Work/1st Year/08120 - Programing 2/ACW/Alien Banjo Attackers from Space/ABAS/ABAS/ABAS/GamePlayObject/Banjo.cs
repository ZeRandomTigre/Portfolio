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
    class Banjo : Sprite
    {
        float shootTimer = 1;
        float spawnDelay = 0;

        public static float banjoPositionX = 0;
        public static float banjoPositionY = 0;
        public static float banjoHealth = 1;

        Vector2 velocity;
        float angle;
        
        public enum BanjoType
        {
            plainBanjo,
            hunterBanjo,
            deadlyBanjo
        }

        protected BanjoType banjoType = BanjoType.plainBanjo;

        public Banjo()
        {
            // random values
            int randX = rand.Next(1,4);
            int randY = rand.Next(50, 100);
            int randVal = rand.Next(0, 100);
            
            // randomly set position between 3 points on screen
            if (randX < 2)
                position.X = MainGame.self.Window.ClientBounds.Width / 4;
            else if (randX < 3)
                position.X = MainGame.self.Window.ClientBounds.Width / 2;
            else
                position.X = MainGame.self.Window.ClientBounds.Width - MainGame.self.Window.ClientBounds.Width / 4;

            position.Y = randY;
            width = 30; 
            height = 80;
            speed = 100;
            SetHealth(1);

            // randomly create different banjo types
            if (randVal < 75)
                banjoType = BanjoType.plainBanjo;
            else if (randVal < 90)
                banjoType = BanjoType.hunterBanjo;
            else
                banjoType = BanjoType.deadlyBanjo;

            if (banjoType == BanjoType.deadlyBanjo)
            {
                speed = 120;
                SetHealth(2);
            }
        }

        public override void OnUpdate()
        {
            int randY = rand.Next(30, 60);
            int randX = rand.Next(50, 200);
            
            if (banjoType == BanjoType.deadlyBanjo)
            {
                // banjo movement   
                angle = (float)Math.Atan2(Player.playerPositionY - position.Y, Player.playerPositionX - position.X); // get angle from banjo to player
                velocity = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)); // use angle to set a velocity
                position += velocity * speed * (float)gameTime.ElapsedGameTime.TotalSeconds; // update position with speed and velocity so banjo moves towards player

                // make banjo move towards bottom of screen once banjo has passed below player
                if (position.Y > Player.playerPositionY)
                    position.Y += speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

                // delay between shots
                if (shootTimer <= 0)
                {
                    MainGame.spawningSprites.Add(new StrumFireNote(position, velocity)); // spawns strumfire note with velocity of where the player was when the note was fired
                    shootTimer = 2; // 2 second delay between shots
                }
                else if (shootTimer > 0)
                    shootTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds; // timer counts down per second by 1
            }

            if (banjoType == BanjoType.hunterBanjo)
            {
                // after 5 seconds hunter banjo acts like deadly banjo
                if (spawnDelay < 5)
                {
                    position.X += speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                // deadly banjo code
                else
                {
                    if (speed < 0)
                        speed = speed * -1;
                    angle = (float)Math.Atan2(Player.playerPositionY - position.Y, Player.playerPositionX - position.X);
                    velocity = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
                    position += velocity * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (position.Y > Player.playerPositionY)
                        position.Y += speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                spawnDelay += (float)MainGame.updateTime.ElapsedGameTime.TotalSeconds;                
            }

            // set plain banjo movement
            if (banjoType == BanjoType.plainBanjo)
                position.X += speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // once banjo hits side of screen reverse speed and move down screen by randY
            if (position.X < 0 || position.X > MainGame.self.Window.ClientBounds.Width - width)
            {
                speed = -speed;
                position.Y += randY;
            };

            banjoPositionX = GetPositionX();
            banjoPositionY = GetPositionY();
            banjoHealth = GetHealth();

            // once health has been depleted
            // delete sprite
            // spawn explosion
            // and set score according to banjo type
            if (banjoHealth == 0)
            {
                MainGame.deletingSprites.Add(this);
                MainGame.spawningSprites.Add(new Explosion(new Vector2( position.X, position.Y + height / 2)));
                if (banjoType == BanjoType.plainBanjo)
                    Player.score += 10;
                if (banjoType == BanjoType.hunterBanjo)
                    Player.score += 20;
                if (banjoType == BanjoType.deadlyBanjo)
                    Player.score += 50;
            }


            // if banjo reaches bottom of the screen
            // delete banjo
            // spawn explosion
            // set lives to 0 - gameover
            if (position.Y > MainGame.self.Window.ClientBounds.Height - height)
            {
                MainGame.deletingSprites.Add(this);
                MainGame.spawningSprites.Add(new Explosion(new Vector2(position.X - width / 2, position.Y + height / 2)));
                Player.lives = 0;
            }
        }


        public override void OnDraw()
        {
            if (banjoType == BanjoType.deadlyBanjo)
                spriteBatch.Draw(MainGame.textures["deadlybanjo"], bounds, null, Color.White, 0, 
                    new Vector2(0, 0), SpriteEffects.None, 0.2f); // draw deadly banjo sprite
            else if (banjoType == BanjoType.hunterBanjo)                
                spriteBatch.Draw(MainGame.textures["hunterbanjo"], bounds, null, Color.White, 0, 
                    new Vector2(0, 0), SpriteEffects.None, 0.3f); // draw hunter banjo sprite
            else
                spriteBatch.Draw(MainGame.textures["plainbanjo"], bounds, null, Color.White, 0, 
                    new Vector2(0, 0), SpriteEffects.None, 0.4f); // draw plain banjo type
        }

        public override string GetSaveData()
        {
            string saveString = "Banjo|" + position.X + "|" + position.Y + "|" + velocity.X + 
                "|" + velocity.Y + "|" + GetHealth() + "|" + shootTimer + "|" + spawnDelay + "|";
            if (banjoType == BanjoType.deadlyBanjo)
                saveString += "deadlyBanjo";
            else if (banjoType == BanjoType.hunterBanjo)
                saveString += "hunterBanjo";
            else
                saveString += "plainBanjo";
            return saveString;
        }

        public override void LoadFromData(string[] loadData)
        {
            position.X = float.Parse(loadData[1]);
            position.Y = float.Parse(loadData[2]);
            velocity.X = float.Parse(loadData[3]);
            velocity.Y = float.Parse(loadData[4]);
            SetHealth(int.Parse(loadData[5]));
            shootTimer = float.Parse(loadData[6]);
            spawnDelay = float.Parse(loadData[7]);
            if (loadData[8] == "deadlyBanjo")
            {
                banjoType = BanjoType.deadlyBanjo;
            }
            else if (loadData[8] == "hunterBanjo")
            {
                banjoType = BanjoType.hunterBanjo;
            }
            else
            {
                banjoType = BanjoType.plainBanjo;
            }
        }        
    }
}
