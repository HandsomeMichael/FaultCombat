


using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace FaultCombat.DataStructures;


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

    public void InputBlock(Mod Mod,Player Player,TriggersSet triggersSet)
    {
        
    }
    public void Blocking()
    {
        
    }

    public void ResetEffects()
    {
        
    }
}