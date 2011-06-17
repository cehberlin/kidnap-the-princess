using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MagicWorld.BlendInClasses
{
    class TutorialManager : DrawableGameComponent
    {
        //TODO: add some basic scripting/placement trough level editor/add texture/add custom font
        List<TutorialInstruction> instructions;
        SpriteFont font;
        SpriteBatch spriteBatch;

        public void AddInstruction(String text)
        {
            TutorialInstruction t = new TutorialInstruction(text);
            t.Manager = this;
            instructions.Add(t);
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
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < instructions.Count; i++)
            {
                if (instructions[i].IsActive)
                {
                    if (instructions[i].DisplayTime <= TimeSpan.Zero)
                        instructions.Remove(instructions[i]);
                    else
                        instructions[i].DisplayTime = instructions[i].DisplayTime.Subtract(gameTime.ElapsedGameTime);
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
