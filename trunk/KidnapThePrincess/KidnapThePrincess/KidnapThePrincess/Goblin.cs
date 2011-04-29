using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace KidnapThePrincess
{
    class Goblin:Hero
    {
        Boolean attacked = false;

        public Boolean Attacked
        {
            get { return attacked; }
            set { attacked = value; }
        }


        public Goblin(Texture2D tex,Rectangle area,List<Enemy> enemies) :
            base(tex, area,enemies)
        {
            Speed = 0.4f;
        }


         public override void Update()
        {
            attacked = false;
             foreach(Enemy e in enemies){
                   if(GeometryHelper.Intersects(Bounds,e.Bounds)){
                       attacked = true;
                       break;
                   }
            }
             base.Update();              

         }
    }
}
