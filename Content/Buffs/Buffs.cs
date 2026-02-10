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

	public class ShortStaminaRegen : ModBuff
	{
		public override void Update(Player player, ref int buffIndex) 
		{
			if (player.TryGetModPlayer(out FaultPlayer fp))
			{
				fp.statStaminaRegen += 1f;
			}
		}
	}

	public class FumbleDodge : ModBuff
	{
		public override void Update(Player player, ref int buffIndex) 
		{
			player.moveSpeed *= 0.75f;
		}
	}

	public class StaggerDebuff : ModBuff
	{
        public override void Update(NPC npc, ref int buffIndex)
        {
            if (npc.TryGetGlobalNPC(out StaggerNPC staggerNPC))
			{
				staggerNPC.staggerable = true;
			}
        }
	}
}
