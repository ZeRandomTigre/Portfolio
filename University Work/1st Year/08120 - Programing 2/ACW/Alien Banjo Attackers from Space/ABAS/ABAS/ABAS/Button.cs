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
    class Button
    {
        SpriteFont font;
        Vector2 position;

        Color colorButton = Color.White;

        public Button(SpriteFont newFont)
        {
            font = newFont;
        }

        public bool isClicked;

        public void Update(KeyboardState keyboard, Keys Key)
        {
            KeyboardState keystate = Keyboard.GetState();

            if (keystate.IsKeyDown(Key))
            {
                isClicked = true;
            }
            else
                isClicked = false;
        }

        public void setPosition(Vector2 newPosition)
        {
            position = newPosition;
        }

        public void Draw(SpriteBatch spriteBatch, string fontText)
        {
            spriteBatch.DrawString(font, fontText,
                position, colorButton);
        }
    }
}
