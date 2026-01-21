using System.IO;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace FaultCombat;

/// <summary>
/// Staggerable NPCs, can only stagger once
/// </summary>
public class StaggerNPC : GlobalNPC
{
    public override bool InstancePerEntity => true;
    public short poise = 1;

    public override void SendExtraAI(NPC npc, BitWriter bitWriter, BinaryWriter binaryWriter)
    {
        binaryWriter.Write(poise);
    }

    public override void ReceiveExtraAI(NPC npc, BitReader bitReader, BinaryReader binaryReader)
    {
        poise = binaryReader.ReadInt16();
    }

    public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
    {
        return lateInstantiation && entity.defDefense > 10 && !entity.friendly && entity.lifeMax > 150;
    }

    public override void PostAI(NPC npc)
    {
        if (poise < 0)
        {
            poise++;
        }
    }

    public override void ModifyIncomingHit(NPC npc, ref NPC.HitModifiers modifiers)
    {
        if (poise < 0)
        {
            modifiers.Defense *= 0f;
        }
    }

}

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
        return NPCID.Sets.Zombies[entity.type];
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