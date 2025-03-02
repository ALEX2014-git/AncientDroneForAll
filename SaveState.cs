using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AncientDroneForAll
{
    public partial class AncientDroneForAll
    {
        private string SaveState_SaveToString(On.SaveState.orig_SaveToString orig, global::SaveState self)
        {
            Logger.LogWarning("Initiated Save procedure.");
            if (AncientDroneForAll.hasDrone == true)
            {
                Logger.LogWarning("Added hasDrone info to save string.");
                self.unrecognizedSaveStrings.Add("ARTYDRONEFORALL:HASDRONE");
            }
            if (AncientDroneForAll.isDroneResynced == true)
            {
                Logger.LogWarning("Added droneResynced info to save string.");
                self.unrecognizedSaveStrings.Add("ARTYDRONEFORALL:DRONERESYNCED");
            }
            if (AncientDroneForAll.rivDroneTalk == true)
            {
                Logger.LogWarning("Added rivDroneTalk info to save string.");
                self.unrecognizedSaveStrings.Add("ARTYDRONEFORALL:RIVDRONETALK");
            }
            string saveData = orig(self);

            return saveData;   
        }

        private void SaveState_LoadGame(On.SaveState.orig_LoadGame orig, SaveState self, string str, RainWorldGame game)
        {
            orig(self, str, game);

            Logger.LogWarning("Resetting drone values before checking Save String.");
            AncientDroneForAll.hasDrone = false;
            AncientDroneForAll.isDroneResynced = false;
            AncientDroneForAll.rivDroneTalk = false;
            Logger.LogWarning("Starting SaveState.LoadGme co-routine.");
            if (self.unrecognizedSaveStrings.Contains("ARTYDRONEFORALL:HASDRONE"))
            {
                Logger.LogWarning("Found hasDrone value. Setting up.");
                AncientDroneForAll.hasDrone = true;
            }

            if (self.unrecognizedSaveStrings.Contains("ARTYDRONEFORALL:DRONERESYNCED"))
            {
                Logger.LogWarning("Found droneSynced value. Setting up.");
                AncientDroneForAll.isDroneResynced = true;
            }

            if (self.unrecognizedSaveStrings.Contains("ARTYDRONEFORALL:RIVDRONETALK"))
            {
                Logger.LogWarning("Found rivDroneTalk value. Setting up.");
                AncientDroneForAll.rivDroneTalk = true;
            }

            Logger.LogWarning("Ending SaveState.LoadGme co-routine.");

        }


    }
}
