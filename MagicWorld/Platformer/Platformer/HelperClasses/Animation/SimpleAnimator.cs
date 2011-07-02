using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using MagicWorld.Services;

namespace MagicWorld.HelperClasses.Animation
{
    /// <summary>
    /// Draws simple (as in "never changing or moving") animations like the portal or eyecandy.
    /// I wrote this to code a little bit more in the spirit of the MVC pattern, your opinions about this are highly appreciated.
    /// </summary>
    class SimpleAnimator : DrawableGameComponent, ISimpleAnimator
    {
        #region Animations the Animator can draw and their respective players
        MagicWorld.Animation portalAnimation;
        AnimationPlayer portalPlayer;
        #endregion

        ContentManager content;
        SpriteBatch spriteBatch;
        Dictionary<AnimationPlayer, List<Vector2>> animations;
        ICameraService camera;

        public SimpleAnimator(Game game)
            : base(game)
        {
            animations = new Dictionary<AnimationPlayer, List<Vector2>>();
            content = game.Content;
            game.Components.Add(this);
            game.Services.AddService(typeof(ISimpleAnimator), this);
        }

        protected override void LoadContent()
        {
            portalAnimation = new MagicWorld.Animation("Content/LevelContent/Common/PortalSpriteSheet", 0.2f, 12, content.Load<Texture2D>("LevelContent/Common/PortalSpriteSheet"), 0);
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            InitPlayers();
            base.LoadContent();
        }
        private void InitPlayers()
        {
            portalPlayer = new AnimationPlayer();
            portalPlayer.PlayAnimation(portalAnimation);
            animations.Add(portalPlayer, new List<Vector2>());
        }

        public void Clear()
        {
            animations.Clear();
            InitPlayers();
        }

        public void AddItem(int type, Vector2 pos)
        {
            List<Vector2> positionList;
            switch (type)
            {
                case 0://portals
                    animations.TryGetValue(portalPlayer, out positionList);
                    positionList.Add(pos);
                    animations[portalPlayer] = positionList;
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        public void InitCamera()
        {
            camera = (ICameraService)Game.Services.GetService(typeof(ICameraService));
        }

        public override void Draw(GameTime gameTime)
        {
            if (camera != null)
            {
                spriteBatch.Begin(SpriteSortMode.Immediate,
                    null,
                    null,
                    null,
                    null,
                    null,
                    camera.TransformationMatrix);

                foreach (KeyValuePair<AnimationPlayer, List<Vector2>> keyPair in animations)//For every animation
                {
                    foreach (Vector2 pos in keyPair.Value)//Draw the animation at each position
                    {
                        keyPair.Key.Draw(gameTime, spriteBatch, pos, SpriteEffects.None);
                    }
                }

                spriteBatch.End();
            }
            base.Draw(gameTime);
        }

        public object GetService(Type serviceType)
        {
            return this;
        }
    }
}
