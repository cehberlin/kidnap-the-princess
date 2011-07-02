using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MagicWorld;
using MagicWorld.DynamicLevelContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using MagicWorld.HelperClasses;
using MagicWorld.Spells;
using MagicWorld.BlendInClasses;

namespace MagicWorld.StaticLevelContent
{
    /// <summary>
    /// Interface for new Level Loaders
    /// </summary>
    public interface ILevelLoader
    {
        /// <summary>
        /// this function is only necessary to pass the level object which is necessary to create several 
        /// game objects; this function should be called first
        /// </summary>
        /// <param name="level"></param>
        void init(Level level);

        /// <summary>
        /// get the set of usabel spells in this level
        /// </summary>
        SpellType[] UsableSpells { get; set; }

        /// <summary>
        /// get all level elements which should be collision handled,
        /// </summary>
        /// <returns></returns>
        List<BasicGameElement> getInteractingObjects();

        /// <summary>
        /// get Gameelelements which are drawn and updated in background
        /// </summary>
        /// <returns></returns>
        List<BasicGameElement> getBackgroundObjects();

        /// <summary>
        /// get Gameelelements which are drawn and updated in foreground
        /// </summary>
        /// <returns></returns>
        List<BasicGameElement> getForegroundObjects();
        
        /// <summary>
        /// get position for player start (if its in the air) he will fall down
        /// </summary>
        /// <returns></returns>
        Vector2 getPlayerStartPosition();

        /// <summary>
        /// get exit element --> collision with it is level exit
        /// </summary>
        /// <returns></returns>
        BasicGameElement getLevelExit();

        /// <summary>
        /// get maximum time for finishing the level
        /// </summary>
        /// <returns></returns>
        double getMaxLevelTime();

        /// <summary>
        /// get level background music
        /// </summary>
        /// <returns></returns>
        Song getBackgroundMusic();

        /// <summary>
        /// bound (rectangle) around the whole level, if player collidates with this bounds he dies
        /// </summary>
        /// <returns></returns>
        Bounds getLevelBounds();


        /// <summary>
        /// minimum of collectable items the player has to collect in this level to finish it
        /// </summary>
        /// <returns></returns>
        int getMinimumItemsToEndLevel();

        /// <summary>
        /// max of collectable items the player has to collect in this level
        /// </summary>
        /// <returns></returns>
        int getMaxItmesToCollect();

        /// <summary>
        /// Gets the instructions the player will see on screen, helping him to finish the level or use the controls.
        /// </summary>
        /// <returns></returns>
        List<TutorialInstruction> GetTutorialInstructions();
    }
}
