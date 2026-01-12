

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
    public int BaseRollTime => 25;
    public int BaseRollSpeed => 12;
    public int BaseRollCooldown => 84;
    public float BaseRollCost => 5f;
    public float BaseStaminaRegen => 4f;
    public float BaseMaxStamina => 5f;
    public int BasePerfectDodgeWindow => 5;

    // Dodge properties
    public int rollTime;
    public int staminaCooldown;
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
    public int statStaminaRegen;
    public float statMaxStamina;

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
        statStaminaRegen = 0;
    }

    public bool RollAvailable() => BaseRollCost >= stamina; 
    public bool IsRolling => rollTime > 0;

    public override void ProcessTriggers(TriggersSet triggersSet)
    {
        if (IsRolling || Player.dead || Player.mount.Active || Player.CCed) return;
        bool dodgeKeyPressed = FaultCombat.KeyDodgeroll?.JustPressed ?? false;

        if (dodgeKeyPressed)
        {
            if (!RollAvailable())
            {
                SoundEngine.PlaySound(new SoundStyle("DodgerollClamity/Sounds/NoRoll"), Player.Center);
                // DodgerollMeterUISystem.NotEnoughStamina();
                return;
            }

            // Main.NewText("Local client dodged");
            Vector2 defaultDirection = new Vector2(Player.direction, 0);
            Vector2 velocity = triggersSet.DirectionsRaw.SafeNormalize(defaultDirection) * GetRollSpeed();

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
        DepleteStamina(BaseRollCost);

        rollTime = GetRollTime();
        dodgedSomething = true;

        if (!Main.dedServ) SoundEngine.PlaySound(new SoundStyle("FaultCombat/Sounds/Roll" + Main.rand.Next(1, 4)
        ).WithVolumeScale(0.5f).WithPitchOffset(Main.rand.NextFloat(0.9f, 1.1f)), Player.Center);
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
        if (rollTime > 0)
        {
            rollTime--;
            if (rollTime == 0)
            {
                float rollCD = BaseRollCooldown;
                staminaCooldown = (int)(rollCD * statRollCooldown);

                // reset rotation
                Player.fullRotationOrigin = Player.Center - Player.position;
                Player.fullRotation = 0f;
            }
        }

        if (staminaCooldown > 0) staminaCooldown--;
        else
        {
            stamina = Math.Min(GetMaxStamina(),stamina + BaseStaminaRegen + statStaminaRegen);
        }


        // if (staminaTimer > 0) staminaTimer--;
    }

    public bool IsPerfectDodge()
    {
        int deltaDodge = GetRollCooldown() - staminaCooldown;
        return deltaDodge <= BasePerfectDodgeWindow;
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
        if (!IsRolling && autoRoll && stamina == GetMaxStamina() && (info.Damage > Player.statLife || Player.statLife <= 50) && !Player.CCed)
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
        
    }

    public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
    {
        
    }
}