using System;
using System.Collections.Generic;
using FaultCombat.TunaFishUtil;
using FaultCombat.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Chat;

namespace FaultCombat.UI;

[Autoload(Side = ModSide.Client)]
public class FaultStaminaUI : ModSystem
{
    public SpriteBatchParams uiSpriteBatch = null;
	private Asset<Texture2D> backTexture;
    // private Asset<Texture2D> redTexture;
	private Asset<Texture2D> fillTexture;
	private Asset<Effect> shader;
    
    public override void PostSetupContent()
    {
        // redTexture = ModContent.Request<Texture2D>($"FaultCombat/UI/StaminaFill2");
		backTexture = ModContent.Request<Texture2D>($"FaultCombat/UI/StaminaBack3");
		fillTexture = ModContent.Request<Texture2D>($"FaultCombat/UI/StaminaFill3");
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

    // Disposing this broke it ig ?

    // public override void Unload()
    // {
    //     backTexture.Dispose();
    //     backTexture = null;
    //     fillTexture.Dispose();
    //     fillTexture = null;
    //     shader.Dispose();
    //     shader = null;
    // }

    public float opacity = 1f;
    public float fillProgressCached = -1f;
    // public float fillProgress2Cached = -1f;
	public void DrawStaminaBar(Player player) 
    {

        if (player == null || !player.active || player.dead) return;
        if (!player.TryGetModPlayer(out FaultPlayer faultPlayer)) return;

        if (faultPlayer.stamina >= faultPlayer.GetMaxStamina())
        {
            opacity = Math.Max(0f,opacity - 0.02f);
        }
        else
        {
            opacity = Math.Min(1f,opacity + 0.05f);
        }

        float fillProgress = 1f - (faultPlayer.stamina / faultPlayer.GetMaxStamina());

        if (fillProgressCached == -1f)
        {
            fillProgressCached = fillProgress;
            // fillProgress2Cached = fillProgress;
        }
        else
        {
            fillProgressCached = MathHelper.Lerp(fillProgressCached,fillProgress,0.1f);
            // if (faultPlayer.staminaCooldownMax > 0)
            // {
            //     // fillProgress2Cached = MathHelper.Lerp(fillProgress2Cached,fillProgress,faultPlayer.staminaCooldown / faultPlayer.staminaCooldownMax);
            //     fillProgress2Cached = MathHelper.Lerp(fillProgress2Cached,fillProgress,0.1f);
            // }
        }

        shader.Value.Parameters["textureSize"].SetValue(fillTexture.Size());
        shader.Value.Parameters["progress"].SetValue(fillProgressCached);

        var posType = FaultConfigClient.Instance.StaminaBarPosition;

        Vector2 position;
        switch (posType)
        {
            case FaultConfigClient.StaminaBarPos.Top:position = player.Top - new Vector2(0,FaultConfigClient.Instance.StaminaBarOffset);break;
            case FaultConfigClient.StaminaBarPos.Bottom:position = player.Bottom + new Vector2(0,FaultConfigClient.Instance.StaminaBarOffset);break;
            case FaultConfigClient.StaminaBarPos.Left:position = player.Left - new Vector2(FaultConfigClient.Instance.StaminaBarOffset,0);break;
            case FaultConfigClient.StaminaBarPos.Right:position = player.Right + new Vector2(FaultConfigClient.Instance.StaminaBarOffset,0);break;
            default:
                position = player.Bottom + new Vector2(0,FaultConfigClient.Instance.StaminaBarOffset);
                break;
        }

        var lightColor = Lighting.GetColor((int)position.X/16,(int)position.Y/16);

		DrawData backDrawData = new() {
			texture = backTexture.Value,
			position = position - Main.screenPosition,
			sourceRect = backTexture.Frame(),
			origin = backTexture.Size() / 2f,
			color = lightColor * opacity * ((float)FaultConfigClient.Instance.StaminaBarOpacity / 100f),
			scale = new Vector2(1f),
		};

        DrawData fillDrawData = backDrawData with {
			texture = fillTexture.Value,
            sourceRect = fillTexture.Frame(),
			origin = fillTexture.Size() / 2f,
		};

        // DrawData fillDrawData2 = fillDrawData with {
		// 	texture = redTexture.Value
		// };

        SpriteBatchParams circleSBParams = SpriteBatchParams.Default;

        if (uiSpriteBatch == null)
        {
            Main.spriteBatch.TakeSnapshot(out uiSpriteBatch);
        }
        Main.spriteBatch.End();
        Main.spriteBatch.Begin(circleSBParams);

        backDrawData.Draw(Main.spriteBatch);

        if (FaultConfigClient.Instance.StaminaBarNumber && opacity > 0)
        {
            DynamicSpriteFont font = FontAssets.MouseText.Value;
            var size = Vector2.One * 0.6f;
            string text = ((int)faultPlayer.stamina).ToString();
            var parsed = ChatManager.ParseMessage(text,Color.White * opacity).ToArray();
            var parsedSize = ChatManager.GetStringSize(font, parsed, size);
            ChatManager.DrawColorCodedString(Main.spriteBatch,font,parsed ,fillDrawData.position + new Vector2(-2,FaultConfigClient.Instance.StaminaBarOffset + 2),Color.White * opacity,0f,parsedSize/2f,size, out _,100);
            // ChatManager.DrawColorCodedString(Main.spriteBatch,font, parsed,fillDrawData.position,0f,parsedSize / 2f,size,out _);
        }


        Main.spriteBatch.End();

        circleSBParams.Effect = shader.Value;

        Main.spriteBatch.Begin(circleSBParams with { Effect = shader.Value });

        // shader.Value.Parameters["progress"].SetValue(fillProgress2Cached);
        // fillDrawData2.Draw(Main.spriteBatch);

		// Main.spriteBatch.End();
        // Main.spriteBatch.Begin(circleSBParams with { Effect = shader.Value });

        // shader.Value.Parameters["progress"].SetValue(fillProgressCached);
		fillDrawData.Draw(Main.spriteBatch);

		Main.spriteBatch.End();
        Main.spriteBatch.Begin(uiSpriteBatch);
	}
}
