

using System;
using FaultCombat.DataStructures;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace FaultCombat;

public class FaultPlayer : ModPlayer
{
    public EgoStat ego;
    public DodgerollStat roll;
    public BlockStat block;

    public bool penetrator;

    public override void ProcessTriggers(TriggersSet triggersSet)
    {
        roll.InputRoll(Mod,Player,triggersSet);
        block.InputBlock(Mod,Player,triggersSet);
    }

    // public override void ModifyHurt(ref Player.HurtModifiers modifiers)
    // {
    //     if (block.poise > 0 && block.blocking)
    //     {
    //         modifiers.FinalDamage *= 0f;
    //     }
    // }

    public override void PreUpdate()
    {
        roll.PreUpdate(Player);
        // block.PreUpdate(Player);
    }
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
        roll.PostUpdate(Player);
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        base.OnHitNPC(target, hit, damageDone);
    }
    
}