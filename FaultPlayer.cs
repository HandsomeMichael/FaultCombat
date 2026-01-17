

using System;
using FaultCombat.DataStructures;
using FaultCombat.Utils;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace FaultCombat;

public class FaultPlayer : ModPlayer
{
    // Base speeds
    public int BaseRollTime => FaultConfig.Instance.RollTime;
    public float BaseRollSpeed => FaultConfig.Instance.RollSpeed;
    public int BaseRollCooldown => FaultConfig.Instance.RollCooldown;
    public float BaseRollCost => FaultConfig.Instance.RollCost;
    public float BaseStaminaRegen => FaultConfig.Instance.StaminaRegenRate;
    public float BaseMaxStamina => FaultConfig.Instance.StaminaBase;
    public int BasePerfectDodgeWindow => 4;

    // Dodge properties
    public int rollTime;
    public int staminaCooldown;
    public int staminaCooldownMax;
    public float stamina;
    public bool dodgedSomething;
    public bool autoRoll;
    public bool direction;
    public void SetRight() => direction = true;
    public void SetLeft() => direction = false;

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

    // static shit

    public void DepleteStamina(float amount)
    {
        stamina = Math.Max(0, stamina - amount);
    }

    public float GetMaxStamina()
    {
        return BaseMaxStamina + statMaxStamina;
    }

    public int GetRollTime()
    {
        return (int)(BaseRollTime * statRollTime);
    }
    public float GetRollSpeed()
    {
        return BaseRollSpeed * statRollSpeed;
    }

    public int GetRollCooldown()
    {
        return (int)(BaseRollCooldown * statRollCooldown);
    }

    public float GetDashCooldown()
    {
        return 0;
    }
    public override void ResetEffects()
    {
        statRollTime = 1f;
        statRollSpeed = 1f;
        statRollCooldown = 1f;
        statStaminaRegen = 0;
    }

    public bool RollAvailable() => stamina >= BaseRollCost; 
    public bool IsRolling => rollTime > 0;

    public override void ProcessTriggers(TriggersSet triggersSet)
    {
        if (IsRolling || Player.dead || Player.mount.Active || Player.CCed) return;
        bool dodgeKeyPressed = FaultCombat.KeyDodgeroll?.JustPressed ?? false;

        if (dodgeKeyPressed)
        {
            if (!RollAvailable())
            {
                SoundEngine.PlaySound(new SoundStyle("FaultCombat/Sounds/NoRoll"), Player.Center);
                // DodgerollMeterUISystem.NotEnoughStamina();
                return;
            }

            // Main.NewText("Local client dodged");
            Vector2 defaultDirection = new Vector2(Player.direction, 0);
            Vector2 velocity = triggersSet.DirectionsRaw.SafeNormalize(defaultDirection) * GetRollSpeed();

            // direction = triggersSet.DirectionsRaw.X == 0 ? Player.direction == 1 : (int)triggersSet.DirectionsRaw.X == 1;
            if (velocity.X == 0)
            {
                // have very little direction boost
                velocity.X = ( triggersSet.DirectionsRaw.X == 0 ? Player.direction : (int)triggersSet.DirectionsRaw.X ) * 0.01f;
            }

            InitiateRoll(velocity);

            // send this to the server lil bro
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                var modPacket = Mod.GetPacket();
                modPacket.Write((byte)FaultCombat.MessageType.Dodge);
                modPacket.WriteVector2(velocity);
                modPacket.Send();
            }
        }
    }

    public void ApplyBoost(Vector2 newVelocity)
    {
        if (newVelocity.X > 0)
        {
            SetRight();
            if (Player.velocity.X + newVelocity.X < newVelocity.X) { Player.velocity.X = newVelocity.X; }
            else { Player.velocity.X += newVelocity.X; }
        }
        else
        {
            SetLeft();
            if (Player.velocity.X + newVelocity.X > newVelocity.X) { Player.velocity.X = newVelocity.X; }
            else { Player.velocity.X += newVelocity.X; }
        }

        Player.velocity.Y += newVelocity.Y;
    }

    public void InitiateRoll(Vector2 newVelocity)
    {
        DepleteStamina(BaseRollCost);

        ApplyBoost(newVelocity);

        rollTime = GetRollTime();
        dodgedSomething = false;

        if (!Main.dedServ) SoundEngine.PlaySound(new SoundStyle("FaultCombat/Sounds/Roll" + Main.rand.Next(1, 4)
        ).WithVolumeScale(0.5f).WithPitchOffset(Main.rand.NextFloat(0.9f, 1.1f)), Player.Center);
    }

    public void InitiateAutoRoll()
    {
        direction = Player.direction == 1;
        DepleteStamina(BaseRollCost);

        rollTime = GetRollTime();
        dodgedSomething = true;

        if (!Main.dedServ) SoundEngine.PlaySound(new SoundStyle("FaultCombat/Sounds/Roll" + Main.rand.Next(1, 4)
        ).WithVolumeScale(1f).WithPitchOffset(Main.rand.NextFloat(1.1f, 1.2f)), Player.Center);
    }

    public override void PostUpdateBuffs()
    {
        
    }

    public override void PostUpdate()
    {
        // roll effect
        if (IsRolling)
        {
            Player.armorEffectDrawShadow = true;
            float progressMax = GetRollTime();
            var progress = 1 - ((float)rollTime) / progressMax;

            Player.direction = direction ? 1 : -1;
            Player.fullRotationOrigin = Player.Center - Player.position;
            Player.fullRotation = Player.direction * MathHelper.Lerp(0, MathHelper.TwoPi, progress);
        }
    }
    public override void PreUpdate()
    {
        // if (rollTimeMax > 0)
        // {
        //     rollTime++;
        //     if (rollTime >= rollTimeMax)
        //     {
        //         float rollCD = BaseRollCooldown;
        //         staminaCooldownMax = (int)(rollCD * statRollCooldown);

        //         // reset rotation
        //         Player.fullRotationOrigin = Player.Center - Player.position;
        //         Player.fullRotation = 0f;

        //         rollTime = 0;
        //         rollTimeMax = 0;
        //     }
        // }
        if (rollTime > 0)
        {
            rollTime--;
            if (rollTime <= 0)
            {
                float rollCD = BaseRollCooldown;
                staminaCooldownMax = (int)(rollCD * statRollCooldown);

                // reset rotation
                Player.fullRotationOrigin = Player.Center - Player.position;
                Player.fullRotation = 0f;
            }
        }
        else
        {
            if (staminaCooldownMax > 0)
            {
                staminaCooldown++;
                if (staminaCooldown >= staminaCooldownMax)
                {
                    staminaCooldown = 0;
                    staminaCooldownMax = 0;
                }

                Main.NewText("Cooldowning");
            }
            else
            {
                stamina = Math.Min(GetMaxStamina(),stamina + (( BaseStaminaRegen + statStaminaRegen ) / 60) );
            }
        }

        Main.NewText("player stamina : "+stamina);

    }

    public bool IsPerfectDodge()
    {
        return (GetRollTime() - rollTime) <= BasePerfectDodgeWindow;

        // return rollTimeMax > 0 && rollTime <= BasePerfectDodgeWindow;

        // int deltaDodge = GetRollCooldown() - staminaCooldown;
        // return deltaDodge <= BasePerfectDodgeWindow;
    }

    public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genDust, ref PlayerDeathReason damageSource)
    {
        if (IsRolling && IsPerfectDodge() && damageSource.IsOther(DeathReasonOtherID.FallDamage))
        {
            CombatText.NewText(Player.Hitbox, Color.Gold, "Lucky...", true);
            Player.statLife = 1;
            playSound = false;
            genDust = false;
            return false;
        }
        return base.PreKill(damage, hitDirection, pvp, ref playSound, ref genDust, ref damageSource);
    }

    public override bool FreeDodge(Player.HurtInfo info)
    {
        if (!IsRolling && autoRoll && RollAvailable() && (info.Damage > Player.statLife || Player.statLife <= 15) && !Player.CCed)
        {
            if (FaultConfigClient.Instance.ShowDodgeIndicator)
            {
                CombatText.NewText(Player.Hitbox, Color.Red, "Instinct!", true);
            }

            InitiateAutoRoll();

            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                ModPacket modPacket = Mod.GetPacket();
                modPacket.Write((byte)FaultCombat.MessageType.AutoDodgeServer);
                modPacket.Send();
            }
            return true;
        }
        return base.FreeDodge(info);
    }

    public override bool ImmuneTo(PlayerDeathReason damageSource, int cooldownCounter, bool dodgeable)
    {
        // dodge any undodgeeable stuff except fall damage
        if (IsRolling && !damageSource.IsOther(DeathReasonOtherID.FallDamage))
        {
            // handles dodge bonus
            if (!dodgedSomething && dodgeable)
            {
                GiveDodgeBonus(damageSource);
                dodgedSomething = true;
            }
            //Main.NewText("asd"+damageSource.SourceOtherIndex);

            // would still immune to any damage including those annoying cactus damage from fargo god i hate that thing
            return true;
        }
        return base.ImmuneTo(damageSource, cooldownCounter, dodgeable);
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

            // if (heldClass.CountsAsClass(DamageClass.Melee))
            // {
                
            // }
            // else if (heldClass.CountsAsClass(DamageClass.Ranged))
            // {
                
            // }
        }
    }

    public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
    {
        
    }
}