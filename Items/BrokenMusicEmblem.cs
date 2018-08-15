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
	public class BrokenMusicEmblem : ModItem
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("A Broken Music Emblem?");
			Tooltip.SetDefault("Aww... How Sad...");
		}
		
		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 24;
			item.maxStack = 1;
            item.value = 50000;
            item.rare = 4;
		}
	}
}