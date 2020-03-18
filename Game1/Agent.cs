using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Game1
{
    class Agent
    {
        public Vector2 Position { get; set; }
        public Texture2D Texture { get; set; }
        public List<Vector2> Actions { get; set; }
        public int Fitness { get; set; }
        public bool ReachedGoal { get; set; }
        public int ActionsToReachGoal { get; set; }

        public Agent(List<Vector2> actions, Vector2 position, Texture2D texture)
        {
            this.Actions = actions;
            this.Position = position;
            this.Texture = texture;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.Texture, this.Position, Color.White);
        }
    }
}