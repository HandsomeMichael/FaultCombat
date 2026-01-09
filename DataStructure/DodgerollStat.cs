


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
    public int BaseTime;
    public int BaseSpeed;
    public int BaseRegen;
    public int BaseCooldown;
    public int BaseCost;

    // Dodge properties
    public int time;
    public int cooldown;
    public int stamina;
    public Vector2 velocity;
    public int GetDirection() => velocity.X > 0 ? 1 : -1;

    // Buff
    public byte bonusType;
    public ushort bonusTimer;

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

    public void ResetEffects()
    {
        statTime = 1f;
        statSpeed = 1f;
        statRegen = 1f;
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
            // bool direction = (triggersSet.DirectionsRaw.X == 0 ? Player.direction : (int)triggersSet.DirectionsRaw.X ) != -1;

            Initiate(Player,velocity);

            // send this to the server lil bro
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                ModPacket modPacket = Mod.GetPacket();
                modPacket.Write((byte)DodgerollClamity.MessageType.FuckingDodgeServer);
                modPacket.WriteVector2(dodgeBoost);
                modPacket.Send();
            }
        }
    }

    public void Initiate(Player player, Vector2 velocity)
    {
        
    }

    public void UpdateEye()
    {
        
    }

    public void Rotate(Player player)
    {
        
    }

    public void InitiateRoll(Player player)
    {
        
    }
}
