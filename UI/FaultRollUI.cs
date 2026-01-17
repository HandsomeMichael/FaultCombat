using System.Collections.Generic;
using FaultCombat.TunaFishUtil;
using FaultCombat.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.UI;

namespace FaultCombat.UI;

[Autoload(Side = ModSide.Client)]
public class PlayerReviveCirclesUIState : ModSystem
{
	private static Asset<Texture2D> backTexture;
	private static Asset<Texture2D> fillTexture;
	private static Asset<Effect> shader;
    
    public override void PostSetupContent()
    {
		backTexture = ModContent.Request<Texture2D>($"FaultCombat/UI/StaminaBack2");
		fillTexture = ModContent.Request<Texture2D>($"FaultCombat/UI/StaminaFill2");
		shader = ModContent.Request<Effect>($"FaultCombat/Shaders/RadialMask");
	}

    private const string VanillaInterfaceLayer = "Vanilla: Entity Health Bars";
    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
    {
        int index = layers.FindIndex(layer => layer.Name.Equals(VanillaInterfaceLayer)) + 1;
        if (index != -1)
        {
            layers.Insert(index, new LegacyGameInterfaceLayer("FaultCombatStamina" + ": UI",
                delegate
                {
                    DrawStaminaBar(Main.LocalPlayer);
                    return true;
                },
                InterfaceScaleType.UI));
        }
    }

    public override void Unload()
    {
        backTexture.Dispose();
        backTexture = null;
        fillTexture.Dispose();
        fillTexture = null;
        shader.Dispose();
        shader = null;
    }

    public float opacity = 1f;
	public void DrawStaminaBar(Player player) 
    {

        if (player == null || !player.active || player.dead) return;
        if (!player.TryGetModPlayer(out FaultPlayer faultPlayer)) return;

        float fillProgress = faultPlayer.stamina / faultPlayer.GetMaxStamina();
        shader.Value.Parameters["progress"].SetValue(fillProgress);
        shader.Value.Parameters["textureSize"].SetValue(fillTexture.Size());

		DrawData backDrawData = new() {
			texture = backTexture.Value,
			position = (player.Center - Main.screenPosition).Floor() + new Vector2(40f, 40f),
			sourceRect = backTexture.Frame(),
			origin = backTexture.Size() / 2f,
			color = Color.White * opacity,
			scale = new Vector2(1f),
		};

        DrawData fillDrawData = backDrawData with {
			texture = fillTexture.Value,
            sourceRect = fillTexture.Frame(),
			origin = fillTexture.Size() / 2f,
		};

        SpriteBatchParams circleSBParams = SpriteBatchParams.Default;

        Main.spriteBatch.TakeSnapshotAndEnd(out SpriteBatchParams sbParams);
        Main.spriteBatch.Begin(circleSBParams);

        backDrawData.Draw(Main.spriteBatch);

        Main.spriteBatch.End();

        circleSBParams.Effect = shader.Value;

        Main.spriteBatch.Begin(circleSBParams with { Effect = shader.Value });
        
		fillDrawData.Draw(Main.spriteBatch);

		Main.spriteBatch.End();
        Main.spriteBatch.Begin(sbParams);
	}
}
