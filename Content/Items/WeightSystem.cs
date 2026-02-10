using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace FaultCombat.Content.Items;

/// <summary>
/// Very WIP, didnt know how to fucking balance this lil bro
/// </summary>
public class WeightSystem : GlobalItem
{
    public override bool IsLoadingEnabled(Mod mod)
    {
        return FaultConfig.Instance.WeightEnable;
    }
    public float GetWeight(Item item)
    {
        //(RarityLoader.RarityCount * 0.25f) ;


        // Calamity integrations
        if (item.ModItem != null && item.ModItem.Mod.Name == "CalamityMod")
        {
            switch (item.ModItem.Name)
            {
                case "CosmicCarKey": return 0f;
                default: break;
            }
        }

        if (item.vanity || item.hasVanityEffects)
        {
            return 0;
        }

        if (item.wingSlot > 0)
        {
            return -0.1f * Math.Max(3,item.rare);
        }
        if (item.shoeSlot > 0 || item.balloonSlot > 0)
        {
            return 0;
        }

        if (item.rare >= ItemRarityID.Count)
        {
            // max weight is 4
            return Math.Min(4f,( ItemRarityID.Count / 4f ) + (( item.rare - ItemRarityID.Count ) / 8f));
        }
        return Math.Min(4,Math.Max(0,item.rare) / 4f ) ;
    }
    public override bool AppliesToEntity(Item entity, bool lateInstantiation)
    {
        return lateInstantiation && entity.accessory;
    }

    public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
    {
        var weight = GetWeight(item);

        for (int i = 0; i < tooltips.Count; i++)
        {
            if (tooltips[i].Name == "Equipable")
            {
                string text;
                if (FaultConfigClient.Instance.TooltipsAdvanced && weight > 0f)
                {
                    text = $"{weight} Weight";
                }
                else
                {
                    text = WeightText(weight);
                }
                tooltips.Insert(i+1,new TooltipLine(Mod,"FaultCombat : Weight",text));
                return;
            }
        }
    }

    public string WeightText(float weight)
    {
        if (weight < 0f)
        {
            return "Reduced "+((int)(weight * 100f))+"% total weight";
        }

        if (weight > 3.5f) {return "Insanely heavy weight";}
        else if (weight > 3f) {return "Very heavy weight";}
        else if (weight > 2f) {return "Heavy weight";}
        else if (weight > 1f) {return "Normal weight";}
        else if (weight > 0.5f) {return "Light weight";}
        return "Very light weight";
    }
    public override void UpdateAccessory(Item item, Player player, bool hideVisual)
    {
        if (player.TryGetModPlayer(out FaultPlayer fp))
        {
            var weight = GetWeight(item);
            if (weight > 0f)
            {
                fp.statWeight += weight;
            }
            else
            {
                fp.statWeightEff += weight;
            }
        }
    }
}