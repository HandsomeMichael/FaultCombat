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
    public override string Tooltip => "Grant decreased roll cooldown by 10%\nIncrease max stamina by 1";
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

public class Magiluminescence : SingleItemPatch
{
    public override int Id => ItemID.Magiluminescence;
    public override string Tooltip => "Small chance to roll without consuming stamina";
}

public class Shoespikes : SingleItemPatch
{
    public override int Id => ItemID.ShoeSpikes;
    public override string Tooltip => "Increase roll speed by 5%";
}

public class ClimbingClaws : SingleItemPatch
{
    public override int Id => ItemID.ClimbingClaws;
    public override string Tooltip => "Increase roll speed by 5%";
}

public class Tabi : SingleItemPatch
{
    public override int Id => ItemID.Tabi;
    public override string Tooltip => "Increase roll speed by 10%";
}

public class BlackBelt : SingleItemPatch
{
    public override int Id => ItemID.BlackBelt;
    public override bool AutoRoll => true;
}
public class CobaltShield : SingleItemPatch
{
    public override int Id => ItemID.CobaltShield;
    public override string Tooltip => "Rolling bash nearby enemies";
}

public class EyeOfGolem : SingleItemPatch
{
    public override int Id => ItemID.EyeoftheGolem;
    public override string Tooltip => "Increase perfect dodgeroll threshold";
}

public class FleshKnuckles : SingleItemPatch
{
    public override int Id => ItemID.FleshKnuckles;
    public override string Tooltip => "Melee attacks restore 10% of lost stamina";
}

public class FrozenShield : SingleItemPatch
{
    public override int Id => ItemID.FrozenShield;
    public override string Tooltip => "Shield broke longer";
}

