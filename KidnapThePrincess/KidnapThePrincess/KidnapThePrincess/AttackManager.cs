using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace KidnapThePrincess
{
    class AttackManager
    {
        Texture2D sprite;
        
        private TimeSpan lifeSpan = new TimeSpan(0, 0, 0, 0, 200);

        private List<Attack> attacks;

        public List<Attack> Attacks
        {
            get { return attacks; }
            set { attacks = value; }
        }

        public AttackManager(Texture2D tex)
        {
            attacks = new List<Attack>();
            sprite = tex;
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
                attacks.Add(new Attack(sprite, h.Position + h.Direction * 30,h));
                h.CoolDown = h.attackDelay;
            }
        }
    }
}
