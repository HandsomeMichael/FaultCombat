


using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace FaultCombat.DataStructures;

public struct DodgerollStat
{
    // Base speeds
    public int BaseTime => 30;
    public int BaseSpeed => 12;
    public float BaseRegen => 1;
    public int BaseCooldown => 45;
    public float BaseCost => 5;
    public float BaseMaxStamina => 10;

    // Dodge properties
    public int time;
    public int cooldown;
    public float stamina;
    public bool dodgedSomething;
    public bool direction;
    public void SetRight() => direction = true;
    public void SetLeft() => direction = false;

    // Buff
    public byte bonusType;
    public ushort bonusTimer;

    // Additive stats
    public float statTime;
    public float statSpeed;
    public float statCooldown;
    public int statRegen;
    public float statMaxStamina;

    public float GetMaxStamina()
    {
        return BaseMaxStamina + statMaxStamina;
    }

    public int GetTime()
    {
        return (int)(BaseTime * statTime);
    }
    public float GetSpeed()
    {
        return BaseSpeed * statSpeed;
    }

    public void ResetEffects()
    {
        statTime = 1f;
        statSpeed = 1f;
        statRegen = 0;
    }

    public bool Available() => BaseCost >= stamina; 
    public bool IsRolling => time > 0;

    public void InputRoll(Mod Mod,Player Player,TriggersSet triggersSet)
    {
        if (IsRolling || Player.dead || Player.mount.Active || Player.CCed) return;
        bool dodgeKeyPressed = FaultCombat.KeyDodgeroll?.JustPressed ?? false;

        if (dodgeKeyPressed)
        {
            if (!Available())
            {
                SoundEngine.PlaySound(new SoundStyle("DodgerollClamity/Sounds/NoRoll"), Player.Center);
                // DodgerollMeterUISystem.NotEnoughStamina();
                return;
            }

            // Main.NewText("Local client dodged");

            Vector2 defaultDirection = new Vector2(Player.direction, 0);
            Vector2 velocity = triggersSet.DirectionsRaw.SafeNormalize(defaultDirection) * GetSpeed();

            Initiate(Player,velocity);

            // send this to the server lil bro
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                var modPacket = Mod.GetPacket();
                modPacket.Write((byte)FaultCombat.MessageType.FuckingDodgeServer);
                modPacket.WriteVector2(velocity);
                modPacket.Send();
            }
        }
    }

    public void DepleteStamina(float amount)
    {
        stamina = Math.Max(0, stamina - amount);
    }

    public void ApplyBoost(Player Player,Vector2 newVelocity)
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

    public void Initiate(Player Player, Vector2 newVelocity)
    {
        DepleteStamina(BaseCost);

        ApplyBoost(Player,newVelocity);

        time = GetTime();
        dodgedSomething = false;

        if (!Main.dedServ) SoundEngine.PlaySound(new SoundStyle("FaultCombat/Sounds/Roll" + Main.rand.Next(1, 4)
        ).WithVolumeScale(0.5f).WithPitchOffset(Main.rand.NextFloat(0.9f, 1.1f)), Player.Center);
    }

    public void UpdateEye()
    {
        
    }

    public void PostUpdate(Player Player)
    {
        // roll effect
        if (IsRolling)
        {
            Player.armorEffectDrawShadow = true;
            float progressMax = GetTime();
            var progress = 1 - time / progressMax;

            Player.direction = direction ? 1 : -1;
            Player.fullRotationOrigin = Player.Center - Player.position;
            Player.fullRotation = Player.direction * MathHelper.Lerp(0, MathHelper.TwoPi, progress);
        }
    }

    public void PreUpdate(Player player)
    {
        if (time > 0) time--;
        if (cooldown > 0) cooldown--;
        else
        {
            stamina = Math.Min(GetMaxStamina(),stamina + BaseRegen + statRegen);
        }
        // if (staminaTimer > 0) staminaTimer--;
    }
}
