using System;
using FaultCombat.Content.Buffs;
using Microsoft.Xna.Framework;
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
    public float statWeightEff;
    public float statWeight;

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
    public bool meleeStagger;
    public bool antiKill;
    public bool putrid;

    // ITS GAMBLING TIME WOOHOO
    public bool hakariFever;
    public byte hakariCoin;
    public byte hakariRedCoin;
    public byte hakariLuck;

    public override void ResetEffects()
    {
        statRollTime = 1f;
        statRollSpeed = 1f;
        statRollCooldown = 1f;
        statStaminaRegen = 0;
        statMaxStamina = 0;
        statAttackSpeed = 0f;
        statWeight = 0f;
        statWeightEff = 1f;
        aglet = false;
        anklet = false;
        eyeOfGolem = false;
        antiKill = false;
        putrid = false;
    }

    public void CalculateWeight()
    {
        statMaxStamina -= statWeight * statWeightEff;
    }
    public override void PostUpdateEquips()
    {
        statRollSpeed += Player.spikedBoots * 0.5f;
        if (Player.dashType == 1)
        {
            statRollSpeed += 0.1f;
        }
        // if (Player.speed)
        if (Player.blackBelt)
        {
            autoRoll = true;
        }

        calamityPlayer?.PostUpdateEquip(this,Player);
    }

    public void AddStagger()
    {

    }

    public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
    {
        if (putrid)
        {
            Player.AddBuff(ModContent.BuffType<ShortStaminaRegen>(), 120);
        }
    }
    public void OnHitEffect(NPC target, NPC.HitInfo hit, int damageDone)
    {
        // Stagger WIP

        // if (meleeStagger && hit.DamageType.CountsAsClass(DamageClass.Melee))
        // {
        //     if (StaggerNPC.Staggerable(target) && target.TryGetGlobalNPC(out StaggerNPC stg))
        //     {
        //         stg.poise += (short)damageDone;
        //     }
        // }
    }

    public override bool CanHitNPC(NPC target)
    {
        if (antiKill && target.life <= target.lifeMax * 0.05f)
        {
            return false;
        }
        return base.CanHitNPC(target);
    }

    public string UpdateSet(int head, int body, int legs)
    {
        // Shadow
        if ((head == 5 || head == 74) && (body == 5 || body == 48) && (legs == 5 || legs == 44))
        {
            statMaxStamina += 5;
            return "Increase stamina by 5";
        }

        if (head == 22 && (body == 14 || body == ItemID.Gi) && legs == 14)
        {
            statMaxStamina += 6;
            return "Increase stamina by 6";
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

    public void RestoreStamina(float amount)
    {
        stamina = Math.Min(GetMaxStamina(), stamina + amount);
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

        // REGULAR DODGE
        if (aglet)
        {
            Player.AddBuff(BuffID.Swiftness, 200);
        }

        if (anklet)
        {
            Player.AddBuff(ModContent.BuffType<FastHandBuff>(), 60);
        }

        // PERFECT ONLY
        if (perfectDodge)
        {

            if (!Main.dedServ && Player.whoAmI == Main.myPlayer)
            {
                // FaultPlayerAura.Aura();
                SoundEngine.PlaySound(new SoundStyle("FaultCombat/Sounds/Anime").WithVolumeScale(0.35f), Player.Center);
            }

            // if (heldClass.CountsAsClass(DamageClass.Melee))
            // {

            // }
            // else if (heldClass.CountsAsClass(DamageClass.Ranged))
            // {

            // }

            if (FaultConfig.Instance.RollPerfectTriggerHurt)
            {

                var info = new Player.HurtInfo
                {
                    DamageSource = damageSource,
                    CooldownCounter = ImmunityCooldownID.General
                };

                VanillaHurtEffects(damageSource);
                calamityPlayer?.HurtEffect(info, Player);

                // if (!Main.dedServ && Player.whoAmI == Main.myPlayer)
                // {
                //     int prevImmune = Player.immuneTime;

                //     PlayerLoader.PostHurt(Player, info);
                //     PlayerLoader.OnHurt(Player, info);
                    
                //     Player.immuneTime = prevImmune;
                // }
            }
        }
    }

    public void VanillaHurtEffects(PlayerDeathReason deathReason)
    {
        if (Player.starCloakItem != null && !Player.starCloakItem.IsAir)
        {
            for (int num15 = 0; num15 < 3; num15++)
            {
                float x = Player.position.X + (float)Main.rand.Next(-400, 400);
                float y = Player.position.Y - (float)Main.rand.Next(500, 800);
                Vector2 vector = new Vector2(x, y);
                float num16 = Player.position.X + (float)(Player.width / 2) - vector.X;
                float num17 = Player.position.Y + (float)(Player.height / 2) - vector.Y;
                num16 += (float)Main.rand.Next(-100, 101);
                float num18 = (float)Math.Sqrt(num16 * num16 + num17 * num17);
                num18 = 23f / num18;
                num16 *= num18;
                num17 *= num18;
                int type = 726;
                Item item = Player.starCloakItem;
                if (Player.starCloakItem_starVeilOverrideItem != null)
                {
                    item = Player.starCloakItem_starVeilOverrideItem;
                    type = 725;
                }

                if (Player.starCloakItem_beeCloakOverrideItem != null)
                {
                    item = Player.starCloakItem_beeCloakOverrideItem;
                    type = 724;
                }

                if (Player.starCloakItem_manaCloakOverrideItem != null)
                {
                    item = Player.starCloakItem_manaCloakOverrideItem;
                    type = 723;
                }

                int num19 = 75;
                if (Main.masterMode)
                    num19 *= 3;
                else if (Main.expertMode)
                    num19 *= 2;

                Projectile.NewProjectile(Player.GetSource_Accessory_OnHurt(item, deathReason), x, y, num16, num17, type, num19, 5f, Player.whoAmI, 0f, Player.position.Y);
            }
        }

        if (Player.honeyCombItem != null && !Player.honeyCombItem.IsAir)
        {
            int num20 = 1;
            if (Main.rand.Next(3) == 0)
                num20++;

            if (Main.rand.Next(3) == 0)
                num20++;

            if (Player.strongBees && Main.rand.Next(3) == 0)
                num20++;

            float num21 = 13f;
            if (Player.strongBees)
                num21 = 18f;

            if (Main.masterMode)
                num21 *= 2f;
            else if (Main.expertMode)
                num21 *= 1.5f;

            IEntitySource projectileSource_Accessory = Player.GetSource_Accessory_OnHurt(Player.honeyCombItem, deathReason);
            for (int num22 = 0; num22 < num20; num22++)
            {
                float speedX = (float)Main.rand.Next(-35, 36) * 0.02f;
                float speedY = (float)Main.rand.Next(-35, 36) * 0.02f;
                Projectile.NewProjectile(projectileSource_Accessory, Player.position.X, Player.position.Y, speedX, speedY, Player.beeType(), Player.beeDamage((int)num21), Player.beeKB(0f), Main.myPlayer);
            }

            Player.AddBuff(BuffID.Honey, 300);

        }
    }
}