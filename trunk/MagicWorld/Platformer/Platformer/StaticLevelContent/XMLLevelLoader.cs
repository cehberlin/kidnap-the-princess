using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MagicWorld.Gleed2dLevelContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using MagicWorld.Spells;
using MagicWorld.DynamicLevelContent;
using MagicWorld.Ingredients;

namespace MagicWorld.StaticLevelContent
{
    class XMLLevelLoader : ILevelLoader
    {
        Level level;
        private SpellType[] useableSpells = { SpellType.ColdSpell, SpellType.CreateMatterSpell, SpellType.NoGravitySpell, SpellType.WarmingSpell, SpellType.ElectricSpell, SpellType.PullSpell, SpellType.PushSpell };

        #region ILevelLoader member

        public void init(Level level)
        {
            this.level = level;
        }

        public Spells.SpellType[] UsableSpells
        {
            get
            {
                return useableSpells;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public List<BasicGameElement> getInteractingObjects()
        {
            List<BasicGameElement> elements = new List<BasicGameElement>();

            //The platforms.
            Layer layer = levelLoader.getLayerByName("Middle");
            elements.AddRange(Load(layer, author, CollisionType.Platform));
            //foreach (Item item in layer.Items)
            //{
            //    TextureItem t = (TextureItem)item;
            //    BlockElement b = new BlockElement(t.asset_name, CollisionType.Platform, level, t.Position - t.Origin);
            //    b.Width = (int)t.Origin.X * 2;
            //    b.Height = (int)t.Origin.Y * 2;
            //    elements.Add(b);
            //}

            Layer blockades = levelLoader.getLayerByName("Blockade");
            elements.AddRange(Load(blockades, author, CollisionType.Impassable));

            //The ingredient layer.
            Layer ingredientLayer = levelLoader.getLayerByName("Ingredient");
            //elements.AddRange(Load(ingredientLayer, author, CollisionType.Passable));
            foreach (Item item in ingredientLayer.Items)
            {
                //String ingredientName = (String)item.CustomProperties["Ingredient"].value;
                Ingredient i = new Ingredient("LevelContent/Cave/bone", CollisionType.Passable, level, item.Position);
                elements.Add(i);
            }
            //The enemies layer.
            Layer enemiesLayer = levelLoader.getLayerByName("Enemy");
            foreach (Item item in enemiesLayer.Items)
            {
                //String monsterName = (String)item.CustomProperties["Enemy"].value;
                ShadowCreature e = new ShadowCreature(level, item.Position, "Sprites/ShadowCreatureSpriteSheet");
                if (author == 2)
                {
                    TextureItem t = (TextureItem)item;
                    e.Position -= t.Origin;
                    
                }
                elements.Add(e);
            }

            return elements;
        }

        public List<BasicGameElement> getBackgroundObjects()
        {
            List<BasicGameElement> elements = new List<BasicGameElement>();
            Layer zeroLayer = levelLoader.getLayerByName("Zero");
            elements.AddRange(Load(zeroLayer, author, CollisionType.Passable));
            //The background.
            Layer backgroundLayer = levelLoader.getLayerByName("Background");
            elements.AddRange(Load(backgroundLayer, author, CollisionType.Passable));
            return elements;
        }

        public List<BasicGameElement> getForegroundObjects()
        {
            Layer frontLayer = levelLoader.getLayerByName("Front");
            return Load(frontLayer, author, CollisionType.Passable);
        }

        private List<BasicGameElement> Load(Layer layer, int author, CollisionType collisionType)
        {
            List<BasicGameElement> elements = new List<BasicGameElement>();
            TextureItem t;
            BlockElement b;

            foreach (Item item in layer.Items)
            {
                t = (TextureItem)item;
                b = new BlockElement(t.asset_name, collisionType, level, t.Position);
                if (author == 2)
                {
                    b.Position -= t.Origin;
                    b.Width = (int)t.Origin.X * 2;
                    b.Height = (int)t.Origin.Y * 2;
                }
                elements.Add(b);
            }

            return elements;
        }

        //TODO: Add a texture for the level starting point.
        public Microsoft.Xna.Framework.Vector2 getPlayerStartPosition()
        {
            return levelLoader.getItemByName("start").Position - new Vector2(200, 0);
        }

        public BasicGameElement getLevelExit()
        {
            BlockElement  b=new BlockElement("LevelContent/Cave/exit", CollisionType.Passable, level, levelLoader.getItemByName("exit").Position);
            if (author == 1)
            {
                return b;
            }
            else
            {
                TextureItem t = (TextureItem)levelLoader.getItemByName("exit");
                b.Position -= t.Origin;
                b.Width = (int)t.Origin.X * 2;
                b.Height = (int)t.Origin.Y * 2;
                return b;
            }
        }

        public double getMaxLevelTime()
        {
            return 99;
        }

        public Microsoft.Xna.Framework.Media.Song getBackgroundMusic()
        {
            return level.Content.Load<Song>("Sounds/Backgroundmusic");
        }

        public HelperClasses.Bounds getLevelBounds()
        {
            //TODO: Check if this needs to be corrected.
            Vector2 left = levelLoader.getItemByName("topLeft").Position;
            Vector2 right = levelLoader.getItemByName("bottomRight").Position;
            return new HelperClasses.Bounds(left, right.X - left.X, right.Y - left.Y);
        }

        #endregion

        Gleed2dLevelLoader levelLoader;

        public XMLLevelLoader(int levelNumber)
        {
            levelLoader = Gleed2dLevelLoader.FromFile("Content/LevelData/level" + levelNumber.ToString() + ".xml");
            author = DetectAuthor();
        }

        //TODO: If Amauri wants to do some levels we need to ind out how Gleed2d functions on his PC.
        /// <summary>
        /// Let's us know who has created the level.
        /// 1=John or Marian
        /// 2=Christopher or Pascal
        /// </summary>
        int author;

        /// <summary>
        /// Function for detecting who was created the level.
        /// </summary>
        /// <returns>1 for John and Marian. 2 For Christopher and Pascal. Zero in case of an error. </returns>
        private int DetectAuthor()
        {
            Item i = levelLoader.getItemByName("Author");
            switch (i.CustomProperties["Author"].description)
            {
                case "John":
                    return 1;
                case "Marian":
                    return 1;
                case "Pascal":
                    return 2;
                case "Christopher":
                    return 2;
                default:
                    return 0;
            }
        }
    }
}
