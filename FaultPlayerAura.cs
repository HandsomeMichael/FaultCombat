// using FaultCombat.Utils;
// using Microsoft.Xna.Framework;
// using Terraria;
// using Terraria.DataStructures;
// using Terraria.ModLoader;

// namespace FaultCombat;
// public class FaultPlayerAura : ModSystem
// {

//     public override void Load()
//     {
//         timer = 31;
//     }

//     public int timer;
//     public static void Aura()
//     {
//         ModContent.GetInstance<FaultPlayerAura>().timer = 0;
//     }
//     public override void PostDrawTiles()
//     {
//         if (!Main.LocalPlayer.active || Main.LocalPlayer.dead) return;
//         if (timer > 30) return;

//         timer++;
//         float progress = timer / 30;

//         Main.spriteBatch.BeginNormal();
//         Main.PlayerRenderer.DrawPlayer(Main.Camera, Main.LocalPlayer, Main.LocalPlayer.position, 0, Vector2.Zero, 0.9f, timer/ 6);
//         Main.spriteBatch.End();
//     }


// }