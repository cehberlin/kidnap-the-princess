using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MagicWorld.DynamicLevelContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using MagicWorld.Spells;
using MagicWorld.Ingredients;
using MagicWorld.DynamicLevelContent.SwitchRiddles;
using MagicWorld.BlendInClasses;

namespace MagicWorld.StaticLevelContent
{
    /// <summary>
    /// Static fixed implementation of a test level
    /// </summary>
    class StaticLevelLoader:ILevelLoader
    {
        Level level;

        public StaticLevelLoader()
        {
        }

        #region LevelLoader Member

        public void init(Level level)
        {
            this.level = level;
        }

        public List<BasicGameElement> getInteractingObjects()
        {
            List<BasicGameElement> elements = new List<BasicGameElement>();

            elements.Add(LoadBlock("BlockA0", CollisionType.Impassable,new Vector2(100f,200),100,32));
            elements.Add(LoadBlock("BlockA3", CollisionType.Impassable, new Vector2(220f, 300), 150, 45));
            elements.Add(LoadBlock("BlockA4", CollisionType.Impassable, new Vector2(400f, 250)));
            elements.Add(LoadBlock("BlockA1", CollisionType.Impassable, new Vector2(500f, 200),300,10));
           // elements.Add(new Enemy(level, new Vector2(350, 250), "MonsterA"));
            elements.Add(new IceBlockElement(level,new Vector2(100f, 130)));
            elements.Add(new IceBlockElement(level, new Vector2(100f, 100),200,32));
           // elements.Add(new Enemy(level, new Vector2(150, 50), "MonsterB"));
            Ingredient ingredient1 = LoadIngredient("Gem", CollisionType.Passable, new Vector2(150, 50));
            Ingredient ingredient2 = LoadIngredient("Gem", CollisionType.Passable, new Vector2(190, 50));
            Ingredient ingredient3 = LoadIngredient("Gem", CollisionType.Passable, new Vector2(350, 250));
            elements.Add(ingredient1);
            elements.Add(ingredient2);
            elements.Add(ingredient3);

            UpDownDoor testDoor = new UpDownDoor("LevelContent/Cave/door", level, new Vector2(600f, 50));
            elements.Add(testDoor);

            //examples for pushable pullable element
            elements.Add(new PushPullElement("LevelContent/Cave/Front/mushroom",CollisionType.Impassable,level,new Vector2(550,50)));

            //element without gravity influence
            elements.Add(new PushPullElement("LevelContent/Cave/Front/mushroom", CollisionType.Impassable, level, new Vector2(500, 50),false));

            //AbstractSwitch sw = new PushDownSwitch("LevelContent/Cave/switch", level, new Vector2(0f, 300), "");
            //AbstractSwitch sw = new TimedElectricitySwitch("LevelContent/Cave/switch", level, new Vector2(0f, 300), "",5000);
            //AbstractSwitch sw = new OnOffElectricitySwitch("LevelContent/Cave/switch", level, new Vector2(0f, 300), "");
            AbstractSwitch sw = new TorchSwitch("LevelContent/Cave/switch", level, new Vector2(0f, 300), "",true);

            //AbstractSwitch sw = new OneTimeDestroySwitch("LevelContent/Cave/switch", level, new Vector2(0f, 300), "");
            sw.SwitchableObjects.AddLast(testDoor);
            elements.Add(sw);

            elements.Add(LoadBlock("BlockA2", CollisionType.Impassable, new Vector2(0, 335), 150, 45));
            elements.Add(LoadBlock("BlockA1", CollisionType.Impassable, new Vector2(800, 230), 200, 45));

            //add a destroyable switch which activates gravity of a gravity element
            AbstractSwitch destroySW = new OneTimeDestroySwitch("LevelContent/Cave/switch", level, new Vector2(620f, 150), "");
            GravityElement graveElement = new GravityElement("LevelContent/Cave/Front/shovel", CollisionType.Impassable, level, new Vector2(620, 0),true);
            elements.Add(destroySW);
            elements.Add(graveElement);
            destroySW.SwitchableObjects.AddLast(graveElement);
            
            return elements;
        }

        public List<BasicGameElement> getBackgroundObjects()
        {
            List<BasicGameElement> backgroundObjects = new List<BasicGameElement>();
            return backgroundObjects;
        }

        public List<BasicGameElement> getForegroundObjects()
        {
            List<BasicGameElement> foregroundObjects = new List<BasicGameElement>();
            return foregroundObjects;
        }

        public Vector2 getPlayerStartPosition()
        {
            Vector2 startPos = new Vector2(25, 5);

            return startPos;
        }

        public BasicGameElement getLevelExit()
        {
            BasicGameElement exit = LoadBlock("Exit", CollisionType.Passable,new Vector2(800,200));
            return exit;
        }

        public double getMaxLevelTime()
        {
            return 99;
        }

        public Song getBackgroundMusic()
        {
            return level.Content.Load<Song>("Sounds/Backgroundmusic");
        }

        public HelperClasses.Bounds getLevelBounds()
        {
            return new HelperClasses.Bounds(0, 0, 5000, 500);
        }

        #endregion

        private BlockElement LoadBlock(string name, CollisionType collision, Vector2 position)
        {
            return new BlockElement("Tiles/" + name, collision, level, position);
        }

        private BlockElement LoadBlock(string name, CollisionType collision, Vector2 position, int width, int height)
        {
            return new BlockElement("Tiles/" + name, collision, level, position,width,height);
        }

        private SpellType[] useableSpells = { SpellType.ColdSpell, SpellType.CreateMatterSpell, SpellType.NoGravitySpell, SpellType.WarmingSpell, SpellType.PullSpell,SpellType.PushSpell,SpellType.ElectricSpell };
        public Spells.SpellType[] UsableSpells
        {
            get
            {
                return this.useableSpells;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        private Ingredient LoadIngredient(String name, CollisionType collision, Vector2 position)
        {
            return new Ingredient("Ingredients/" + name, collision, level, position);
        }

        private Ingredient LoadIngredient(String name, CollisionType collision, Vector2 position, int width, int height)
        {
            return new Ingredient("Ingredients/" + name, collision, level, position, width, height);
        }



        public int getMinimumItemsToEndLevel()
        {
            return 3;
        }

        public int getMaxItmesToCollect()
        {
            return 3;
        }


        public List<BlendInClasses.TutorialInstruction> GetTutorialInstructions()
        {
            List<TutorialInstruction> instructs = new List<TutorialInstruction>();
            
            return instructs;
        }
    }
}
