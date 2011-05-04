using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace KidnapThePrincess
{
    class AttackManager
    {
        Texture2D[] attackTexs;
        Vector2 attackOffset;
        Vector2 bruteAttackOffset;
        Vector2 knightAttackOffset;
        Vector2 widowAttackOffset;
        SoundEffect[] attackSounds;

        private TimeSpan lifeSpan = new TimeSpan(0, 0, 0, 0, 800);

        private List<Attack> attacks;

        public List<Attack> Attacks
        {
            get { return attacks; }
            set { attacks = value; }
        }

        public AttackManager(Texture2D[] texs, SoundEffect[] e)
        {
            attacks = new List<Attack>();
            attackTexs = texs;
            attackSounds = e;
            attackOffset = new Vector2();
        }

        public void Update(GameTime time)
        {
            RemoveOldAttacks();
            foreach (Attack a in attacks)
            {
                if (a.Duration.Subtract(lifeSpan) < TimeSpan.Zero)
                {
                    a.Update(time);
                }
            }
        }

        public void Draw(SpriteBatch sb)
        {
            foreach (Attack a in attacks)
            {
                a.Draw(sb);
            }
        }

        public void RemoveOldAttacks()
        {
            for (int i = 0; i < attacks.Count; i++)
            {
                if (attacks[i].Duration.Subtract(lifeSpan) > TimeSpan.Zero)
                    attacks.Remove(attacks[i]);
            }
        }

        public void AddAttack(Hero h)
        {
            if (h.CanAttack)
            {
                //set offset of attack
                if (h.Direction.X < 0) 
                {
                    attackOffset.X = -2*h.sprite.Width / 2;
                }
                else if (h.Direction.X > 0) 
                {
                    attackOffset.X = h.sprite.Width / 2;
                }
                else
                {
                    attackOffset.X = -20;
                }
                if (h.Direction.Y < 0) 
                {
                    attackOffset.Y = -h.sprite.Height/2;
                }
                else if (h.Direction.Y > 0) 
                {
                    attackOffset.Y = h.sprite.Height-20;
                }
                else
                {
                    attackOffset.Y = h.sprite.Width / 2;
                }

                attacks.Add(new Attack(attackTexs[h.Type - 1], h.Position + attackOffset, h));
                h.CoolDown = h.attackDelay;
                attackSounds[h.Type - 1].Play();
            }
        }
    }
}
