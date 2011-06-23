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
using MagicWorld.DynamicLevelContent.SwitchRiddles;
using MagicWorld.BlendInClasses;

namespace MagicWorld.StaticLevelContent
{

    class XMLLevelLoader : ILevelLoader
    {

        const String LAYER_COLLECTABLEITEMS = "Ingredient";
        const String LAYER_SPECIAL = "Special";

        const String PROPERTY_SWITCH = "switch";
        const String PROPERTY_SWITCHABLE = "switchable";
        const String PROPERTY_DOOR = "door";


        #region "Level Properties"
        // Item name of Item with all custom properties for the level
        const String ITEM_NAME_LEVEL_PROPERTIES = "LevelProperties";

        const String PROPERTY_NAME_MIN_ITEM = "min item";

        const String PROPERTY_NAME_MAX_TIME = "max time";

        const String PROPERTY_NAME_INSTRUCTION = "Instruction";

        //custom property names for spells
        const String USABLE_SPELL_FIRE = "warm";
        const String USABLE_SPELL_FREEZE = "cold";
        const String USABLE_SPELL_PUSH = "push";
        const String USABLE_SPELL_PULL = "pull";
        const String USABLE_SPELL_ELECTRIC = "electric";
        const String USABLE_SPELL_NOGRAVITY = "nogravity";
        const String USABLE_SPELL_MATTER = "matter";
        #endregion

        Level level;
        private SpellType[] useableSpells_default = { SpellType.ColdSpell, SpellType.CreateMatterSpell, SpellType.NoGravitySpell, SpellType.WarmingSpell, SpellType.ElectricSpell, SpellType.PullSpell, SpellType.PushSpell };

        #region ILevelLoader member

        public void init(Level level)
        {
            this.level = level;
        }

        public Spells.SpellType[] UsableSpells
        {
            get
            {
                LinkedList<SpellType> usableSpellList = new LinkedList<SpellType>();

                Item levelPropertyItem = levelLoader.getItemByName(ITEM_NAME_LEVEL_PROPERTIES);

                if (levelPropertyItem == null) //return default if no usableSpellItem
                {
                    return useableSpells_default;
                }


                if (levelPropertyItem.CustomProperties.ContainsKey(USABLE_SPELL_FIRE))
                {
                    usableSpellList.AddLast(SpellType.WarmingSpell);
                }
                if (levelPropertyItem.CustomProperties.ContainsKey(USABLE_SPELL_ELECTRIC))
                {
                    usableSpellList.AddLast(SpellType.ElectricSpell);
                }
                if (levelPropertyItem.CustomProperties.ContainsKey(USABLE_SPELL_FREEZE))
                {
                    usableSpellList.AddLast(SpellType.ColdSpell);
                }
                if (levelPropertyItem.CustomProperties.ContainsKey(USABLE_SPELL_MATTER))
                {
                    usableSpellList.AddLast(SpellType.CreateMatterSpell);
                }
                if (levelPropertyItem.CustomProperties.ContainsKey(USABLE_SPELL_NOGRAVITY))
                {
                    usableSpellList.AddLast(SpellType.NoGravitySpell);
                }
                if (levelPropertyItem.CustomProperties.ContainsKey(USABLE_SPELL_PULL))
                {
                    usableSpellList.AddLast(SpellType.PullSpell);
                }
                if (levelPropertyItem.CustomProperties.ContainsKey(USABLE_SPELL_PUSH))
                {
                    usableSpellList.AddLast(SpellType.PushSpell);
                }


                return usableSpellList.ToArray();
            }
            set
            {
                throw new NotImplementedException();
            }
        }


        public int getMinimumItemsToEndLevel()
        {

            Item levelPropertyItem = levelLoader.getItemByName(ITEM_NAME_LEVEL_PROPERTIES);

            if (levelPropertyItem.CustomProperties[PROPERTY_NAME_MIN_ITEM] != null)
            {
                String value = (String)levelPropertyItem.CustomProperties[PROPERTY_NAME_MIN_ITEM].value;
                int minItems = int.Parse(value);
                return minItems;
            }
            else
            {
                throw new Exception("Level has no level property Item!");
            }
        }

        public List<BasicGameElement> getInteractingObjects()
        {
            List<BasicGameElement> elements = new List<BasicGameElement>();


            // get switches

            LinkedList<AbstractSwitch> switchList = new LinkedList<AbstractSwitch>();
            Layer specialLayer = levelLoader.getLayerByName(LAYER_SPECIAL);
            foreach (Item item in specialLayer.Items)
            {
                if (item.CustomProperties.ContainsKey(PROPERTY_SWITCH))
                {
                    String id = (String)item.CustomProperties[PROPERTY_SWITCH].value;
                    TextureItem ti = (TextureItem)item;

                    PushDownSwitch pds = new PushDownSwitch(ti.asset_name, level, getCorrectedStartPosition(ti), id);
                    correctWidhAndHeight(pds, ti);

                    switchList.AddLast(pds);
                    elements.Add(pds);
                }
            }



            //The platforms.
            Layer layer = levelLoader.getLayerByName("Middle");
            elements.AddRange(Load(layer, author, CollisionType.Platform));

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
                Enemy e = null;
                String monsterName = (String)item.CustomProperties["EnemyType"].value;
                if (monsterName.Equals("ShadowCreature"))
                {
                    e = new ShadowCreature(level, item.Position, "Sprites/ShadowCreatureSpriteSheet");
                }
                else if (monsterName.Equals("Bat"))
                {
                    PathItem pathItem = (PathItem)item.CustomProperties["Path"].value;
                    e = new Bat(level, item.Position, "Sprites/ShadowCreatureSpriteSheet", pathItem);
                }
                if (author == 2 && e != null)
                {
                    TextureItem t = (TextureItem)item;
                    e.Position -= t.Origin;

                }

                elements.Add(e);
            }


            //The moveable platform layer.
            Layer moveablePlatformLayer = levelLoader.getLayerByName("Moveable Platform");
            if (moveablePlatformLayer != null)
            {
                foreach (Item item in moveablePlatformLayer.Items)
                {
                    //String ingredientName = (String)item.CustomProperties["Ingredient"].value;
                    TextureItem t = (TextureItem)item;
                    PathItem pathItem = (PathItem)item.CustomProperties["Path"].value;
                    MoveablePlatform m = new MoveablePlatform(t.asset_name, CollisionType.Impassable, level, item.Position, pathItem);
                    m.Position -= t.Origin;
                    m.Width = (int)t.Origin.X * 2;
                    m.Height = (int)t.Origin.Y * 2;
                    elements.Add(m);
                }
            }

            // get switchable items

            foreach (Item item in specialLayer.Items)
            {
                if (item.CustomProperties.ContainsKey(PROPERTY_DOOR))
                {
                    TextureItem ti = (TextureItem)item;

                    Door door = new Door(ti.asset_name, level, getCorrectedStartPosition(ti));
                    correctWidhAndHeight(door, ti);
                    elements.Add(door);

                    if (item.CustomProperties.ContainsKey(PROPERTY_SWITCHABLE))
                    {
                        String id = (String)item.CustomProperties[PROPERTY_SWITCHABLE].value;
                        connectSwitchable(switchList, id, door);
                    }
                }
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


                if (t.asset_name.Contains("platform"))
                {
                    b = new Platform(t.asset_name, collisionType, level, t.Position);
                }
                else
                {
                    b = new BlockElement(t.asset_name, collisionType, level, t.Position);
                }
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
            BlockElement b = new BlockElement("LevelContent/Cave/exit", CollisionType.Passable, level, levelLoader.getItemByName("exit").Position);
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
            Item levelPropertyItem = levelLoader.getItemByName(ITEM_NAME_LEVEL_PROPERTIES);

            if (levelPropertyItem.CustomProperties[PROPERTY_NAME_MAX_TIME] != null)
            {
                String value = (String)levelPropertyItem.CustomProperties[PROPERTY_NAME_MAX_TIME].value;
                int time = int.Parse(value);
                return time;
            }
            else
            {
                throw new Exception("Level has no level property Item!");
            }
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

        public List<TutorialInstruction> GetTutorialInstructions()
        {
            List<TutorialInstruction> instructs = new List<TutorialInstruction>();
            Layer l = levelLoader.getLayerByName("Special");
            foreach (Item item in l.Items)
            {
                //TODO: Make a real check if we found a tutorial instruction.
                if (item.CustomProperties.Count == 1)
                {
                    try
                    {
                        if (item.CustomProperties[PROPERTY_NAME_INSTRUCTION] != null)
                        {
                            instructs.Add(new TutorialInstruction(item.CustomProperties[PROPERTY_NAME_INSTRUCTION].value.ToString(), new Vector2(200, 50)));
                        }
                    }
                    catch (KeyNotFoundException k) { }
                }
            }
            return instructs;
        }

        #endregion

        Gleed2dLevelLoader levelLoader;

        public XMLLevelLoader(int levelNumber)
        {
            levelLoader = Gleed2dLevelLoader.FromFile("Content/LevelData/level" + levelNumber.ToString() + ".xml");
            author = DetectAuthor();
        }


        public int getMaxItmesToCollect()
        {
            Layer ingredientLayer = levelLoader.getLayerByName(LAYER_COLLECTABLEITEMS);
            return ingredientLayer.Items.Count;
        }

        private void correctWidhAndHeight(BlockElement element, TextureItem ti)
        {
            element.Width = (int)ti.Origin.X * 2;
            element.Height = (int)ti.Origin.Y * 2;
        }

        private Vector2 getCorrectedStartPosition(TextureItem ti)
        {
            Vector2 pos = ti.Position - ti.Origin;
            return pos;
        }

        private void connectSwitchable(LinkedList<AbstractSwitch> switchList, String id, IActivation switchableObject)
        {
            foreach (PushDownSwitch sw in switchList)
            {
                if (sw.ID.Equals(id))
                {
                    sw.SwitchableObjects.AddLast(switchableObject);
                }
            }

        }

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
            return 2;
            //Item i = levelLoader.getItemByName("Author");
            //switch (i.CustomProperties["Author"].description)
            //{
            //    case "John":
            //        return 1;
            //    case "Marian":
            //        return 1;
            //    case "Pascal":
            //        return 2;
            //    case "Christopher":
            //        return 2;
            //    default:
            //        return 0;
            //}
        }
    }
}
