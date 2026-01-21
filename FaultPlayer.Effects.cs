using System;
using FaultCombat.Content.Buffs;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace FaultCombat;

public partial class FaultPlayer : ModPlayer
{
    // Additive stats
    public float statRollTime;
    public float statRollSpeed;
    public float statRollCooldown;
    public float statStaminaRegen;
    public float statMaxStamina;
    public float statAttackSpeed;

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
    public bool eyeOfGolem;

    public override void ResetEffects()
    {
        statRollTime = 1f;
        statRollSpeed = 1f;
        statRollCooldown = 1f;
        statStaminaRegen = 0;
        statMaxStamina = 0;
        statAttackSpeed = 0f;
        aglet = false;
        anklet = false;
        eyeOfGolem = false;
    }

    public override void PostUpdateEquips()
    {
        statRollSpeed += Player.spikedBoots * 0.5f;
        if (Player.dashType == 1)
        {
            statRollSpeed += 0.1f;
        }
        if (Player.blackBelt)
        {
            autoRoll = true;
        }
    }

    public string UpdateSet(int head,int body,int legs)
    {
        if ((head == 5 || head == 74) && (body == 5 || body == 48) && (legs == 5 || legs == 44))
        {
            statMaxStamina += 2;
            return "Increase stamina by 2";
        }

        if (head == 22 && ( body == 14 || body == ItemID.Gi) && legs == 14)
        {
            statMaxStamina += 3;
            return "Increase stamina by 3";
        }

        if (head == 57 && body == 37 && legs == 35)
        {
            statStaminaRegen += 1f;
            return "Increase stamina regeneration by 1/s";
        }

        return "";
    }

    public override float UseSpeedMultiplier(Item item)
    {
        return base.UseSpeedMultiplier(item) + statAttackSpeed;
    }

    public bool IsPerfectDodge()
    {
        int window = BasePerfectDodgeWindow;
        if (eyeOfGolem)
        {
            window += 2;
        }

        return (GetRollTime() - rollTime) <= window;

        // return rollTimeMax > 0 && rollTime <= BasePerfectDodgeWindow;

        // int deltaDodge = GetRollCooldown() - staminaCooldown;
        // return deltaDodge <= BasePerfectDodgeWindow;
    }

    public void DepleteStamina(float amount)
    {
        if (Player.hasMagiluminescence)
        {
            if (Main.rand.NextBool(10))
            {
                amount = 0;
            }
        }
        stamina = Math.Max(0, stamina - amount);
    }

    // Buff
    public byte dodgeBonusType;
    public ushort dodgeBonusTimer;
    
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

        if (anklet)
        {
            Player.AddBuff(ModContent.BuffType<FastHandBuff>(),60);
        }
    }
}