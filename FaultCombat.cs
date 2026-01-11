using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace FaultCombat
{
    public class FaultCombat : Mod
	{
		public static bool HasCalamity() => calamity != null;
		public static Mod calamity = null;
		public static ModKeybind KeyDodgeroll { get; set; }
		public static ModKeybind KeyBlock { get; set; }

        public override void Load()
		{
			KeyDodgeroll = KeybindLoader.RegisterKeybind(this, "Dodgeroll", Keys.LeftControl);
			KeyBlock = KeybindLoader.RegisterKeybind(this, "Block", Keys.LeftAlt);
		}

        public override void Unload()
        {
            calamity = null;
			KeyDodgeroll = null;
			KeyBlock = null;
        }

		
		// current only does dodge, later i would problaby add parry , cancel dodge, cancel dash, cancel item use , throw weapon 
		public enum MessageType : byte
		{
			FuckingDodge, FuckingDodgeServer,
			InstinctDodged, InstinctDodgedServer,
			Stagger,StaggerServer,
			Block,BlockServer,
			Throw,ThrowServer
		}
		
		public override void HandlePacket(BinaryReader reader, int whoAmI)
		{
			MessageType msgType = (MessageType)reader.ReadByte();
            FaultPlayer modPlayer;
            switch (msgType)
			{
				// server be like : hmm i should send all ts to other client except for you again
				case MessageType.FuckingDodgeServer:
					if (whoAmI != 255)
					{
						var velocity = reader.ReadVector2();

						// does the server handle shit ? fuck no
						if (Main.player[whoAmI].TryGetModPlayer(out modPlayer)) modPlayer.roll.ApplyBoost(modPlayer.Player,velocity);

						ModPacket modPacket = GetPacket();
						modPacket.Write((byte)MessageType.FuckingDodge);
						modPacket.Write((byte)whoAmI);
						modPacket.WriteVector2(velocity);
						modPacket.Send(-1, whoAmI);
					}
					else
					{
						Logger.WarnFormat("Dodgeroll: packet shouldve not been sent to a client, wtf", msgType);
					}
					break;
				// client be like : uhhhh. okay
				case MessageType.FuckingDodge:
				
					byte playerNumber = reader.ReadByte();
					var boost = reader.ReadVector2();
					// int direction = reader.ReadInt16();
					// Main.player[playerNumber].GetModPlayer<DodgerollPlayer>().InitiateDodgeroll(boost, direction);
					
					break;

				//sync instinct dodge
				case MessageType.InstinctDodgedServer:
					if (whoAmI != 255)
					{
						ModPacket modPacket = GetPacket();
						modPacket.Write((byte)MessageType.FuckingDodge);
						modPacket.Write((byte)whoAmI);
						modPacket.Send(-1, whoAmI);
					}
					else
					{
						Logger.WarnFormat("Dodgeroll: packet shouldve not been sent to a client, wtf", msgType);
					}
					break;

				case MessageType.InstinctDodged:
					int pNum = reader.ReadByte();
					if (Main.player[pNum].TryGetModPlayer(out modPlayer)) modPlayer.roll.InstinctDodged();
					break;
				default:
					Logger.WarnFormat("Dodgeroll: Unknown Message type: {0}", msgType);
					break;
			}
		}
    }
}
