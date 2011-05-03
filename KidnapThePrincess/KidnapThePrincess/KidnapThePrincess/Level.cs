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
        Vector2 v=Vector2.Zero;
        SpriteFont aFont;
        //vector used temporarely tho set the camera position
        Vector2 camPos = Vector2.Zero;

        #region Declarations
        private Vector2 castlePosition;
        public Vector2 CastlePosition
        {
            get { return castlePosition; }
            set { castlePosition = value; }
        }
        Texture2D debugTex;
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
        Texture2D powTex;
        private List<Camera2d> cams;

        public List<Camera2d> Cameras
        {
            get { return cams; }
            set { cams = value; }
        }

        private Camera2d cameraP1;

        public Camera2d CameraP1
        {
            get { return cameraP1; }
            set { cameraP1 = value; }
        }

        public Camera2d CameraP2
        {
            get { return cameraP2; }
            set { cameraP2 = value; }
        }

        private Camera2d princessCam;

        public Camera2d PrincessCamera
        {
            get { return princessCam; }
            set { princessCam = value; }
        }

        private Camera2d cameraP2;


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

        private List<Vector2> treePositions;
        private Vector2 markerOffset;

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

        //Used for Splitscreen gaming
        private bool isP1Offscreen;
        public bool IsP1Offscreen
        {
            get { return isP1Offscreen; }
            set { isP1Offscreen = value; }
        }
        private bool isP2Offscreen;
        public bool IsP2Offscreen
        {
            get { return isP2Offscreen; }
            set { isP2Offscreen = value; }
        }

        GameObjectManager gameObjectManager;
        public GameObjectManager GameObjectManager 
        {
            get { return gameObjectManager; }
            set { gameObjectManager = value; }
        }

        CollisionManager collisionManager;
        AttackManager attackManager;
        EnemyManager enemyManager;
        
        #endregion

        #region LoadLevelContent //Constructor, Init Methods...

        Game1 game;
        
        public Level(Game1 game)
        {
            this.game = game;
            P1HeroIndex = 1;
            P2HeroIndex = -1;
            castlePosition = new Vector2(0, 0);
            heroes = new List<Hero>();
            //enemies = new List<Enemy>();
            treePositions = new List<Vector2>();
            playArea = new Rectangle((int)castlePosition.X - 300, (int)castlePosition.Y, 600, 2000);
            isP1Offscreen = false;
            isP2Offscreen = false;
            cams = new List<Camera2d>();
            gameObjectManager = new GameObjectManager();
            collisionManager = new CollisionManager();
        }

        public void Load(ContentManager c)
        {
            aFont = c.Load<SpriteFont>("Fonts\\Font");
            princessCam = new Camera2d(game.Window.ClientBounds.Width, game.Window.ClientBounds.Height);
            cameraP2 = new Camera2d(game.Window.ClientBounds.Width, game.Window.ClientBounds.Height);
            cameraP1 = new Camera2d(game.Window.ClientBounds.Width, game.Window.ClientBounds.Height);
            cameraP1.Pos = new Vector2(0, 300);
            CameraP2.Pos = new Vector2(0, 300);
            cams.Add(princessCam);
            cams.Add(cameraP1);
            cams.Add(cameraP2);
            addTextures(c);
            Init();
        }

        public void Init()
        {
            treePositions.Clear();
            addTrees();
            AddGameObjects();
            heroes.Clear();
            addHeroes();
            enemyManager.Init();
        }

        private void AddGameObjects()
        {
            //Long term goal: Level editor ;)
            for (int i = 0; i < 10; i++)
            {
                Haystack h = new Haystack(haystackTex, new Vector2(200, 200 + i * 600));
                gameObjectManager.AddObject(h);
                Hut hut = new Hut(hut1Tex, new Vector2(200 - haystackTex.Width, 200 + i * 600));
                gameObjectManager.AddObject(hut);
                Crate c = new Crate(crateTex, new Vector2(200 - crateTex.Width - haystackTex.Width, 200 + i * 600 + hut1Tex.Height - crateTex.Height));
                gameObjectManager.AddObject(c);

                hut = new Hut(hut2Tex, new Vector2(-250, 400 + i * 400));
                gameObjectManager.AddObject(hut);

                c = new Crate(crateTex, new Vector2(-80, 450 * i + 200));
                gameObjectManager.AddObject(c);
                c = new Crate(crateTex, new Vector2(-80 - crateTex.Width, 450 * i + 200));
                gameObjectManager.AddObject(c);
                c = new Crate(crateTex, new Vector2(-80 - crateTex.Width * 2, 450 * i + 200));
                gameObjectManager.AddObject(c);
                c = new Crate(crateTex, new Vector2(-80 - crateTex.Width / 2, 450 * i + 200 - crateTex.Height));
                gameObjectManager.AddObject(c);
            }
            Haystack aHaystack=new Haystack(haystackTex,new Vector2(-273,446));
            gameObjectManager.AddObject(aHaystack);
            Hut aHut = new Hut(hut2Tex, new Vector2(17,962));
            gameObjectManager.AddObject(aHut);
            aHut = new Hut(hut2Tex, new Vector2(17, 1362));
            gameObjectManager.AddObject(aHut);
            Crate aCrate = new Crate(crateTex, new Vector2(17,1362+hut2Tex.Height-crateTex.Height));
            gameObjectManager.AddObject(aCrate);

            aHut = new Hut(hut1Tex, new Vector2(97, 953));
            gameObjectManager.AddObject(aHut);
            aHut=new Hut(hut2Tex,new Vector2(63,997));
            gameObjectManager.AddObject(aHut);
            aHut = new Hut(hut2Tex, new Vector2(132, 997));
            gameObjectManager.AddObject(aHut);

            aHut = new Hut(hut1Tex, new Vector2(-297, 200));
            gameObjectManager.AddObject(aHut);

            aHaystack = new Haystack(haystackTex, new Vector2(-142, 1750));
            gameObjectManager.AddObject(aHaystack);
            aHut = new Hut(hut1Tex, new Vector2(-132, 1750));
            gameObjectManager.AddObject(aHut);

            aHut = new Hut(hut1Tex, new Vector2(166, 1240));
            gameObjectManager.AddObject(aHut);
            //166 1713
            aCrate = new Crate(crateTex, new Vector2(166, 1713));
            gameObjectManager.AddObject(aCrate);
            aCrate = new Crate(crateTex, new Vector2(166 - crateTex.Width, 1713));
            gameObjectManager.AddObject(aCrate);
            aCrate = new Crate(crateTex, new Vector2(166 - crateTex.Width * 2, 1713));
            gameObjectManager.AddObject(aCrate);
            aCrate = new Crate(crateTex, new Vector2(166 - crateTex.Width / 2, 1713- crateTex.Height));
            gameObjectManager.AddObject(aCrate);
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
            markerOffset = new Vector2(-P1MarkerTex.Width/2, -P1MarkerTex.Height/2);

            attackTex = c.Load<Texture2D>("attack");
            powTex = c.Load<Texture2D>("pow");
            debugTex = c.Load<Texture2D>("white");

            attackManager = new AttackManager(powTex);
            enemyManager = new EnemyManager(templarTex, castlePosition, heroes, this);

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
            Goblin g = new Goblin(goblinTex, playArea);
            g.Position = new Vector2(castlePosition.X, castlePosition.Y + castleTex.Height);
            g.Destination = new Vector2(carriagRec.Center.X, carriagRec.Center.Y);
            g.IsActive = true;
            heroes.Add(g);

            princessCarrier = g; //makes it easier for reference

            Hero h = new Brute(bruteTex, playArea);
            h.Position = new Vector2(castlePosition.X - 2 * h.sprite.Width, castlePosition.Y + castleTex.Height);
            h.IsActive = true;
            heroes.Add(h);

            h = new Knight(darknightTex, playArea);
            h.Position = new Vector2(castlePosition.X + h.sprite.Width, castlePosition.Y + castleTex.Height);
            h.IsActive = false;
            heroes.Add(h);

            h = new Widow(widowTex, playArea);
            h.Position = new Vector2(castlePosition.X - h.sprite.Width, castlePosition.Y + castleTex.Height);
            heroes.Add(h);
        }

        #endregion LoadLevelContent

        #region UpdateLevelContent

        public void Draw(SpriteBatch sb)
        {
            //Draw ground
           sb.Draw(playAreaTex, playArea, Color.White); 
            //Draw scenery
            gameObjectManager.Draw(sb);
            attackManager.Draw(sb);
            foreach (Vector2 pos in treePositions)
            {
                sb.Draw(treeTex, pos, Color.White);
            }
            sb.Draw(castleTex, castlePosition, Color.White);
            sb.Draw(carriageTex, carriagRec, Color.White);

            //draw heroes
            foreach (Hero h in heroes)
            {
                if (h.Freezed)
                {
                    sb.Draw(h.sprite, h.Position, Color.Blue);
                }
                else
                {
                    sb.Draw(h.sprite, h.Position, Color.White);

                    if(GameState.DEBUG)
                        sb.Draw(debugTex, h.CollisionArea, Color.Red);
                }
            }
            sb.Draw(P1MarkerTex, heroes[P1HeroIndex].Position+markerOffset, Color.White);
            if(P2HeroIndex>0)
                sb.Draw(P2MarkerTex, heroes[P2HeroIndex].Position + markerOffset, Color.White);
            enemyManager.Draw(sb);
        }

        public void Update(GameTime time)
        {
            if (GameState.getInstance(this).Status == GameState.State.RUN)
            {
                
                //Collision detection
                collisionManager.ObstacleCollisionResolution(gameObjectManager.GameObjects, heroes);
                gameObjectManager.Update(time);
                attackManager.Update(time);
                //let the enemies carry our princess to the castle
                collisionManager.PrincessCollisionCheck(enemyManager.Enemies, heroes[0]);
                //Enemies and heroes should not overlap
                //collisionManager.HeroEnemyCollisionResolution(enemyManager.Enemies, heroes);
                //
                attackManager.Attacks.Remove(collisionManager.AttackEnemyCollision(enemyManager.Enemies, attackManager.Attacks));
                attackManager.Attacks.Remove(collisionManager.AttackGameObjectCollision(gameObjectManager.GameObjects, attackManager.Attacks));

                #region camera update

                
                //focus the camera in the middle and adjust the zoom until it fits the heroes
                // the camera doesn't move in the x direction, just in the middle of the princess and both heroes

                //Adjust position
                camPos.X = 0;

                if (P2HeroIndex > 0)
                {
                    camPos.Y = (heroes[P1HeroIndex].Position.Y + heroes[P2HeroIndex].Position.Y) / 2;
                }
                else
                {
                    camPos.Y = (heroes[P1HeroIndex].Position.Y);
                }
                princessCam.Pos = camPos;

                isP1Offscreen = !princessCam.Area.Contains((int)heroes[P1HeroIndex].X, (int)heroes[P1HeroIndex].Y + 50);
                if (P2HeroIndex > 0)
                    isP2Offscreen = !princessCam.Area.Contains((int)heroes[P2HeroIndex].X, (int)heroes[P2HeroIndex].Y + 50);

                //Adjust Zoom
                if (isP1Offscreen || isP2Offscreen)

                {
                    princessCam.Zoom = princessCam.Zoom - 0.001f;
                    princessCam.Height = (int)(game.GraphicsDevice.Viewport.Height / princessCam.Zoom);                    
                }
                else
                {
                    //adjust the zoom back if the players are in the bounds
                    if (princessCam.Height > game.GraphicsDevice.Viewport.Height * princessCam.Zoom)
                    {
                        princessCam.Zoom = princessCam.Zoom + 0.001f;
                        princessCam.Height = (int)(game.GraphicsDevice.Viewport.Height / princessCam.Zoom);                        
                    }
                }
                               
                #endregion

                for (int i = 0; i < heroes.Count; i++)
                {

                    Hero h = heroes[i];
                    if (!h.IsActive && i != 0)//makes the inactive heroes follow player one
                    {
                        h.Destination = heroes[1].Position + new Vector2(40, -80);
                    }
                    h.Update(time);
                    //hero stops following the princess when he becomes active
                    if (h.IsActive) h.Direction = Vector2.Zero;
                }
                
                //Update enemies
                enemyManager.Update(time);
            }
        }

        public void MoveHeroLeft(int player)
        {
            if (player == 0)
            {
                heroes[P1HeroIndex].moveLeft();
            }
            else if (P2HeroIndex > 0)
                heroes[P2HeroIndex].moveLeft();
        }
        public void MoveHeroRight(int player)
        {
            if (player == 0)
            {
                heroes[P1HeroIndex].moveRight();
            }
            else if (P2HeroIndex > 0)
                heroes[P2HeroIndex].moveRight();
        }
        public void MoveHeroUp(int player)
        {
            if (player == 0)
            {
                heroes[P1HeroIndex].moveUp();
            }
            else if (P2HeroIndex > 0)
                heroes[P2HeroIndex].moveUp();
        }
        public void MoveHeroDown(int player)
        {
            if (player == 0)
            {
                heroes[P1HeroIndex].moveDown();
            }
            else if (P2HeroIndex > 0)
                heroes[P2HeroIndex].moveDown();
        }
        public void HeroAttack(int player)
        {
            if (player == 0)
            {
                if (!heroes[P1HeroIndex].Freezed)
                    attackManager.AddAttack(heroes[P1HeroIndex]);
                
            }
            else if (P2HeroIndex > 0)
            {
                if (!heroes[P2HeroIndex].Freezed)
                    attackManager.AddAttack(heroes[P2HeroIndex]);
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
                if (P1HeroIndex == P2HeroIndex)
                {
                    P1HeroIndex++;
                    P1HeroIndex %= 4;
                    if (P1HeroIndex == 0)
                        P1HeroIndex++;
                }
                heroes[P1HeroIndex].IsActive = true;
            }
            else if (P2HeroIndex > 0)
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
                    if(P2HeroIndex==0)
                        P2HeroIndex++;
                }
                heroes[P2HeroIndex].IsActive = true;
            }
        }

        #endregion UpdateLevelContent

        public void PosInfo()
        {
            //I used this to get an idea where my hero is, in order to place some Gameobjects nicely
            v = heroes[P1HeroIndex].Position;
        }


        public void toggleOnePlayerTwoPlayer()
        {
            if (P2HeroIndex > 0)
            {
                heroes[P2HeroIndex].IsActive = false;
                heroes[P2HeroIndex].Direction = Vector2.Zero;
                P2HeroIndex = -1;
            }
            else
            {
                P2HeroIndex = P1HeroIndex+1;
                P2HeroIndex %= 4;
                if (P2HeroIndex == 0) P2HeroIndex++;
                heroes[P2HeroIndex].IsActive = true;
            }
        }
    }
}
