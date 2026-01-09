using System.IO;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
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
    public Vector2 dodgeDirection;

    public bool IsDodging => dodgeDirection != Vector2.Zero;

    public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
    {
        return lateInstantiation && entity.lifeMax > 100 && !entity.friendly;
    }

    public override bool CanHitPlayer(NPC npc, Player target, ref int cooldownSlot)
    {
        if (IsDodging)
        {
            return false;
        }
        return base.CanHitPlayer(npc, target, ref cooldownSlot);
    }

    public override void ModifyHitNPC(NPC npc, NPC target, ref NPC.HitModifiers modifiers)
    {
        base.ModifyHitNPC(npc, target, ref modifiers);
    }
}