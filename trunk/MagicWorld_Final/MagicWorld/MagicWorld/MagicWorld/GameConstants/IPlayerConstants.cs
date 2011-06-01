using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicWorld.GameConstants
{
    public interface IPlayerConstants
    {
        float MAXJUMPTIME { get; set; }

        float JUMPLAUNCHVELOCITY { get; set; }

        float GRAVITYACCELARATION { get; set; }

        float MAXFALLSPEED { get; set; }

        float JUMPCONTROLPOWER { get; set; }

        double MAXNOGRAVITYTIME { get; set; }

        float MOVEACCELERATION { get; set; }

        float MAXMOVESPEED { get; set; }

        float GROUNDDRAGFACTOR { get; set; }

        float AIRDRAGFACTOR { get; set; }
    }
}
