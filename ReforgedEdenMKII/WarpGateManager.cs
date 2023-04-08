using Eleon.Modding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReforgedEdenMKII
{
    public static class WarpGateManager
    {
        private static readonly HashSet<string> LOADED_PLAYFIELDS;
        private static readonly Dictionary<string, List<WarpGate>> PLAYFIELD_WARPGATES;

        static WarpGateManager()
        {
            LOADED_PLAYFIELDS = new HashSet<string>();
            PLAYFIELD_WARPGATES = new Dictionary<string, List<WarpGate>>
            {
                {
                    "Andromeda to Decay Warp Gate",
                    new List<WarpGate>()
                    {
                        new WarpGate("Eden_BAO_ProgGate1", "Ancient Warp Gate", "Andromeda to Decay Warp Gate", "Decay to Andromeda Warp Gate")
                    }
                },
                {
                    "Decay to Andromeda Warp Gate",
                    new List<WarpGate>()
                    {
                        new WarpGate("Eden_BAO_ProgGate2", "Ancient Warp Gate", "Decay to Andromeda Warp Gate", "Andromeda to Decay Warp Gate")
                    }
                }
            };
        }

        internal static void OnGSL(ReforgedEdenMKII mod, GlobalStructureList gsl)
        {
            foreach (var kvp in gsl.globalStructures)
            {
                if (LOADED_PLAYFIELDS.Contains(kvp.Key))
                {
                    foreach (var wg in PLAYFIELD_WARPGATES[kvp.Key])
                    {
                        wg.OnGSL(mod, kvp.Value);
                    }
                }
            }
        }

        internal static void OnPlayerInfo(ReforgedEdenMKII mod, PlayerInfo pi)
        {
            if (LOADED_PLAYFIELDS.Contains(pi.playfield))
            {
                foreach (var wg in PLAYFIELD_WARPGATES[pi.playfield])
                {
                    wg.OnPlayerInfo(mod, pi);
                }
            }
        }
        
        internal static void OnPlayfieldLoaded(string playfield)
        {
            if (PLAYFIELD_WARPGATES.ContainsKey(playfield))
                LOADED_PLAYFIELDS.Add(playfield);
        }

        internal static void OnPlayfieldUnloaded(string playfield)
        {
            if (PLAYFIELD_WARPGATES.ContainsKey(playfield) && LOADED_PLAYFIELDS.Contains(playfield))
                LOADED_PLAYFIELDS.Remove(playfield);
        }

        internal static void OnUpdate(ReforgedEdenMKII mod)
        {
            foreach (var pf in LOADED_PLAYFIELDS)
            {
                foreach (var wg in PLAYFIELD_WARPGATES[pf])
                {
                    if (wg.LastUpdated < DateTime.Now.Ticks - 10000000)
                        wg.Update(mod);
                }
            }
        }
    }
}
