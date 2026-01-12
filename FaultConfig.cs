using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace FaultCombat
{
    public class FaultConfigClient : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;
        public static FaultConfigClient Instance;

        [Header("Gameplay")]

        [DefaultValue(true)]
        public bool AccesoryAutoRoll { get; set; }

        [DefaultValue(false)]
        public bool HorizontalRollOnly { get; set; }

        [DefaultValue(true)]
        public bool ShowDodgeIndicator { get; set; }

        [Header("UI")]

        [DefaultValue(true)]
        public bool StaminaBarShow { get; set; }

        [DefaultValue(true)]
        public bool StaminaBarCooldown { get; set; }

        [DefaultValue(StaminaBarPos.TOP)]
        public StaminaBarPos StaminaBarPosition { get; set; }

        [Slider]
        [DefaultValue(15)]
        public int StaminaBarOffset { get; set; }
        
        [Slider]
        [Range(0, 100)]
        [DefaultValue(85)]
        public int StaminaBarOpacity { get; set; }

        public enum StaminaBarPos
        {
            TOP,
            BOTTOM,
            LEFT,
            RIGHT
        }

    }

    public class FaultConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;
        public static FaultConfig Instance;

        [Header("Dodgeroll")]

        [DefaultValue(true)]
        public bool RollEnable { get; set; }

        [Slider]
        [DefaultValue(5)]
        public float RollCost { get; set; }

        [Slider]
        [DefaultValue(85)]
        public float RollCooldown { get; set; }

        [Slider]
        [DefaultValue(25)]
        public int RollTime { get; set; }

        [Slider]
        [DefaultValue(7)]
        public float RollSpeed { get; set; }

        [DefaultValue(false)]
        public bool RollUseItem { get; set; }

        [DefaultValue(true)]
        public bool RollCancelUseItem { get; set; }

        [DefaultValue(false)]
        public bool RollPerfectTriggerHurt { get; set; }

        [Header("Stamina")]

        [Slider]
        [DefaultValue(5f)]
        public float StaminaBase { get; set; }

        [Slider]
        [DefaultValue(0.4f)]
        public float StaminaRegenRate { get; set; }

        [Header("Balances")]

        [DefaultValue(false)]
        public bool DashRequireStamina { get; set; }
    }
}