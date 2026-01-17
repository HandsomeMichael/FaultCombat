

using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FaultCombat.Items;

public abstract class SingleItemPatch : GlobalItem
{
    public virtual int Id => ItemID.None;
    public override bool AppliesToEntity(Item entity, bool lateInstantiation)
    {
        return entity.type == Id;
    }

    public override void UpdateEquip(Item item, Player player)
    {
        if (player.TryGetModPlayer<FaultPlayer>(out var faultPlayer))
        {
            UpdatePlayer(faultPlayer);
        }
    }

    public virtual void UpdatePlayer(FaultPlayer faultPlayer) {}
    public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
    {
        if (Tooltip != "")
        {
            tooltips.Add(new TooltipLine(Mod, "FaultCombat: Patch Tooltip", Tooltip));
        }
    }

    public virtual string Tooltip => "";
}
public abstract class ModItemPatch : GlobalItem
{
    public override bool AppliesToEntity(Item entity, bool lateInstantiation)
    {
        return entity.ModItem != null && entity.ModItem.Mod.Name == ModName && ItemName == entity.ModItem.Name;
    }
    public virtual string ItemName => "cum";
    public virtual string ModName => "CalamityMod";
    public override bool IsLoadingEnabled(Mod mod)
    {
        return ModLoader.HasMod(ModName);
    }

    public override void UpdateEquip(Item item, Player player)
    {
        if (player.TryGetModPlayer<FaultPlayer>(out var faultPlayer))
        {
            UpdatePlayer(faultPlayer);
        }
    }

    public virtual void UpdatePlayer(FaultPlayer faultPlayer) {}
}

public abstract class ModItemPatchMultiple : GlobalItem
{
    public override bool AppliesToEntity(Item entity, bool lateInstantiation)
    {
        return entity.ModItem != null && entity.ModItem.Mod.Name == ModName && ItemName.Contains(entity.ModItem.Name);
    }
    public virtual string[] ItemName => new string[] { "cum" };
    public virtual string ModName => "CalamityMod";

    public override bool IsLoadingEnabled(Mod mod)
    {
        return ModLoader.HasMod(ModName);
    }

    public override void UpdateEquip(Item item, Player player)
    {
        if (player.TryGetModPlayer<FaultPlayer>(out var faultPlayer))
        {
            UpdatePlayer(faultPlayer);
        }
    }

    public virtual void UpdatePlayer(FaultPlayer faultPlayer) {}
}