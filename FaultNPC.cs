using System.IO;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace FaultCombat;

/// <summary>
/// Staggerable NPCs, can only stagger once
/// </summary>
public class StaggerNPC : GlobalNPC
{
    public override bool InstancePerEntity => true;
    public int poise = 1;

    public override void SendExtraAI(NPC npc, BitWriter bitWriter, BinaryWriter binaryWriter)
    {
        binaryWriter.Write(poise);
    }

    public override void ReceiveExtraAI(NPC npc, BitReader bitReader, BinaryReader binaryReader)
    {
        poise = binaryReader.ReadInt32();
    }

    public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
    {
        return lateInstantiation && entity.defDefense > 10 && !entity.friendly && entity.lifeMax > 150;
    }

    public override void HitEffect(NPC npc, NPC.HitInfo hit)
    {
        if (poise > 0)
        {
            poise += hit.Damage;
            if (poise > npc.lifeMax / 2)
            {
                CombatText.NewText(npc.Hitbox, Color.Yellow, "Staggered!");
                poise = -60 * 5;
            }
        }
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