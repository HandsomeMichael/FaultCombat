using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FaultCombat.Content.Buffs
{
	public class FastHandBuff : ModBuff
	{
		public override void Update(Player player, ref int buffIndex) 
		{
			if (player.TryGetModPlayer(out FaultPlayer fp))
			{
				fp.statAttackSpeed += 0.20f;
			}
		}
	}
}
