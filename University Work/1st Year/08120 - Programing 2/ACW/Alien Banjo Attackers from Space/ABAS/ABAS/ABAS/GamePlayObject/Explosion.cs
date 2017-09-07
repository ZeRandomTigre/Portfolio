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
    class Explosion : Sprite
    {
        float explosionLength = 0;
        int frame = 0;

        public Explosion(Vector2 spawnPos)
        {
            position = spawnPos;
            width = 64;
            height = 64;
        }

        public override void OnUpdate()
        {
            // explosion length increases by 1 every second
            explosionLength += (float)gameTime.ElapsedGameTime.TotalSeconds;

            // number of frames
            frame = (int)Math.Floor(explosionLength * 20);

            // end animation after explosion length > 1
            if (explosionLength > 1)
                MainGame.deletingSprites.Add(this);
        }

        public override void OnDraw()
        {
            spriteBatch.Draw(MainGame.textures["explosion"], bounds, new Rectangle((int)(frame % 5) * 96, frame / 5 * 96, 96, 96), 
                Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0.9f);
        }
    }
}
