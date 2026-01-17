using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace FaultCombat;

public partial class FaultPlayer : ModPlayer
{
    // Buff
    public byte dodgeBonusType;
    public ushort dodgeBonusTimer;
    
    // Additive stats
    public float statRollTime;
    public float statRollSpeed;
    public float statRollCooldown;
    public float statStaminaRegen;
    public float statMaxStamina;

    // Nature power
    public bool powerPalm;
    public bool powerCorrupt;
    public bool powerBoreal;
    public bool powerMahogany;
    public bool powerNature;
    public bool powerAsh;

    // actives

    public bool aglet;
    public bool anklet;

    public override void ResetEffects()
    {
        statRollTime = 1f;
        statRollSpeed = 1f;
        statRollCooldown = 1f;
        statStaminaRegen = 0;
        aglet = false;
        anklet = false;
    }
    public void GiveDodgeBonus(PlayerDeathReason damageSource)
    {
        bool perfectDodge = IsPerfectDodge();
        var heldClass = Player.HeldItem.DamageType;

        if (perfectDodge)
        {

            if(!Main.dedServ && Player.whoAmI == Main.myPlayer)
            {
                // FaultPlayerAura.Aura();
                SoundEngine.PlaySound(new SoundStyle("FaultCombat/Sounds/Anime").WithVolumeScale(0.5f), Player.Center);
            }

            if (aglet)
            {
                Player.AddBuff(BuffID.Swiftness,60);
            }

            // if (heldClass.CountsAsClass(DamageClass.Melee))
            // {
                
            // }
            // else if (heldClass.CountsAsClass(DamageClass.Ranged))
            // {
                
            // }
        }
    }
}