using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace FaultCombat.Content.Items;
public class PotionRestoreStamina : GlobalItem
{
    public float GetStamina(Item item)
    {
        if (item.healLife > 40)
        {
            return item.healLife / 50f;
        }
        return 0;
    }
    public override bool AppliesToEntity(Item entity, bool lateInstantiation)
    {
        return lateInstantiation && entity.healLife > 0;
    }

    public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
    {
        float stamina = GetStamina(item);
        if (stamina > 0)
        {
            tooltips.Add(new TooltipLine(Mod,"FaultCombat : Grant Stamina",$"Grant {stamina} stamina"));

            if (Main.LocalPlayer.TryGetModPlayer(out FaultPlayer fp))
            {
                fp.calamityPlayer?.GrandGelatinTooltips(tooltips,fp);
            }
        }
    }
    public override bool? UseItem(Item item, Player player)
    {
        if (player.TryGetModPlayer(out FaultPlayer fp))
        {
            float stamina = GetStamina(item);
            if (stamina > 0)
            {
                fp.stamina += stamina;
            }

            // only do it client side bc im too lazy to sync this shi
            if (!Main.dedServ && player.whoAmI == Main.myPlayer) fp.calamityPlayer?.GrandGelatinEffect(fp);
        }
        return base.UseItem(item, player);
    }
}