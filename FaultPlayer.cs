

using Terraria;
using Terraria.GameInput;
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

    public int buildup;
    public int cooldown;
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
    public bool direction;

    public bool IsRight => direction;
    public bool IsLeft => !direction;

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

public struct BlockStat
{
    public int poise;
    public int cooldown;
    public bool blocking;

    public void Update(Player player)
    {
        int maxPoise = ( player.statLifeMax2 / 2 ) * (player.statDefense / 2);

        if (poise <= 0)
        {
            player.statDefense = Player.DefenseStat.Default;
        }

        if (cooldown > 0)
        {
            cooldown--;
        }
        else
        {
            if (poise < maxPoise)
            {
                poise++;
            }
        }
    }

    public void Blocking()
    {
        
    }

    public void ResetEffects()
    {
        
    }
}

public class FaultPlayer : ModPlayer
{
    public EgoStat ego;
    public DodgerollStat roll;
    public BlockStat block;

    public override void ProcessTriggers(TriggersSet triggersSet)
    {
        // dont do dodgeroll
        // if (!DodgerollConfig.Instance.EnableDodgeroll || state != DodgerollState.NONE || Player.dead || Player.mount.Active || Player.CCed) return;

        // bool dodgeKeyPressed = DodgerollKey?.JustPressed ?? false;
        // bool haveStamina = Stamina >= GetStaminaUsage();

        // if (dodgeKeyPressed)
        // {
        //     if (!haveStamina || !DodgerollAvailable())
        //     {
        //         SoundEngine.PlaySound(new SoundStyle("DodgerollClamity/Sounds/NoRoll"), Player.Center);
        //         DodgerollMeterUISystem.NotEnoughStamina();
        //         return;
        //     }

        //     // Main.NewText("Local client dodged");

        //     var defaultDirection = new Vector2(Player.direction, 0);
        //     var dodgeBoost = triggersSet.DirectionsRaw.SafeNormalize(defaultDirection) * DodgerollConfig.Instance.DodgerollBoost * (1f + statDodgeBoost);
        //     var dodgeDirection = triggersSet.DirectionsRaw.X == 0 ? Player.direction : (int)triggersSet.DirectionsRaw.X;

        //     InitiateDodgeroll(dodgeBoost, dodgeDirection);

        //     // send this to the server lil bro
        //     if (Main.netMode == NetmodeID.MultiplayerClient)
        //     {
        //         ModPacket modPacket = Mod.GetPacket();
        //         modPacket.Write((byte)DodgerollClamity.MessageType.FuckingDodgeServer);
        //         modPacket.WriteVector2(dodgeBoost);
        //         modPacket.Write(dodgeDirection);
        //         modPacket.Send();
        //     }
        // }
    }

    // public override void ModifyHurt(ref Player.HurtModifiers modifiers)
    // {
    //     if (block.poise > 0 && block.blocking)
    //     {
    //         modifiers.FinalDamage *= 0f;
    //     }
    // }

    public override void ResetEffects()
    {
        block.ResetEffects();
        roll.ResetEffects();
        roll.UpdateEye();
    }

    public override void PostUpdate()
    {

        block.Update(Player);
        // roll effect
        roll.Rotate(Player);
    }
    
}