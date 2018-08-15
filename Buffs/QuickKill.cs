using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.GameInput;

namespace MusicMadness.Buffs
{
	public class QuickKill : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("R.I.P.");
			Description.SetDefault("Quick Kill...");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = false;
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
			longerExpertDebuff = false;
			canBeCleared = false;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.lifeRegen -= 9999999;
		}
		
        public virtual bool Autoload(ref string name, ref string texture)
		{
			return mod.Properties.Autoload;
		}
	}
}