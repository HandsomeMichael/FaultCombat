using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
namespace FaultCombat.Content.Items;


[JITWhenModsEnabled("CalamityMod")]
public class LoreOverride : GlobalItem
{
    // public override bool IsLoadingEnabled(Mod mod) => FaultCombat.HasCalamity();
    public override bool AppliesToEntity(Item entity, bool lateInstantiation)
    {
        return lateInstantiation && entity.ModItem is CalamityMod.Items.LoreItems.LoreItem;
    }

    public override bool CanRightClick(Item item) => true;
    public override void RightClick(Item item, Player player)
    {
        if (player.TryGetModPlayer(out LoreConsume lc))
        {
            lc.lores[LoreConsume.GetLore(item.ModItem.Name)] = true;
            CombatText.NewText(player.Hitbox,Color.Silver,"You can feel stamina flowing..");

            // Shimmer this shi
            Vector2 lastPos = item.Center;
            item.Center = player.Center;
            typeof(Item).GetMethod("GetShimmered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)?.Invoke(item, null);
            item.Center = lastPos;
        }
    }

    public override void PostDrawInInventory(Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
    {
        base.PostDrawInInventory(item, spriteBatch, position, frame, drawColor, itemColor, origin, scale);
    }
    
    public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
    {
        if (Main.LocalPlayer.TryGetModPlayer(out LoreConsume lc))
        {
            bool consumed = lc.lores[LoreConsume.GetLore(item.ModItem.Name)];
            tooltips.Add(new TooltipLine(Mod, "FaultCombat : lore", consumed ? "You already absorb this knowledge" : "Right-click to absorb knowledge, shimmering it in the process"));
        }
    }
}


public class LoreConsume : ModPlayer
{
    // public override bool IsLoadingEnabled(Mod mod) => FaultCombat.HasCalamity();
    public override void SaveData(TagCompound tag)
    {
        tag.Add("loreSaved",Pack(lores));
    }

    public override void LoadData(TagCompound tag)
    {
        if (tag.ContainsKey("loreSaved"))
        {
            lores = Unpack(tag.GetString("loreSaved"));
        }
    }

    public bool[] lores = new bool[64];

    public bool[] Unpack(string value)
    {
        bool[] bits = new bool[64];

        for (int i = 0; i < 64; i++)
        {
            bits[i] = value[i] == '1' ? true : false;
        }

        return bits;
    }

    public string Pack(bool[] bits)
    {
        string value = "";

        for (int i = 0; i < bits.Length && i < 64; i++)
        {
            value += bits[i] ? "1" : "0";
        }

        return value;
    }

    public override void PostUpdateEquips()
    {
        if (Player.TryGetModPlayer(out FaultPlayer fp))
        {
            foreach (bool lore in lores)
            {
                if (lore)
                {
                    fp.statMaxStamina += 0.5f;
                }
            }
        }
    }

    public static int GetLore(string lore)
    {
        switch (lore)
        {
            case "LoreArchmage": return 0;
            case "LoreAstralInfection": return 1;
            case "LoreAstrumAureus": return 2;
            case "LoreAstrumDeus": return 3;
            case "LoreAwakening": return 4;
            case "LoreAzafure": return 5;
            case "LoreBloodMoon": return 6;
            case "LoreBrainofCthulhu": return 7;
            case "LoreBrimstoneElemental": return 8;
            case "LoreCalamitas": return 9;
            case "LoreCalamitasClone": return 10;
            case "LoreCeaselessVoid": return 11;
            case "LoreCorruption": return 12;
            case "LoreCrabulon": return 13;
            case "LoreCrimson": return 14;
            case "LoreCynosure": return 15;
            case "LoreDesertScourge": return 16;
            case "LoreDestroyer": return 17;
            case "LoreDevourerofGods": return 18;
            case "LoreDragonfolly": return 19;
            case "LoreDukeFishron": return 20;
            case "LoreEaterofWorlds": return 21;
            case "LoreEmpressofLight": return 22;
            case "LoreExoMechs": return 23;
            case "LoreEyeofCthulhu": return 24;
            case "LoreGolem": return 25;
            case "LoreHiveMind": return 26;
            case "LoreKingSlime": return 27;
            case "LoreLeviathanAnahita": return 28;
            case "LoreMechs": return 29;
            case "LoreOldDuke": return 30;
            case "LorePerforators": return 31;
            case "LorePlaguebringerGoliath": return 32;
            case "LorePlantera": return 33;
            case "LorePolterghast": return 34;
            case "LorePrelude": return 35;
            case "LoreProfanedGuardians": return 36;
            case "LoreProvidence": return 37;
            case "LoreQueenBee": return 38;
            case "LoreQueenSlime": return 39;
            case "LoreRavager": return 40;
            case "LoreRequiem": return 41;
            case "LoreSignus": return 42;
            case "LoreSkeletron": return 43;
            case "LoreSkeletronPrime": return 44;
            case "LoreSlimeGod": return 45;
            case "LoreStormWeaver": return 46;
            case "LoreSulphurSea": return 47;
            case "LoreTwins": return 48;
            case "LoreUnderworld": return 49;
            case "LoreWallofFlesh": return 50;
            case "LoreYharon": return 51;
            case "LoreAbyss": return 52;
            case "LoreAquaticScourge": return 53;
            default: return 0;
        }
    }
}