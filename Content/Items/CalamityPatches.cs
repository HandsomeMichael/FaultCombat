

using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FaultCombat.Content.Items;

public class SupremeCalamityDrop : ModItemPatch
{
    public override string ItemName => "Calamity";
    public override string Tooltip => "Can now dodge any undodgeable attacks";
}

public class PAngelTreads : ModItemPatch
{
    public override string ItemName => "AngelTreads";
    public override string Tooltip => "Dodgeroll have a chance to dodge undodgeable attacks\nGrant decreased roll cooldown by 10%\nIncrease max stamina by 2";
}

public class PMoonWalkers : ModItemPatch
{
    public override string ItemName => "MoonWalkers";
    public override string Tooltip => "Dodgeroll have a chance to dodge undodgeable attacks\nGrant decreased roll cooldown by 10%\nIncrease max stamina by 3";
}

public class PVoidStriders : ModItemPatch
{
    public override string ItemName => "VoidStriders";
    public override string Tooltip => "Dodgeroll have a chance to dodge undodgeable attacks\nGrant decreased roll cooldown by 15%\nIncrease max stamina by 4";
}

public class PTracersSeraph : ModItemPatch
{
    public override string ItemName => "TracersSeraph";
    public override string Tooltip => "Dodgeroll have a chance to dodge undodgeable attacks\nGrant decreased roll cooldown by 15%\nIncrease max stamina by 5";
}

public class PWingsofRebirth : ModItemPatch
{
    public override string ItemName => "WingsofRebirth";
    public override string Tooltip => "Increase max stamina by 10";
    public override void UpdatePlayer(FaultPlayer faultPlayer)
    {
        faultPlayer.statMaxStamina += 10f;
    }
}

public class PIronBoots : ModItemPatch
{
    public override string ItemName => "IronBoots";
    public override string Tooltip => "Increase dodgeroll stamina by 2 underwater";
    public override void UpdatePlayer(FaultPlayer faultPlayer)
    {
        if (faultPlayer.Player.wet)
        {
            faultPlayer.statMaxStamina += 2f;
        }
    }
}

public class PGrandGelatin : ModItemPatch
{
    public override string ItemName => "GrandGelatin";
    public override string Tooltip => "Using any healing potion below 50% health grant extra dodgeroll";
}

//The transformer , Hide of Astrum Deus , Frost Barrier, Craw Carapace , Deific Amulet
// Void of calamity , Yharim Gift , Amidias Spark, Alchemical Flask, Rampart of Deities

// public class CalamityHurtEffects : ModItemPatchMultiple 
// {
//     public override string[] ItemName => ["YharimsGift","HideofAstrumDeus"];
//     public override string Tooltip => "Also applies on perfect dodgeroll";
// }