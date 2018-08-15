using Terraria;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using System.Collections;
using System.IO;
using Terraria.ModLoader.IO;
using System.Linq;
using Terraria.Utilities;
using System;

namespace MusicMadness.Items
{
	public class MusicianVoodooDoll : ModItem
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Musician Voodoo Doll");
			Tooltip.SetDefault("'You are a terrible person.'");
		}
		
		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 24;
			item.maxStack = 1;
			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<Playerone>(mod).EquippedYourDoom = true;
		}
	}
}