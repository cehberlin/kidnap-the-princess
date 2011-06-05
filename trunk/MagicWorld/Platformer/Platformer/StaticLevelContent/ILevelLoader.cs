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

namespace MagicWorld.StaticLevelContent
{
    /// <summary>
    /// Interface for new Level Loaders
    /// </summary>
    public interface ILevelLoader
    {
        Level Level
        {
            get;
            set;
        }

        SpellType[] UsableSpells { get; set; }

        List<BasicGameElement> getGeneralObjects();
        
        Vector2 getPlayerStartPosition();

        BasicGameElement getLevelExit();

        double getMaxLevelTime();

        Song getBackgroundMusic();

        Bounds getLevelBounds();
    }
}
