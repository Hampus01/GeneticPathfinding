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

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        int numberOfAgents = 100;
        int numberOfActions = 500;
        int mutationChance = 2;
        int maximumSpeed = 5;

        Vector2 goal = new Vector2(600, 100);

        Vector2 start;
        int currentStep = 0;
        int currentGeneration = 0;
        int reachedGoal = 0;
        Texture2D agentTexture;
        Texture2D goalTexture;
        Random rnd;

        Agent[] agents;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }


        protected override void Initialize()
        {
            rnd = new Random();

            agentTexture = Content.Load<Texture2D>("Agent");
            goalTexture = Content.Load<Texture2D>("Goal");

            start = new Vector2(graphics.PreferredBackBufferWidth / 2,
                                graphics.PreferredBackBufferHeight / 2);

            agents = new Agent[numberOfAgents];

            // Create starting population
            for (int i = 0; i < numberOfAgents; i++)
            {
                agents[i] = new Agent(new List<Vector2>(), start, agentTexture);
                for (int j = 0; j < numberOfActions; j++)
                {
                    agents[i].Actions.Add(GenerateAction());
                }
            }


            base.Initialize();
        }


        protected override void LoadContent()
        {

            spriteBatch = new SpriteBatch(GraphicsDevice);

        }


        protected override void UnloadContent()
        {

        }


        protected override void Update(GameTime gameTime)
        {
            // Let all agnets perform their next action
            if (currentStep < numberOfActions)
            {
                foreach (Agent a in agents)
                {
                    if (!a.ReachedGoal)
                    {
                        a.Position += a.Actions[currentStep];
                        Rectangle agentBox = new Rectangle((int)a.Position.X,
                            (int)a.Position.Y, a.Texture.Width, a.Texture.Height);
                        Rectangle goalBox = new Rectangle((int)goal.X,
                            (int)goal.Y, goalTexture.Width, goalTexture.Height);
                        if (agentBox.Intersects(goalBox))
                        {
                            a.ReachedGoal = true;
                            a.ActionsToReachGoal = currentStep;
                        }
                    }
                }

                currentStep++;
            }
            else
            {
                // Calculate fitness for all agents
                foreach (Agent a in agents)
                {
                    a.Fitness = (int)Math.Sqrt(Math.Pow((a.Position.X - goal.X), 2) +
                        Math.Pow((a.Position.Y - goal.Y), 2));
                }

                // Order agents by lowest fitness
                agents = agents.OrderBy(p => p.Fitness).ToArray<Agent>();

                // Remake a new population for the next generation

                //Test

                // This code only creates a new random population and must be changed in order for the agents to show progress over time.
                for (int i = 0; i < numberOfAgents; i++)
                {
                    agents[i] = new Agent(new List<Vector2>(), start, agentTexture);
                    for (int j = 0; j < numberOfActions; j++)
                    {
                        agents[i].Actions.Add(GenerateAction());
                    }
                }


                currentStep = 0;
            }



            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            spriteBatch.Draw(goalTexture, goal, Color.White);

            foreach (Agent a in agents)
            {
                a.Draw(spriteBatch);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private Vector2 GenerateAction()
        {
            int r = rnd.Next(0, 4);
            if (r == 0) { return new Vector2(0, -rnd.Next(1, maximumSpeed)); } // Up
            else if (r == 1) { return new Vector2(0, rnd.Next(1, maximumSpeed)); } // Down
            else if (r == 2) { return new Vector2(-rnd.Next(1, maximumSpeed), 0); } // Left
            else { return new Vector2(rnd.Next(1, maximumSpeed), 0); } // Right
        }
    }
}