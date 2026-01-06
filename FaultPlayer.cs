

using Terraria;
using Terraria.ModLoader;
namespace FaultPlayer;


public struct EgoStat
{

    // Ego List :

    // Wrath > Melee
    // Gloom > Ranger
    // Pride > Magic
    // Sloth > Summoner
    // Envoy > Rogue
    // Limbo > Generic / Accesories

    public int wrath;
    public int gloom;
    public int pride;
    public int sloth;
    public int envoy;
    public int limbo;

}

public struct DodgerollStat
{
    // Base speeds
    public int BaseTime;
    public int BaseSpeed;
    public int BaseRegen;
    public int BaseCooldown;

    // Dodge properties
    public int time;
    public int cooldown;
    public float stamina;
    public byte type;

    // Additive stats
    public float statTime;
    public float statSpeed;
    public float statRegen;
    public float statCooldown;

    public int GetTime()
    {
        return (int)(BaseTime * statTime);
    }
    public float GetSpeed()
    {
        return BaseSpeed * statSpeed;
    }

    public void ResetEffect()
    {
        statTime = 1f;
        statSpeed = 1f;
        statRegen = 1f;
    }

    public void Rotate(Player player)
    {
        
    }

    public void InitiateRoll(Player player)
    {
        
    }
}

public struct PoiseStat
{
    
}

public class FaultPlayer : ModPlayer
{
    public EgoStat ego;
    public DodgerollStat roll;
    public PoiseStat poise;

    public override void ResetEffects()
    {
    }

    public override void PostUpdate()
    {
        // roll effect
        roll.Rotate(Player);
    }
    
}