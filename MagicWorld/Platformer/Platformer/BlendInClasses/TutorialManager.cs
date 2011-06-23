using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MagicWorld.Services;

namespace MagicWorld.BlendInClasses
{
    public class TutorialManager : DrawableGameComponent
    {
        //TODO: add some basic scripting/placement trough level editor/add texture/add custom font
        List<TutorialInstruction> instructions;
        SpriteFont font;
        SpriteBatch spriteBatch;
        IPlayerService playerService;

        public void AddInstruction(String text, Vector2 pos)
        {
            TutorialInstruction t = new TutorialInstruction(text, pos);
            t.Manager = this;
            instructions.Add(t);
        }

        public void AddInstructionSet(List<TutorialInstruction> instructs)
        {
            instructions.AddRange(instructs);
        }

        public TutorialManager(Game game)
            : base(game)
        {
            instructions = new List<TutorialInstruction>();
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            font = Game.Content.Load<SpriteFont>("Instructions/InstructionFont");
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            playerService = (IPlayerService)Game.Services.GetService(typeof(IPlayerService));
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            playerService = (IPlayerService)Game.Services.GetService(typeof(IPlayerService));
            for (int i = 0; i < instructions.Count; i++)
            {
                if (instructions[i].IsActive)
                {
                    if (instructions[i].DisplayTime <= TimeSpan.Zero)
                        instructions.Remove(instructions[i]);
                    else
                        instructions[i].DisplayTime = instructions[i].DisplayTime.Subtract(gameTime.ElapsedGameTime);
                }
                else
                {
                    if (instructions[i].Position.X < playerService.Position.X)
                    {
                        instructions[i].IsActive = true;
                    }
                }
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            for (int i = 0; i < instructions.Count; i++)
            {
                if (instructions[i].IsActive)
                {
                    spriteBatch.DrawString(font, instructions[i].Text, instructions[i].Position, instructions[i].Color);
                }
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
