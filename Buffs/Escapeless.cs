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
	public class Escapeless : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Out of Escape Ropes");
			Description.SetDefault("You seem to be stuck...");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = false;
			Main.buffNoTimeDisplay[Type] = true;
			longerExpertDebuff = false;
			canBeCleared = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.buffTime[buffIndex] = 18000;
			player.GetModPlayer<Playerone>(mod).Escapeless = true;
		}
		
        public virtual bool Autoload(ref string name, ref string texture)
		{
			return mod.Properties.Autoload;
		}
	}
}