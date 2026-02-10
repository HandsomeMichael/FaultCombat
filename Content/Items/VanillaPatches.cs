using Terraria;
using Terraria.ID;

namespace FaultCombat.Content.Items;

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

public class TerraBoots : SingleItemPatch
{
    public override int Id => ItemID.TerrasparkBoots;
    public override string Tooltip => "Grant decreased roll cooldown by 10%\nIncrease max stamina by 2";
    public override void UpdatePlayer(FaultPlayer faultPlayer)
    {
        faultPlayer.statRollCooldown -= 0.1f;
        faultPlayer.statMaxStamina += 2;
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

public class PutridScent : SingleItemPatch
{
    public override int Id => ItemID.PutridScent;
    public override string Tooltip => "True melee attacks regenerates stamina";
    public override void UpdatePlayer(FaultPlayer faultPlayer)
    {
        faultPlayer.putrid = true;
    }
}

public class AccesoryHurtEffects : MultipleItemPatch
{
    public override int[] Id => 
    [ItemID.CrossNecklace,ItemID.StarVeil,ItemID.BeeCloak,
    ItemID.HoneyComb,ItemID.PanicNecklace,ItemID.StarCloak,
    ItemID.StingerNecklace,ItemID.SweetheartNecklace];
    public override string Tooltip => "Also applies on [c/F6AE2A:perfect] dodgeroll";
}

// public class FrozenShield : SingleItemPatch
// {
//     public override int Id => ItemID.FrozenShield;
//     public override string Tooltip => "Freeze nearby enemies when shield broke";
// }


