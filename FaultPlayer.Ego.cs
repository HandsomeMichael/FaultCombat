using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace FaultCombat;


public struct EgoRes
{
    public EgoRes(DamageClass affliction)
    {
        this.affliction = affliction;
        value = 0;
        level = 0;
    }

    public DamageClass affliction;
    public int value;
    public byte level;
    public const byte maxLevel = 32;
    public const int buildUpReq = 100;
    public void Reset()
    {
        value = 0;
        level = 0;
    }
    public void BuildUp(int add)
    {
        value += add;
        if (value >= buildUpReq)
        {
            if (level < maxLevel)
            {
                level++;
            }
            value = 0;
        }
    }

    public void Consume(byte cost = 0)
    {
        level -= cost;
        value = 0;
    }
}
public partial class FaultPlayer : ModPlayer
{

    // Ego List :

    public EgoRes egoWrath = new EgoRes(DamageClass.Melee);
    public EgoRes egoGloom = new EgoRes(DamageClass.Ranged);
    public EgoRes egoPride = new EgoRes(DamageClass.Magic);
    public EgoRes egoSloth = new EgoRes(DamageClass.Summon);
    public EgoRes egoEnvoy = new EgoRes(DamageClass.Throwing);
    public EgoRes egoLimbo = new EgoRes(DamageClass.Generic);

    public int egoTime;

    public void ResetEgo()
    {
        if (egoTime > 0)
        {
            egoTime--;
            if (egoTime <= 0)
            {
                egoWrath.Reset();
                egoGloom.Reset();
                egoPride.Reset();
                egoSloth.Reset();
                egoEnvoy.Reset();
                egoLimbo.Reset();
                Main.NewText("Ego resetted");
            }
        }
    }
    public void EgoCheck(NPC target, NPC.HitInfo hit, int damageDone)
    {
        if (hit.DamageType.CountsAsClass(egoWrath.affliction)){egoWrath.BuildUp(damageDone);}
        else if (hit.DamageType.CountsAsClass(egoGloom.affliction)){egoGloom.BuildUp(damageDone);}
        else if (hit.DamageType.CountsAsClass(egoPride.affliction)){egoPride.BuildUp(damageDone);}
        else if (hit.DamageType.CountsAsClass(egoSloth.affliction)){egoSloth.BuildUp(damageDone);}
        else if (hit.DamageType.CountsAsClass(egoEnvoy.affliction)){egoEnvoy.BuildUp(damageDone);}
        else {egoLimbo.BuildUp(damageDone);}

        Main.NewText($"Ego Build Up : W{egoWrath.level} G{egoGloom.level} P{egoPride.level} S{egoSloth.level} E{egoEnvoy.level} L{egoLimbo.level}");

        egoTime = 65;
    }
}