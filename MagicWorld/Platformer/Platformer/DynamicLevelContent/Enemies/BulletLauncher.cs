using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using MagicWorld.DynamicLevelContent.SwitchRiddles;

namespace MagicWorld.DynamicLevelContent.Enemies
{
    /// <summary>
    /// this object launches arbitary bullet enemy objects
    /// could be used like a vulcano..
    /// </summary>
    class BulletLauncher:BlockElement,IActivation
    {
        protected Vector2 launchVelocity;

        protected double delayTimeMs;

        protected double currentDelay;

        protected bool activated = true;

        protected String bulletSprite;

        protected bool enableGravityForBullets;

        public BulletLauncher(String texture, String bulletSprite, Level level, Vector2 position,
            Vector2 launchVelocity, double delayTimeMs,bool enableGravityForBullets=true)
            :base(texture,CollisionType.Platform,level,position)
        {
            this.delayTimeMs=delayTimeMs;
            currentDelay = delayTimeMs;
            this.launchVelocity = launchVelocity;
            this.enableGravityForBullets = enableGravityForBullets;
            this.bulletSprite = bulletSprite;
        }

        public override void Update(GameTime gameTime)
        {
            if (activated)
            {
                //disabled sound
                //if ((level.Player.Position - Position).Length() < 400)
                //{
                //    audioService.playSoundLoop(Audio.SoundType.lava);
                //}
                //else
                //{
                //    audioService.stopSoundLoop(Audio.SoundType.lava, false);
                //}
                currentDelay -= gameTime.ElapsedGameTime.TotalMilliseconds;
                if (currentDelay <= 0)
                {
                    currentDelay = delayTimeMs;

                    Bullet bullet = new Bullet(level, Position, launchVelocity, bulletSprite, enableGravityForBullets);

                    level.GeneralColliadableGameElements.Add(bullet);
                }
            }

            base.Update(gameTime);
        }

        public void activate()
        {
            activated = true;
        }

        public void deactivate()
        {
            activated = false;
        }
    }
}
