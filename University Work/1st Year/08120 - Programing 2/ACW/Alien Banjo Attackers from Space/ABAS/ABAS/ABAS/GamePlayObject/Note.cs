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
    class Note : Sprite
    {
        public Note(Vector2 spawnPos)
        {
            width = 16;
            height = 16;
            position = spawnPos;
            speed = 250;
        }

        public override void OnUpdate()
        {
            // moves in y direction at speed x
            position.Y -= speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // checks if note intersects banjo
            // if so delete banjo, note and add explosion
            for (int i = 0; i < MainGame.banjos.Count; i++)
            {
                if (MainGame.banjos[i] is Banjo && MainGame.banjos[i].bounds.Intersects(bounds))
                {
                    MainGame.deletingSprites.Add(this);
                    if (MainGame.banjos[i].GetHealth() == 1)
                    {
                        MainGame.banjos[i].SetHealth(0);
                        return;
                    }
                    if (MainGame.banjos[i].GetHealth() == 2)
                        MainGame.banjos[i].SetHealth(1);
                }
            }

            // checks if note interesects strumfire note
            // if so delete note, strumfire note and add explosion
            for (int i = 0; i < MainGame.sprites.Count; i++)
            {
                if (MainGame.sprites[i] is StrumFireNote && MainGame.sprites[i].bounds.Intersects(bounds))
                {
                    MainGame.sprites[i].SetHealth(0);
                    return;
                }
            }

            // if hits top of screen - delete note
            if (position.Y + height < 0)
                MainGame.deletingSprites.Add(this);
        }

        public override void OnDraw()
        {
            spriteBatch.Draw(MainGame.textures["note"], bounds, null, Color.White, 0, new Vector2(0,0), SpriteEffects.None, 0.8f);
        }

        public override string GetSaveData()
        {
            string saveString = "Note|" + position.X + "|" + position.Y + "|";
            return saveString;
        }

        public override void LoadFromData(string[] loadData)
        {
            position.X = float.Parse(loadData[1]);
            position.Y = float.Parse(loadData[2]);
        }
    }
}
