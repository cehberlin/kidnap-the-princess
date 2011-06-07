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
        private SpellType[] useableSpells = { SpellType.ColdSpell, SpellType.CreateMatterSpell, SpellType.NoGravitySpell, SpellType.WarmingSpell };

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
            //TODO: Add the never moving zero layer

            //The platforms.
            Layer layer = levelLoader.getLayerByName("Middle");
            foreach (Item item in layer.Items)
            {
                TextureItem t = (TextureItem)item;
                BlockElement b = new BlockElement(t.asset_name, CollisionType.Platform, level, t.Position - t.Origin);
                //b.Width = b.Texture.Width;
                //b.Height = b.Texture.Height;
                b.Width = (int)t.Origin.X * 2;
                b.Height = (int)t.Origin.Y * 2;
                elements.Add(b);
            }

            //The ingredient layer.
            Layer ingredientLayer = levelLoader.getLayerByName("Ingredients");
            foreach (Item item in ingredientLayer.Items)
            {
                String ingredientName = (String)item.CustomProperties["Ingredient"].value;
                Ingredient i = new Ingredient("Ingredients/"+ ingredientName, CollisionType.Passable, level, item.Position);
                elements.Add(i);
            }
            return elements;
        }

        public List<BasicGameElement> getBackgroundObjects()
        {
            List<BasicGameElement> elements = new List<BasicGameElement>();

            //The background.
            Layer backgroundLayer = levelLoader.getLayerByName("Background");
            foreach (Item item in backgroundLayer.Items)
            {
                TextureItem t = (TextureItem)item;
                BlockElement b = new BlockElement(t.asset_name, CollisionType.Passable, level, t.Position - t.Origin);
                //b.Width = b.Texture.Width;
                //b.Height = b.Texture.Height;
                b.Width = (int)t.Origin.X * 2;
                b.Height = (int)t.Origin.Y * 2;
                elements.Add(b);
            }

            return elements;
        }

        public List<BasicGameElement> getForegroundObjects()
        {
            List<BasicGameElement> elements = new List<BasicGameElement>();
            //The front layer.
            Layer frontLayer = levelLoader.getLayerByName("Front");
            foreach (Item item in frontLayer.Items)
            {
                TextureItem t = (TextureItem)item;
                BlockElement b = new BlockElement(t.asset_name, CollisionType.Passable, level, t.Position - t.Origin);
                //b.Width = b.Texture.Width;
                //b.Height = b.Texture.Height;
                b.Width = (int)t.Origin.X * 2;
                b.Height = (int)t.Origin.Y * 2;
                elements.Add(b);
            }

            return elements;
        }

        public Microsoft.Xna.Framework.Vector2 getPlayerStartPosition()
        {
            return levelLoader.getItemByName("start").Position - new Vector2(200, 0);

        }

        public BasicGameElement getLevelExit()
        {
            return new BlockElement("Tiles/exit", CollisionType.Passable, level, levelLoader.getItemByName("exit").Position);
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
            Vector2 left = levelLoader.getItemByName("leftCorner").Position;
            Vector2 right = levelLoader.getItemByName("rightCorner").Position;
            return new HelperClasses.Bounds(left, right.X - left.X, right.Y - left.Y);
        }

        #endregion

        Gleed2dLevelLoader levelLoader;

        public XMLLevelLoader(int levelNumber)
        {
            levelLoader = Gleed2dLevelLoader.FromFile("Content/LevelData/level" + levelNumber.ToString() + ".xml");
        }

    }
}
