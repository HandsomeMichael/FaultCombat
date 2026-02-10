using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace FaultCombat;

/// <summary>
/// Staggerable NPCs, can only stagger once
/// </summary>
// public class StaggerNPC : GlobalNPC
// {
//     public override bool InstancePerEntity => true;
//     public short poise = 0;
//     public bool staggerable;

//     public override void ResetEffects(NPC npc)
//     {
//         staggerable = false;
//     }

//     public short GetMaxPoise(NPC npc)
//     {
//         return (short)(npc.lifeMax / 2);
//     }

//     public override void SendExtraAI(NPC npc, BitWriter bitWriter, BinaryWriter binaryWriter)
//     {
//         binaryWriter.Write(poise);
//     }

//     public override void ReceiveExtraAI(NPC npc, BitReader bitReader, BinaryReader binaryReader)
//     {
//         poise = binaryReader.ReadInt16();
//     }

//     public static bool Staggerable(NPC entity)
//     {
//         return entity.defDefense > 5 && !entity.friendly && entity.lifeMax > 100;
//     }
//     public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
//     {
//         if (lateInstantiation && Staggerable(entity))
//         {
//             Mod.Logger.Info("Added "+entity.TypeName+" as staggerable enemy");
//             return true;
//         }
//         return false;
//     }

//     public override void SetDefaults(NPC entity)
//     {
//         poise = (short)(entity.lifeMax / 2);
//     }

//     public bool IsStaggered =>poise <= 0 && poise >= staggerTime;
//     public override void PostAI(NPC npc)
//     {
//         if (IsStaggered)
//         {
//             poise--;
//         }
//     }

//     public const short staggerTime = -60 * 10;
//     public override void ModifyIncomingHit(NPC npc, ref NPC.HitModifiers modifiers)
//     {
//         if (IsStaggered)
//         {
//             modifiers.Defense *= 0f;
//         }
//     }

//     public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
//     {
        
//     }

//     // public override void HitEffect(NPC npc, NPC.HitInfo hit)
//     // {
//     //     if (poise > 0)
//     //     {
//     //         poise -= (short)hit.Damage;
//     //         if (poise <= 0)
//     //         {
//     //             Main.NewText("Staggered");
//     //         }
//     //     }
//     // }

// }

public class DodgerollingNPC : GlobalNPC
{
    public override bool InstancePerEntity => true;
    public byte rollTime;
    public byte rollCooldown;
    public const byte rollTimeMax = 25;

    public bool IsDodging => rollTime > 0;

    // public readonly int[] rollables = {};

    public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
    {
        return NPCID.Sets.Zombies[entity.type] || entity.aiStyle == NPCAIStyleID.Fighter;
        // return lateInstantiation && entity.lifeMax > 100 && !entity.friendly;
    }

    public override void PostAI(NPC npc)
    {
        if (rollTime > 0)
        {
            rollTime--;
            var progress = 1 - ((float)rollTime) / (float)rollTimeMax;
            npc.rotation = npc.direction * MathHelper.Lerp(0, MathHelper.TwoPi, progress);
            if (rollTime == 0)
            {
                npc.rotation = 0f;
            }
        }
        if (rollCooldown > 0)
        {
            rollCooldown--;
        }
        // else if (npc.HasPlayerTarget)
        // {
        //     if (Main.player[npc.target].DistanceSQ(npc.Center) < 50 && Main.rand.NextBool(2))
        //     {
                
        //     }
        // }
    }

    public override void ModifyIncomingHit(NPC npc, ref NPC.HitModifiers modifiers)
    {
        if (!Main.expertMode) return;
        if (IsDodging) return;

        if (rollCooldown <= 0 && Main.rand.NextBool(FaultConfig.Instance.EnemyRollChance))
        {
            modifiers.DisableKnockback();
            modifiers.DisableCrit();
            modifiers.HideCombatText();
            modifiers.FinalDamage *= 0;
            CombatText.NewText(npc.Hitbox,Color.DimGray,"Miss");
            rollTime = rollTimeMax;
            if (npc.life > npc.lifeMax * 0.35f)
            {
                npc.velocity.X += npc.direction * 4f;
            }
            else
            {
                // retreat on low hp
                npc.velocity.X += npc.direction * -4f;
            }
            npc.velocity.Y -= 1f;
            rollCooldown = 250;
        }
        else
        {
            rollCooldown = 20;
        }
    }

    public override bool CanHitPlayer(NPC npc, Player target, ref int cooldownSlot)
    {
        if (IsDodging) return false;
        return base.CanHitPlayer(npc, target, ref cooldownSlot);
    }

    public override bool? CanBeHitByItem(NPC npc, Player player, Item item)
    {
        if (IsDodging) return false;
        return base.CanBeHitByItem(npc, player, item);
    }

    public override bool CanBeHitByNPC(NPC npc, NPC attacker)
    {
        if (IsDodging) return false;
        return base.CanBeHitByNPC(npc, attacker);
    }

    public override bool? CanBeHitByProjectile(NPC npc, Projectile projectile)
    {
        if (IsDodging) return false;
        return base.CanBeHitByProjectile(npc, projectile);
    }
}