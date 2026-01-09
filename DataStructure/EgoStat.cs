using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace FaultCombat.DataStructures;

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