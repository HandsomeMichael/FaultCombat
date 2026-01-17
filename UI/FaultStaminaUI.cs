using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Light;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace FaultCombat.UI
{

    [Autoload(Side = ModSide.Client)]
    public class StaminaBarUI : ModSystem
    {
        private const string VanillaInterfaceLayer = "Vanilla: Entity Health Bars";
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int index = layers.FindIndex(layer => layer.Name.Equals(VanillaInterfaceLayer)) + 1;
            if (index != -1)
            {
                layers.Insert(index, new LegacyGameInterfaceLayer("FaultCombatStamina" + ": UI",
                    delegate
                    {
                        DrawPlayerMeter(Main.spriteBatch);
                        return true;
                    },
                    InterfaceScaleType.UI));
            }
        }

        public override void UpdateUI(GameTime gameTime)
        {
            var player = Main.LocalPlayer;
            if (player == null || player.dead || !player.active || player.CCed) return;
            if (!player.TryGetModPlayer(out FaultPlayer faultPlayer)) return;

            if (faultPlayer.staminaCooldown == 0 || faultPlayer.stamina == 0f)
            {
                lastStamina = MathHelper.Lerp(lastStamina,faultPlayer.stamina,0.2f);
            }
            if (fadingTimer > 0 && faultPlayer.stamina >= faultPlayer.GetMaxStamina()) fadingTimer--;
            if (faultPlayer.stamina < faultPlayer.GetMaxStamina() && fadingTimer != fadingLength)
            {
                fadingTimer = fadingLength;
            }
        }

        public const byte shakeMax = 20;
        public static void NotEnoughStamina()
        {
            if (!Main.dedServ)
                ModContent.GetInstance<StaminaBarUI>().shake = shakeMax;
        }

        public byte shake;
        public byte fadingTimer;
        public const byte fadingLength = 40;
        public float lastStamina;
        public float lastDownHud;
                
        public void DrawPlayerMeter(SpriteBatch spriteBatch)
        {
            var player = Main.LocalPlayer;
            if (player == null || player.dead || !player.active || player.CCed) return;
            if (!player.TryGetModPlayer(out FaultPlayer faultPlayer)) return;

            // if (fadingTimer > 0 && dodgeroll.Stamina >= dodgeroll.MaxStamina) fadingTimer--;
            // if (dodgeroll.Stamina < dodgeroll.MaxStamina && fadingTimer != fadingLength)
            // {
            //     fadingTimer = fadingLength;
            // }

            var opacity = fadingTimer / (float)fadingLength * (((float)FaultConfigClient.Instance.StaminaBarOpacity) / 100f);
            var progress = faultPlayer.stamina / faultPlayer.GetMaxStamina();

            float cdProgress = 0f;
            if (faultPlayer.IsRolling)
            {
                cdProgress = 1f - faultPlayer.GetRollTime() / faultPlayer.rollTime;
                //cdProgress = 1f - (float)faultPlayer.dodgerollTimer / (float)dodgeroll.GetDodgeMax();
            }
            // else
            // {
            //     cdProgress = (float)faultPlayer.staminaCooldownMax / faultPlayer.staminaCooldown;
            //     // cdProgress = (float)dodgeroll.staminaTimer / (float)dodgeroll.GetStaminaCD();
            // }

            var posType = FaultConfigClient.Instance.StaminaBarPosition;

            Vector2 position;
            switch (posType)
            {
                case FaultConfigClient.StaminaBarPos.TOP:position = player.Top - new Vector2(0,FaultConfigClient.Instance.StaminaBarOffset);break;
                case FaultConfigClient.StaminaBarPos.BOTTOM:position = player.Bottom + new Vector2(0,FaultConfigClient.Instance.StaminaBarOffset);break;
                case FaultConfigClient.StaminaBarPos.LEFT:position = player.Left - new Vector2(FaultConfigClient.Instance.StaminaBarOffset,0);break;
                case FaultConfigClient.StaminaBarPos.RIGHT:position = player.Right + new Vector2(FaultConfigClient.Instance.StaminaBarOffset,0);break;
                default:
                    position = player.Bottom + new Vector2(0,FaultConfigClient.Instance.StaminaBarOffset);
                    break;
            }
            position -= Main.screenPosition;
            //position.Y += DodgerollConfig.Instance.StaminaPositionOffset;
            position += new Vector2(Main.rand.Next(-shake, shake), Main.rand.Next(-shake, shake)) / 2f;
            position /= Main.UIScale;
            if (shake > 0) shake--;

            var barTexture = ModContent.Request<Texture2D>("FaultCombat/UI/StaminaBar").Value;
            var barNope = ModContent.Request<Texture2D>("FaultCombat/UI/StaminaBar_Nope").Value;
            var frameTexture = ModContent.Request<Texture2D>("FaultCombat/UI/StaminaFrame").Value;
            var frameBack = ModContent.Request<Texture2D>("FaultCombat/UI/StaminaFrame_Back").Value;
            var staminaCD = ModContent.Request<Texture2D>("FaultCombat/UI/StaminaCD").Value;
            var barRec = new Rectangle(0, 0, (int)(barTexture.Width * progress), barTexture.Height);
            var barNopeRec = new Rectangle(0, 0, (int)(barTexture.Width * lastStamina), barTexture.Height);
            var staminaCDRec = new Rectangle(0, 0, (int)(barTexture.Width * cdProgress), barTexture.Height);
            var orig = frameTexture.Size() / 2f;

            var lightColor = Lighting.GetColor((int)player.Bottom.X / 16, (int)player.Bottom.Y / 16);
            var color =  lightColor * opacity;//new Color(defactoColor,defactoColor,defactoColor) * opacity;
            float rotation = 0f;
            SpriteEffects effect = SpriteEffects.None;

            if (posType == FaultConfigClient.StaminaBarPos.LEFT || posType == FaultConfigClient.StaminaBarPos.RIGHT)
            {
                rotation = MathHelper.ToRadians(90);
                //effect = SpriteEffects.FlipHorizontally;
            }


            spriteBatch.Draw(frameBack, position, null, color, rotation, orig, 1f, effect, 0f);
            if (opacity > 0f) spriteBatch.Draw(barNope, position, barNopeRec, color, rotation, orig, 1f, effect, 0f);
            spriteBatch.Draw(barTexture, position, barRec, color, rotation, orig, 1f, effect, 0f);
            spriteBatch.Draw(frameTexture, position, null, color, rotation, orig, 1f, effect, 0f);

            spriteBatch.Draw(staminaCD, position, staminaCDRec, color, rotation, orig, 1f, effect, 0f);

        }
    }
}