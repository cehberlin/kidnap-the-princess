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

namespace KidnapThePrincess
{
    class Level
    {
        private Vector2 castlePosition;
        public Vector2 CastlePosition
        {
            get { return castlePosition; }
            set { castlePosition = value; }
        }
        Texture2D castleTex;
        Texture2D carriageTex;
        Texture2D bruteTex;
        Texture2D widowTex;
        Texture2D darknightTex;
        Texture2D goblinTex;
        Texture2D treeTex;
        Texture2D hut1Tex;
        Texture2D hut2Tex;
        Texture2D haystackTex;
        Texture2D crateTex;
        Texture2D playAreaTex;
        Texture2D P1MarkerTex;
        Texture2D P2MarkerTex;
        Texture2D templarTex;
        Texture2D attackTex;
        private Camera2d camera;

        public Camera2d Camera
        {
            get { return camera; }
            set { camera = value; }
        }

        Goblin princessCarrier;

        internal Goblin PrincessCarrier
        {
            get { return princessCarrier; }
            set { princessCarrier = value; }
        }

        private List<Hero> heroes;

        public List<Hero> Heroes
        {
            get { return heroes; }
            set { heroes = value; }
        }

        private List<Enemy> enemies;

        internal List<Enemy> Enemies
        {
            get { return enemies; }
            set { enemies = value; }
        }

        private List<Vector2> treePositions;

        private Rectangle playArea;
        /// <summary>
        /// The area of the screen in which the players and enemies can move.
        /// </summary>
        public Rectangle PlayArea
        {
            get { return playArea; }
            set { playArea = value; }
        }
        public Rectangle carriagRec;

        int P1HeroIndex;
        int P2HeroIndex;

        #region LoadLevelContent //Constructor, Init Methods...

        Game1 game;

        public Level(Game1 game)
        {
            this.game = game;
            P1HeroIndex = 1;
            P2HeroIndex = 2;
            castlePosition = new Vector2(0, 0);
            heroes = new List<Hero>();
            enemies = new List<Enemy>();
            camera = new Camera2d();
            camera.Pos = new Vector2(0, 300);
            treePositions = new List<Vector2>();
            playArea = new Rectangle((int)castlePosition.X - 300, (int)castlePosition.Y, 600, 2000);
        }

        public void Load(ContentManager c)
        {
            addTextures(c);
            Init();
        }

        public void Init()
        {
            treePositions.Clear();
            addTrees();
            heroes.Clear();
            addHeroes();

            enemies.Clear();
        }

        private void addTrees()
        {
            //add trees limiting the scenery
            for (int i = 0; i < 500; i++)
            {
                treePositions.Add(new Vector2(playArea.Location.X - treeTex.Width / 2, i * treeTex.Height));
                treePositions.Add(new Vector2(playArea.Location.X - treeTex.Width / 2, i * treeTex.Height / 2));

                treePositions.Add(new Vector2(playArea.Location.X + playArea.Width - treeTex.Width / 2, i * treeTex.Height));
                treePositions.Add(new Vector2(playArea.Location.X + playArea.Width - treeTex.Width / 2, i * treeTex.Height / 2));
            }
        }

        private void addTextures(ContentManager c)
        {
            //Debug texture
            playAreaTex = c.Load<Texture2D>("brown");
            //Load textures for scenery
            castleTex = c.Load<Texture2D>("castle");
            carriageTex = c.Load<Texture2D>("carriage");
            treeTex = c.Load<Texture2D>("tree");
            templarTex = c.Load<Texture2D>("templar");
            crateTex = c.Load<Texture2D>("crate");
            hut1Tex = c.Load<Texture2D>("hut1");
            hut2Tex = c.Load<Texture2D>("hut2");
            haystackTex = c.Load<Texture2D>("haystack");
            P1MarkerTex = c.Load<Texture2D>("P1Marker");
            P2MarkerTex = c.Load<Texture2D>("P2Marker");

            attackTex = c.Load<Texture2D>("attack");

            carriagRec = new Rectangle(0, 2000, carriageTex.Width, carriageTex.Height);

            //Load textures for heroes
            goblinTex = c.Load<Texture2D>("goblin");
            darknightTex = c.Load<Texture2D>("darkknight");
            widowTex = c.Load<Texture2D>("widow");
            bruteTex = c.Load<Texture2D>("brute");
        }

        private void addHeroes()
        {
            //add heroes to list
            Goblin g = new Goblin(goblinTex, playArea, enemies);
            g.Position = new Vector2(castlePosition.X, castlePosition.Y + castleTex.Height);
            g.Destination = new Vector2(carriagRec.Center.X, carriagRec.Center.Y);
            heroes.Add(g);

            princessCarrier = g; //makes it easier for reference

            Hero h = new Brute(bruteTex, playArea, enemies);
            h.Position = new Vector2(castlePosition.X - 2 * h.sprite.Width, castlePosition.Y + castleTex.Height);
            h.IsActive = true;
            heroes.Add(h);

            h = new Knight(darknightTex, playArea, enemies);
            h.Position = new Vector2(castlePosition.X + h.sprite.Width, castlePosition.Y + castleTex.Height);
            h.IsActive = true;
            heroes.Add(h);

            h = new Widow(widowTex, playArea, enemies);
            h.Position = new Vector2(castlePosition.X - h.sprite.Width, castlePosition.Y + castleTex.Height);
            heroes.Add(h);

        }

        #endregion LoadLevelContent

        #region UpdateLevelContent

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(playAreaTex, playArea, Color.White);
            //Draw scenery
            foreach (Vector2 pos in treePositions)
            {
                sb.Draw(treeTex, pos, Color.White);
            }
            sb.Draw(castleTex, castlePosition, Color.White);
            sb.Draw(carriageTex, carriagRec, Color.White);
            foreach (Hero h in heroes)
            {
                if (h.Freezed)
                {
                    sb.Draw(h.sprite, h.Position, Color.Blue);
                }
                else
                {
                    if (h.IsAttacking)
                    {
                        //draw text
                        sb.Draw(attackTex, h.Position + new Vector2(10, 10), Color.White);
                        sb.Draw(h.sprite, h.Position, Color.Red);
                    }
                    else
                    {
                        sb.Draw(h.sprite, h.Position, Color.White);
                    }
                }


            }
            sb.Draw(P1MarkerTex, heroes[P1HeroIndex].Position, Color.White);
            sb.Draw(P2MarkerTex, heroes[P2HeroIndex].Position, Color.White);

            foreach (Enemy e in enemies)
            {
                sb.Draw(e.sprite, e.Position, Color.White);
            }
        }

        Random spawnRandom = new Random();
        public void SpawnEnemy()
        {
            //check if temple is still visible
            Enemy e = new Templar(templarTex, heroes);
            if (camera.Pos.Y > (game.Window.ClientBounds.Height))
            {
                e.Position = new Vector2(castlePosition.X, castlePosition.Y + castleTex.Height);
            }
            else
            {
                //enemies can come from top and bottom
                if (spawnRandom.Next(2) == 1)
                {
                    e.Position = new Vector2(castlePosition.X + spawnRandom.Next(-100, 100), camera._pos.Y + (game.Window.ClientBounds.Height / 2));
                }
                else
                {
                    e.Position = new Vector2(castlePosition.X + spawnRandom.Next(-100, 100), camera._pos.Y - (game.Window.ClientBounds.Height / 2));
                }
            }
            enemies.Add(e);
        }

        public void Update(GameTime time)
        {
            if (GameState.getInstance(this).Status == GameState.State.RUN)
            {
                camera.Pos = heroes[0].Position;


                //let the enemies carry our princess to the castle
                if (princessCarrier.Attacked)
                {
                    princessCarrier.Destination = castlePosition;
                }
                else
                {
                    princessCarrier.Destination = new Vector2(carriagRec.Center.X, carriagRec.Center.Y);
                }

                for (int i = 0; i < heroes.Count; i++)
                {

                    Hero h = heroes[i];
                    if (!h.IsActive && i != 0)//makes the inactive heroes follow the princess
                    {
                        h.Destination = heroes[0].Position + new Vector2(40, -80);
                    }
                    h.Update(time);
                    if (h.IsActive) h.Direction = Vector2.Zero;
                }


                foreach (Enemy e in enemies)
                {
                    e.Update(time);
                }
            }
        }

        public void MoveHeroLeft(int player)
        {
            if (player == 0)
            {
                if (!heroes[P1HeroIndex].Freezed)
                    heroes[P1HeroIndex].Direction = new Vector2(-1, heroes[P1HeroIndex].Direction.Y);
            }
            else
                if (!heroes[P2HeroIndex].Freezed)
                    heroes[P2HeroIndex].Direction = new Vector2(-1, heroes[P2HeroIndex].Direction.Y);
        }
        public void MoveHeroRight(int player)
        {
            if (player == 0)
            {
                if (!heroes[P1HeroIndex].Freezed)
                    heroes[P1HeroIndex].Direction = new Vector2(1, heroes[P1HeroIndex].Direction.Y);
            }
            else
                if (!heroes[P2HeroIndex].Freezed)
                    heroes[P2HeroIndex].Direction = new Vector2(1, heroes[P2HeroIndex].Direction.Y);
        }
        public void MoveHeroUp(int player)
        {
            if (player == 0)
            {
                if (!heroes[P1HeroIndex].Freezed)
                    heroes[P1HeroIndex].Direction = new Vector2(heroes[P1HeroIndex].Direction.X, -1);
            }
            else
                if (!heroes[P2HeroIndex].Freezed)
                    heroes[P2HeroIndex].Direction = new Vector2(heroes[P2HeroIndex].Direction.X, -1);
        }
        public void MoveHeroDown(int player)
        {
            if (player == 0)
            {
                if (!heroes[P1HeroIndex].Freezed)
                    heroes[P1HeroIndex].Direction = new Vector2(heroes[P1HeroIndex].Direction.X, 1);
            }
            else
                if (!heroes[P2HeroIndex].Freezed)
                    heroes[P2HeroIndex].Direction = new Vector2(heroes[P2HeroIndex].Direction.X, 1);
        }
        public void HeroAttack(int player)
        {
            if (player == 0)
            {
                if (!heroes[P1HeroIndex].Freezed)
                    heroes[P1HeroIndex].IsAttacking = true;
            }
            else
            {
                if (!heroes[P2HeroIndex].Freezed)
                    heroes[P2HeroIndex].IsAttacking = true;
            }
        }

        public void SwitchHero(int player)
        {
            if (player == 0)
            {
                heroes[P1HeroIndex].Direction = Vector2.Zero;
                heroes[P1HeroIndex].IsActive = false;
                P1HeroIndex++;
                P1HeroIndex %= 4;
                if (P1HeroIndex == 0) P1HeroIndex++;
                if (P1HeroIndex == P2HeroIndex) P1HeroIndex++;
                heroes[P1HeroIndex].IsActive = true;
            }
            else
            {
                heroes[P2HeroIndex].Direction = Vector2.Zero;
                heroes[P2HeroIndex].IsActive = false;
                P2HeroIndex++;
                P2HeroIndex %= 4;
                if (P2HeroIndex == 0) P2HeroIndex++;
                if (P2HeroIndex == P1HeroIndex)
                {
                    P2HeroIndex++;
                    P2HeroIndex %= 4;
                }
                heroes[P2HeroIndex].IsActive = true;
            }
        }

        #endregion UpdateLevelContent
    }
}
