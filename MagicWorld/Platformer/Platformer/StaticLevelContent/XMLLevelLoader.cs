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
using MagicWorld.DynamicLevelContent.Enemies;
using System.Diagnostics;

namespace MagicWorld.StaticLevelContent
{

    class XMLLevelLoader : ILevelLoader
    {

        const String LAYER_COLLECTABLEITEMS = "Ingredient";
        const String LAYER_SPECIAL = "Special";

        #region switch and switchable

        const String PROPERTY_SWITCH = "switch_weight";
        const String PROPERTY_SWITCH_ELECTRICITY = "switch_electricitySwitch";
        const String PROPERTY_SWITCH_TORCH_ON = "switch_torch_on";
        const String PROPERTY_SWITCH_TORCH_OFF = "switch_torch_off";
        const String PROPERTY_SWITCH_TIME = "switchTime";
        const String PROPERTY_SWITCH_DESTROY = "switch_destroy";

        const String PROPERTY_SWITCHABLE = "switchable";
        const String PROPERTY_DOOR_UPDOWN = "updown_door";
        const String PROPERTY_DOOR_OPENCLOSE = "openclose_door";
        const String PROPERTY_DOOR_OPEN = "open";
        const String PROPERTY_GRAVITY_ELEMENT = "gravity_element";
        const String PROPERTY_ENABLE_GRAVITY = "enable_gravity";
        const String PROPERTY_ENABLE_COLLISION = "enable_collision";

        #endregion

        #region collision type
        const String PROPERTY_COLLISION_TYPE = "collision_type";
        const String PROPERTY_COLLISION_TYPE_PASSABLE = "passable";
        const String PROPERTY_COLLISION_TYPE_IMPASSABLE = "impassable";
        const String PROPERTY_COLLISION_TYPE_PLATFORM = "platform";
        #endregion

        const String PROPERTY_TRUE = "True";
        const String PROPERTY_FALSE = "False";

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

        /// <summary>
        /// minimum ingredients
        /// </summary>
        /// <returns></returns>
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
            LinkedList<AbstractSwitch> switchList = loadSwitches(elements);


            //The platforms.
            Layer middleLayer = levelLoader.getLayerByName("Middle");
            if (middleLayer != null)
            {
                elements.AddRange(Load(middleLayer, author, CollisionType.Platform));
            }

            Layer blockades = levelLoader.getLayerByName("Blockade");
            if (blockades != null)
            {
                elements.AddRange(Load(blockades, author, CollisionType.Impassable));
            }

            //The ingredient layer.
            Layer ingredientLayer = levelLoader.getLayerByName("Ingredient");
            if (ingredientLayer != null)
            {
                //elements.AddRange(Load(ingredientLayer, author, CollisionType.Passable));
                foreach (Item item in ingredientLayer.Items)
                {
                    //String ingredientName = (String)item.CustomProperties["Ingredient"].value;
                    Ingredient i = new Ingredient("LevelContent/Cave/bone", CollisionType.Passable, level, item.Position);
                    elements.Add(i);
                }
            }

            //The enemies layer.
            Layer enemiesLayer = levelLoader.getLayerByName("Enemy");
            if (enemiesLayer != null)
            {
                foreach (Item item in enemiesLayer.Items)
                {
                    Enemy e = null;
                    if (item.CustomProperties.ContainsKey("EnemyType"))
                    {
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
                        else if (monsterName.Equals("Bullet"))
                        {
                            if (item.CustomProperties.ContainsKey("Aim"))
                            {
                                bool enable_gravity = true;//default
                                enable_gravity = checkForEnabledGravity(item, enable_gravity);
                                int delaytime = 2000;//Default
                                delaytime = getDelayTime(item, delaytime);

                                Item aim = (Item)item.CustomProperties["Aim"].value;

                                TextureItem ti = (TextureItem)item;

                                Vector2 startPos = ti.Position;

                                Vector2 velocity = aim.Position - startPos;

                                BulletLauncher launcher = new BulletLauncher(ti.asset_name, "Sprites/WarmSpell/Run", level,
                                    startPos, velocity, delaytime, enable_gravity);
                                elements.Add(launcher);
                            }
                            else
                            {
                                Debug.WriteLine("Bullet Enemy needs Aim for calculating shooting direction and velocity");
                            }
                        }
                    }

                    if (e != null)
                    {
                        elements.Add(e);
                    }
                }
            }


            //The layer of push and pullable items.
            Layer pushPullLayer = levelLoader.getLayerByName("PushPull");
            if (pushPullLayer != null)
            {
                foreach (Item item in pushPullLayer.Items)
                {
                    bool enablegravity = false;
                    enablegravity = checkForEnabledGravity(item, enablegravity);
                    TextureItem ti = (TextureItem)item;

                    CollisionType collisionType = CollisionType.Impassable;
                    collisionType = getCollisionType(item, collisionType);
                    PushPullElement pushPullElement = new PushPullElement(ti.asset_name, collisionType, level, item.Position, enablegravity);

                    elements.Add(pushPullElement);
                }
            }


            //The moveable platform layer.
            Layer moveablePlatformLayer = levelLoader.getLayerByName("Moveable Platform");
            if (moveablePlatformLayer != null)
            {
                if (moveablePlatformLayer != null)
                {
                    foreach (Item item in moveablePlatformLayer.Items)
                    {
                        //String ingredientName = (String)item.CustomProperties["Ingredient"].value;
                        if (!(Boolean)item.CustomProperties["Spell"].value)
                        {
                            TextureItem t = (TextureItem)item;
                            PathItem pathItem = (PathItem)item.CustomProperties["Path"].value;
                            MoveablePlatform m = new MoveablePlatform(t.asset_name, CollisionType.Impassable, level, item.Position, pathItem);
                            m.Position -= t.Origin;
                            m.Width = (int)t.Origin.X * 2;
                            m.Height = (int)t.Origin.Y * 2;
                            elements.Add(m);
                        }
                        else
                        {
                            TextureItem t = (TextureItem)item;
                            PathItem pathItem = (PathItem)item.CustomProperties["Path"].value;
                            SpellMoveablePlatform m = new SpellMoveablePlatform(t.asset_name, CollisionType.Impassable, level, item.Position, pathItem);
                            m.Position -= t.Origin;
                            m.Width = (int)t.Origin.X * 2;
                            m.Height = (int)t.Origin.Y * 2;
                            elements.Add(m);
                        }
                    }
                }
            }


            // get switchable items
            Layer specialLayer = levelLoader.getLayerByName(LAYER_SPECIAL);
            if (specialLayer != null)
            {
                foreach (Item item in specialLayer.Items)
                {
                    if (item.CustomProperties.ContainsKey(PROPERTY_DOOR_UPDOWN)
                        || item.CustomProperties.ContainsKey(PROPERTY_DOOR_OPENCLOSE))
                    {
                        TextureItem ti = (TextureItem)item;

                        AbstractDoor door;

                        if (item.CustomProperties.ContainsKey(PROPERTY_DOOR_UPDOWN)) //up down door
                        {
                            bool opened = false;
                            opened = checkForOpenDoorStatus(item, opened);
                            door = new UpDownDoor(ti.asset_name, level, getCorrectedStartPosition(ti), opened);
                        }
                        else //open close door
                        {
                            //detect open/close status on naming
                            //we assume that the texture  path for open door contains the word "open"
                            //the corresponding closed texture needs to have the same path with
                            //open replaced by "closed"
                            string otherTexture = ti.asset_name;
                            if (ti.asset_name.Contains("open"))
                            {
                                otherTexture = otherTexture.Replace("open", "closed");
                                door = new OpenCloseDoor(ti.asset_name, otherTexture, level, getCorrectedStartPosition(ti), true);
                            }
                            else if (ti.asset_name.Contains("closed"))
                            {
                                otherTexture = otherTexture.Replace("closed", "open");
                                door = new OpenCloseDoor(otherTexture, ti.asset_name, level, getCorrectedStartPosition(ti), false);
                            }
                            else
                            {
                                throw new ArgumentException("Door does not fit nameing convension!");
                            }
                        }
                        correctWidhAndHeight(door, ti);
                        elements.Add(door);

                        if (item.CustomProperties.ContainsKey(PROPERTY_SWITCHABLE))
                        {
                            String id = (String)item.CustomProperties[PROPERTY_SWITCHABLE].value;
                            connectSwitchable(switchList, id, door);
                        }
                    }
                    else if (item.CustomProperties.ContainsKey(PROPERTY_GRAVITY_ELEMENT))
                    {
                        bool enablegravity = false;
                        enablegravity = checkForEnabledGravity(item, enablegravity);

                        bool enablecollision = false;
                        enablecollision = checkForEnabledCollision(item, enablecollision);

                        TextureItem ti = (TextureItem)item;

                        CollisionType collisionType = CollisionType.Impassable;
                        collisionType = getCollisionType(item, collisionType);

                        GravityElement gravityElement = new GravityElement(ti.asset_name, collisionType, level, getCorrectedStartPosition(ti), enablecollision, enablegravity);
                        correctWidhAndHeight(gravityElement, ti);
                        elements.Add(gravityElement);

                        if (item.CustomProperties.ContainsKey(PROPERTY_SWITCHABLE))
                        {
                            String id = (String)item.CustomProperties[PROPERTY_SWITCHABLE].value;
                            connectSwitchable(switchList, id, gravityElement);
                        }
                    }
                }
            }


            //The portal layer.
            Layer portalLayer = levelLoader.getLayerByName("Portal");
            if (portalLayer != null)
            {
                foreach (Item item in portalLayer.Items)
                {
                    TextureItem ti = (TextureItem)item;

                    Item destination = (Item)item.CustomProperties["Destination"].value;

                    if (destination != null)
                    {
                        String portalHandlingType = "";
                        if (item.CustomProperties.ContainsKey("Type"))
                        {
                            portalHandlingType = (String)item.CustomProperties["Type"].value;
                        }

                        PortalHandlingType type;

                        if (portalHandlingType.Equals("ShadowCreature"))
                        {
                            type = PortalHandlingType.SHADOW_CREATURE;
                        }
                        else if (portalHandlingType.Equals("PushPull"))
                        {
                            type = PortalHandlingType.PUSHPULL;
                        }
                        else
                        {
                            type = PortalHandlingType.PLAYER;
                        }

                        Portal portal = new Portal(ti.asset_name, level, getCorrectedStartPosition(ti), destination.Position, type);
                        correctWidhAndHeight(portal, ti);
                        elements.Add(portal);
                    }
                }
            }

            //The iceicle layer.
            Layer icecleLayer = levelLoader.getLayerByName("Icecicle");
            if (icecleLayer != null)
            {
                foreach (Item item in icecleLayer.Items)
                {
                    TextureItem ti = (TextureItem)item;

                    Icecicle ice = new Icecicle(ti.asset_name, level, getCorrectedStartPosition(ti));
                        
                    elements.Add(ice);                   
                }
            }

            return elements;
        }

        /// <summary>
        /// analyse the enable collision property
        /// </summary>
        /// <param name="item"></param>
        /// <param name="enablecollision"></param>
        /// <returns></returns>
        private static bool checkForEnabledCollision(Item item, bool enablecollision)
        {
            if (item.CustomProperties.ContainsKey(PROPERTY_ENABLE_COLLISION))
            {
                enablecollision = (bool)item.CustomProperties[PROPERTY_ENABLE_COLLISION].value;
            }
            return enablecollision;
        }

        /// <summary>
        /// analyse the enable gravity property
        /// </summary>
        /// <param name="item"></param>
        /// <param name="enablegravity"></param>
        /// <returns></returns>
        private static bool checkForEnabledGravity(Item item, bool enablegravity)
        {
            if (item.CustomProperties.ContainsKey(PROPERTY_ENABLE_GRAVITY))
            {
                enablegravity = (bool)item.CustomProperties[PROPERTY_ENABLE_GRAVITY].value;
            }
            return enablegravity;
        }

        /// <summary>
        /// get delay time custom property
        /// </summary>
        /// <param name="item"></param>
        /// <param name="enablegravity"></param>
        /// <returns></returns>
        private static int getDelayTime(Item item, int time)
        {
            if (item.CustomProperties.ContainsKey("DelayTime"))
            {
                time = (int)Convert.ToInt32((string)item.CustomProperties["DelayTime"].value);
            }
            return time;
        }

        /// <summary>
        /// analyse the enable gravity property
        /// </summary>
        /// <param name="item"></param>
        /// <param name="enablegravity"></param>
        /// <returns></returns>
        private static bool checkForOpenDoorStatus(Item item, bool opened)
        {
            if (item.CustomProperties.ContainsKey(PROPERTY_ENABLE_GRAVITY))
            {
                opened = (bool)item.CustomProperties[PROPERTY_DOOR_OPEN].value;
            }
            return opened;
        }


        /// <summary>
        /// analyses the custom property collision type
        /// </summary>
        /// <param name="item"></param>
        /// <param name="defaultCollision"></param>
        /// <returns></returns>
        private static CollisionType getCollisionType(Item item, CollisionType defaultCollision)
        {
            if (item.CustomProperties.ContainsKey(PROPERTY_COLLISION_TYPE))
            {
                String collisiontype = (String)item.CustomProperties[PROPERTY_COLLISION_TYPE].value;
                if (collisiontype.Equals(PROPERTY_COLLISION_TYPE_IMPASSABLE))
                {
                    defaultCollision = CollisionType.Impassable;
                }
                else if (collisiontype.Equals(PROPERTY_COLLISION_TYPE_PASSABLE))
                {
                    defaultCollision = CollisionType.Passable;
                }
                else if (collisiontype.Equals(PROPERTY_COLLISION_TYPE_PLATFORM))
                {
                    defaultCollision = CollisionType.Platform;
                }
            }
            return defaultCollision;
        }



        public List<BasicGameElement> getBackgroundObjects()
        {
            List<BasicGameElement> elements = new List<BasicGameElement>();
            Layer zeroLayer = levelLoader.getLayerByName("Zero");
            if (zeroLayer != null)
            {
                elements.AddRange(Load(zeroLayer, author, CollisionType.Passable));
            }
            //The background.
            Layer backgroundLayer = levelLoader.getLayerByName("Background");
            if (backgroundLayer != null)
            {
                elements.AddRange(Load(backgroundLayer, author, CollisionType.Passable));
            }
            return elements;
        }

        public List<BasicGameElement> getForegroundObjects()
        {
            List<BasicGameElement> foreGroundElements = new List<BasicGameElement>();
            Layer frontLayer = levelLoader.getLayerByName("Front");
            if (frontLayer != null)
            {
                foreGroundElements.AddRange(Load(frontLayer, author, CollisionType.Passable));
            }
            return foreGroundElements;
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
            if (l != null)
            {
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
            }
            return instructs;
        }

        #endregion

        MagicWorld.Gleed2dLevelContent.Level levelLoader;

        public XMLLevelLoader(int levelNumber)
        {
            levelLoader = MagicWorld.Gleed2dLevelContent.Level.FromFile("Content/LevelData/level" + levelNumber.ToString() + ".xml");
            author = DetectAuthor();
        }


        public int getMaxItmesToCollect()
        {
            Layer ingredientLayer = levelLoader.getLayerByName(LAYER_COLLECTABLEITEMS);
            if (ingredientLayer != null)
            {
                return ingredientLayer.Items.Count;
            }
            else
            {
                return 0;
            }
        }


        #region "helper methods"

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

        private LinkedList<AbstractSwitch> loadSwitches(List<BasicGameElement> elements)
        {
            LinkedList<AbstractSwitch> switchList = new LinkedList<AbstractSwitch>();
            Layer specialLayer = levelLoader.getLayerByName(LAYER_SPECIAL);
            if (specialLayer != null)
            {
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
                    else if (item.CustomProperties.ContainsKey(PROPERTY_SWITCH_ELECTRICITY))
                    {
                        String id = (String)item.CustomProperties[PROPERTY_SWITCH].value;
                        TextureItem ti = (TextureItem)item;
                        AbstractSwitch sw = null;

                        if (item.CustomProperties.ContainsKey(PROPERTY_SWITCH_TIME))
                        {
                            double time = Double.Parse((String)item.CustomProperties[PROPERTY_SWITCH_TIME].value);
                            sw = new TimedElectricitySwitch(ti.asset_name, level, getCorrectedStartPosition(ti), id, time);
                        }
                        else
                        {
                            sw = new OnOffElectricitySwitch(ti.asset_name, level, getCorrectedStartPosition(ti), id);
                        }

                        correctWidhAndHeight(sw, ti);

                        switchList.AddLast(sw);
                        elements.Add(sw);
                    }
                    else if (item.CustomProperties.ContainsKey(PROPERTY_SWITCH_TORCH_ON))
                    {
                        String id = (String)item.CustomProperties[PROPERTY_SWITCH_TORCH_ON].value;
                        TextureItem ti = (TextureItem)item;

                        TorchSwitch pds = new TorchSwitch(ti.asset_name, level, getCorrectedStartPosition(ti), id, true);
                        correctWidhAndHeight(pds, ti);

                        switchList.AddLast(pds);
                        elements.Add(pds);
                    }
                    else if (item.CustomProperties.ContainsKey(PROPERTY_SWITCH_TORCH_OFF))
                    {
                        String id = (String)item.CustomProperties[PROPERTY_SWITCH_TORCH_OFF].value;
                        TextureItem ti = (TextureItem)item;

                        TorchSwitch pds = new TorchSwitch(ti.asset_name, level, getCorrectedStartPosition(ti), id, false);
                        correctWidhAndHeight(pds, ti);

                        switchList.AddLast(pds);
                        elements.Add(pds);
                    }
                    else if (item.CustomProperties.ContainsKey(PROPERTY_SWITCH_DESTROY))
                    {
                        String id = (String)item.CustomProperties[PROPERTY_SWITCH_DESTROY].value;
                        TextureItem ti = (TextureItem)item;

                        OneTimeDestroySwitch pds = new OneTimeDestroySwitch(ti.asset_name, level, getCorrectedStartPosition(ti), id);
                        correctWidhAndHeight(pds, ti);

                        switchList.AddLast(pds);
                        elements.Add(pds);
                    }
                }
            }

            return switchList;
        }


        private void connectSwitchable(LinkedList<AbstractSwitch> switchList, String id, IActivation switchableObject)
        {
            foreach (AbstractSwitch sw in switchList)
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

        #endregion
}
