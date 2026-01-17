using Terraria;
using Terraria.ID;

namespace FaultCombat.Items;

public class Aglet : SingleItemPatch
{
    public override int Id => ItemID.Aglet;
    public override string Tooltip => "Grants a swiftness buff upon sucessfull dodgeroll";
    public override void UpdatePlayer(FaultPlayer faultPlayer)
    {
        faultPlayer.aglet = true;
    }
}
public class Anklet : SingleItemPatch
{
    public override int Id => ItemID.AnkletoftheWind;
    public override string Tooltip => "Grants an attack speed buff upon sucessfull dodgeroll";
    public override void UpdatePlayer(FaultPlayer faultPlayer)
    {
        faultPlayer.anklet = true;
    }
}

public class LightningBoots : SingleItemPatch
{
    public override int Id => ItemID.LightningBoots;
    public override string Tooltip => "Grant decreased dodgeroll cooldown by 10%\nIncrease max stamina by 1";
    public override void UpdatePlayer(FaultPlayer faultPlayer)
    {
        faultPlayer.statRollCooldown -= 0.1f;
        faultPlayer.statMaxStamina += 1;
    }
}

public class SpectreBoots : SingleItemPatch
{
    public override int Id => ItemID.SpectreBoots;
    public override string Tooltip => "Increase max stamina by 1";
    public override void UpdatePlayer(FaultPlayer faultPlayer)
    {
        faultPlayer.statMaxStamina += 1;
    }
}