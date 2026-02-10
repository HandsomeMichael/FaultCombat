using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FaultCombat.Content.Items;

public abstract class WIPAccesory : ModItem
{
    public override string Texture => "FaultCombat/Content/Items/Placeholder";
    public virtual int Rare => ItemRarityID.Red;

    public override void SetDefaults()
    {
        Item.width = 10;
        Item.height = 10;
        Item.value = 10000;
        Item.rare = Rare;
        Item.accessory = true;
    }

    public virtual void UpdatePlayer(Player player,FaultPlayer fp,bool hideVisual)
    {
        
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        if (player.TryGetModPlayer(out FaultPlayer fp))
        {
            UpdatePlayer(player,fp,hideVisual);
        }
    }
}
public class AntiKillOrb : WIPAccesory
{
    public override void UpdatePlayer(Player player, FaultPlayer fp, bool hideVisual)
    {
        fp.antiKill = true;
    }
    public override void AddRecipes()
    {
        CreateRecipe()
            .AddRecipeGroup(RecipeGroupID.IronBar,10)
            .AddIngredient(ItemID.Wire,10)
            .AddIngredient(ItemID.Star,5)
            .AddTile(TileID.Anvils)
            .Register();
    }
}

public class NullifyWeight : WIPAccesory
{
    public override void UpdatePlayer(Player player, FaultPlayer fp, bool hideVisual)
    {
        fp.statWeightEff = 0f;
    }
}

public class HakariFever : WIPAccesory
{
    public override void ModifyTooltips(List<TooltipLine> tooltips)
    {
        if (Main.LocalPlayer.TryGetModPlayer(out FaultPlayer fp))
        {
            if (fp.hakariLuck > 0)
            {
                tooltips.Add(new TooltipLine(Mod,"Hakari : FeverUp","Have "));
            }
            else
            {
                tooltips.Add(new TooltipLine(Mod,"Hakari : Fever","Equip any vanity item to show fever"));
            }
        }
    }
    public override void UpdatePlayer(Player player, FaultPlayer fp, bool hideVisual)
    {
        fp.hakariFever = true;
    }
}