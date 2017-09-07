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
    public class Sprite
    {
        protected SpriteBatch spriteBatch 
        { 
            get
            { 
                return MainGame.spriteBatch; 
            } 
        
        }
        protected GameTime gameTime 
        { 
            get 
            { 
                return MainGame.updateTime; 
            } 
        }
        protected Random rand 
        { 
            get 
            { 
                return MainGame.rand; 
            } 
        }

        protected Vector2 position = new Vector2(0, 0); // x & y position of sprite
        protected int width = 64; // width of sprite
        protected int height = 64; // height of sprite
        protected int speed = 5; // speed of sprite ( pixels per second)
        protected int health = 2; // health of sprite

        public virtual float GetPositionX()
        {
            return position.X;
        }

        public float GetPositionY()
        {
            return position.Y;
        }

        public int GetHealth()
        {
            return health;
        }

        public void SetHealth(int newHealth)
        {
            health = newHealth;
        }

        public Rectangle bounds 
        { 
            get 
            { 
                return new Rectangle((int)position.X, (int)position.Y, width, height); 
            } 
        }

        public virtual void OnUpdate() { }

        public virtual void OnDraw() { }

        public virtual string GetSaveData() 
        { 
            return ""; 
        }

        public virtual void LoadFromData(string[] loadData) { }

    }
}
