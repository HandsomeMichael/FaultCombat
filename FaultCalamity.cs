using Terraria.ModLoader;

namespace FaultCombat;

[JITWhenModsEnabled("CalamityMod")]
public class FaultCalamityPlayerData
{
    public static FaultCalamityPlayerData Load()
    {
        if (ModLoader.GetMod("CalamityMod") == null) return null;
        var data = new FaultCalamityPlayerData();
        return data; 
    }

    
}