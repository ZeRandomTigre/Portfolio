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
    class StrumFireNote : Sprite
    {
        Vector2 velocity = new Vector2();

        public static float strumFireNotepositionX = 0;
        public static float strumFireNotepositionY = 0;
        public int strumFireNoteHealth = 1;

        public StrumFireNote(Vector2 spawnPos, Vector2 spawnVelocity)
        {
            width = 16;
            height = 16;
            position = spawnPos;
            speed = 250;
            velocity = spawnVelocity;
        }

        public override void OnUpdate()
        {
            strumFireNotepositionX = GetPositionX();
            strumFireNotepositionY = GetPositionY();
            strumFireNoteHealth = GetHealth();

            // move in direction of velocity and at speed x
            position += velocity * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // if goes out of bounds delete
            if (position.Y + height > MainGame.self.Window.ClientBounds.Height)
                MainGame.deletingSprites.Add(this);

            // if no health delte with explosion
            if (strumFireNoteHealth == 0)
            {
                MainGame.deletingSprites.Add(this);
                MainGame.spawningSprites.Add(new Explosion(new Vector2(position.X, position.Y)));
            }
        }

        public override void OnDraw()
        {
            // render texture
            spriteBatch.Draw(MainGame.textures["note"], bounds, null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0.7f);
        }

        public override string GetSaveData()
        {
            string saveString = "StrumFireNote|" + position.X + "|" + position.Y + "|" + velocity.X + "|" + velocity.Y + "|";
            return saveString;
        }

        public override void LoadFromData(string[] loadData)
        {
            position.X = float.Parse(loadData[1]);
            position.Y = float.Parse(loadData[2]);
            velocity.X = float.Parse(loadData[3]);
            velocity.Y = float.Parse(loadData[4]);
        }
    }
}
