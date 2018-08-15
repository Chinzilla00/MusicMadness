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
	public class MusicEmblem : ModItem
	{	
		public bool Shiny = false;
		public bool Glitch = false;
		public bool Beefy = false;
		public bool Soulless = false;
		public bool Debuffless = false;
		public bool Unbreakable = false;
		
		public override ModItem Clone(Item item)
		{
			MusicEmblem clone = (MusicEmblem)MemberwiseClone();
			clone.items = new List<Item>(items.Select(x => x.Clone()));
			return clone;
		}

		private List<Item> items = new List<Item>();
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Music Emblem");
			Tooltip.SetDefault("A Rare Accessory That Plays Music And Functions!");
		}
		
		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 24;
			item.accessory = true;
			if(UI.AccessoryUI.Meme != null && !UI.AccessoryUI.Meme.IsAir)
			{
				item.value = (UI.AccessoryUI.Meme.value * 2);
			}
		}
		
		internal void AddItem(Item item)
		{
			Player player = Main.LocalPlayer;
			if(UI.AccessoryUI.Meme != null && !UI.AccessoryUI.Meme.IsAir)
			{
				items.Add(UI.AccessoryUI.Meme);
			}
			if(UI.MusicBoxUI.Bean != null && !UI.MusicBoxUI.Bean.IsAir)
			{
				items.Add(UI.MusicBoxUI.Bean);
			}
			if(!player.GetModPlayer<Playerone>(mod).ShinyBought && !player.GetModPlayer<Playerone>(mod).GlitchBought && !player.GetModPlayer<Playerone>(mod).BeefyBought && !player.GetModPlayer<Playerone>(mod).SoullessBought && !player.GetModPlayer<Playerone>(mod).DebufflessBought)
			{
				int BonusPick = Main.rand.Next(25);
				if (BonusPick == 0)
				{
					Shiny = false;
					Glitch = false;
					Beefy = false;
					Soulless = false;
					Debuffless = false;
					Unbreakable = false;
					Main.PlaySound(SoundID.Unlock);
				}
				else if (BonusPick == 1)
				{
					Shiny = true;
					Glitch = false;
					Beefy = false;
					Soulless = false;
					Debuffless = false;
					Unbreakable = false;
					player.GetModPlayer<Playerone>(mod).ShinyBuyable = true;
					Main.PlaySound(SoundID.Unlock);
					Main.NewText("SHINY BONUS!!", Color.Coral);
				}
				else if (BonusPick == 2)
				{
					Shiny = false;
					Glitch = true;
					Beefy = false;
					Soulless = false;
					Debuffless = false;
					Unbreakable = false;
					player.GetModPlayer<Playerone>(mod).GlitchBuyable = true;
					Main.PlaySound(SoundID.Unlock);
					Main.NewText("GLITCH BONUS!!", Color.LimeGreen);
				}
				else if (BonusPick == 3)
				{
					Shiny = false;
					Glitch = false;
					Beefy = true;
					Soulless = false;
					Debuffless = false;
					Unbreakable = false;
					player.GetModPlayer<Playerone>(mod).BeefyBuyable = true;
					Main.PlaySound(SoundID.Unlock);
					Main.NewText("BEEFY BONUS!!", Color.Maroon);
				}
				else if (BonusPick == 4)
				{
					Shiny = false;
					Glitch = false;
					Beefy = false;
					Soulless = true;
					Debuffless = false;
					Unbreakable = false;
					player.GetModPlayer<Playerone>(mod).SoullessBuyable = true;
					Main.PlaySound(SoundID.Unlock);
					Main.NewText("SOULLESS BONUS!!", Color.IndianRed);
				}
				else if (BonusPick == 5)
				{
					Shiny = false;
					Glitch = false;
					Beefy = false;
					Soulless = false;
					Debuffless = true;
					Unbreakable = false;
					player.GetModPlayer<Playerone>(mod).DebufflessBuyable = true;
					Main.PlaySound(SoundID.Unlock);
					Main.NewText("DEBUFFLESS BONUS!!", Color.RoyalBlue);
				}
				else
				{
					Shiny = false;
					Glitch = false;
					Beefy = false;
					Soulless = false;
					Debuffless = false;
					Unbreakable = false;
					Main.PlaySound(SoundID.Unlock);
				}
			}
			if (player.GetModPlayer<Playerone>(mod).ShinyBought)
			{
				if(UI.AccessoryUI.Meme != null && !UI.AccessoryUI.Meme.IsAir && UI.MusicBoxUI.Bean != null && !UI.MusicBoxUI.Bean.IsAir)
				{
					if (!player.GetModPlayer<Playerone>(mod).UnbreakableUsed)
					{
						Shiny = true;
						Glitch = false;
						Beefy = false;
						Soulless = false;
						Debuffless = false;
						Unbreakable = false;
						Main.PlaySound(SoundID.Unlock);
						player.GetModPlayer<Playerone>(mod).ShinyBought = false;
					}
					else
					{
						Shiny = true;
						Glitch = false;
						Beefy = false;
						Soulless = false;
						Debuffless = false;
						Unbreakable = true;
						Main.PlaySound(SoundID.Unlock);
						player.GetModPlayer<Playerone>(mod).ShinyBought = false;
						player.GetModPlayer<Playerone>(mod).UnbreakableUsed = false;
					}
				}
			}
			if (player.GetModPlayer<Playerone>(mod).GlitchBought)
			{
				if(UI.AccessoryUI.Meme != null && !UI.AccessoryUI.Meme.IsAir && UI.MusicBoxUI.Bean != null && !UI.MusicBoxUI.Bean.IsAir)
				{
					if (!player.GetModPlayer<Playerone>(mod).UnbreakableUsed)
					{
						Shiny = false;
						Glitch = true;
						Beefy = false;
						Soulless = false;
						Debuffless = false;
						Unbreakable = false;
						Main.PlaySound(SoundID.Unlock);
						player.GetModPlayer<Playerone>(mod).GlitchBought = false;
					}
					else
					{
						Shiny = false;
						Glitch = true;
						Beefy = false;
						Soulless = false;
						Debuffless = false;
						Unbreakable = true;
						Main.PlaySound(SoundID.Unlock);
						player.GetModPlayer<Playerone>(mod).GlitchBought = false;
						player.GetModPlayer<Playerone>(mod).UnbreakableUsed = false;
					}
				}
			}
			if (player.GetModPlayer<Playerone>(mod).BeefyBought)
			{
				if(UI.AccessoryUI.Meme != null && !UI.AccessoryUI.Meme.IsAir && UI.MusicBoxUI.Bean != null && !UI.MusicBoxUI.Bean.IsAir)
				{
					if (!player.GetModPlayer<Playerone>(mod).UnbreakableUsed)
					{
						Shiny = false;
						Glitch = false;
						Beefy = true;
						Soulless = false;
						Debuffless = false;
						Unbreakable = false;
						Main.PlaySound(SoundID.Unlock);
						player.GetModPlayer<Playerone>(mod).BeefyBought = false;
					}
					else
					{
						Shiny = false;
						Glitch = false;
						Beefy = true;
						Soulless = false;
						Debuffless = false;
						Unbreakable = true;
						Main.PlaySound(SoundID.Unlock);
						player.GetModPlayer<Playerone>(mod).BeefyBought = false;
						player.GetModPlayer<Playerone>(mod).UnbreakableUsed = false;
					}
				}
			}
			if (player.GetModPlayer<Playerone>(mod).SoullessBought)
			{
				if(UI.AccessoryUI.Meme != null && !UI.AccessoryUI.Meme.IsAir && UI.MusicBoxUI.Bean != null && !UI.MusicBoxUI.Bean.IsAir)
				{
					if (!player.GetModPlayer<Playerone>(mod).UnbreakableUsed)
					{
						Shiny = false;
						Glitch = false;
						Beefy = false;
						Soulless = true;
						Debuffless = false;
						Unbreakable = false;
						Main.PlaySound(SoundID.Unlock);
						player.GetModPlayer<Playerone>(mod).SoullessBought = false;
					}
					else
					{
						Shiny = false;
						Glitch = false;
						Beefy = false;
						Soulless = true;
						Debuffless = false;
						Unbreakable = true;
						Main.PlaySound(SoundID.Unlock);
						player.GetModPlayer<Playerone>(mod).SoullessBought = false;
						player.GetModPlayer<Playerone>(mod).UnbreakableUsed = false;
					}
				}
			}
			if (player.GetModPlayer<Playerone>(mod).DebufflessBought)
			{
				if(UI.AccessoryUI.Meme != null && !UI.AccessoryUI.Meme.IsAir && UI.MusicBoxUI.Bean != null && !UI.MusicBoxUI.Bean.IsAir)
				{
					if (!player.GetModPlayer<Playerone>(mod).UnbreakableUsed)
					{
						Shiny = false;
						Glitch = false;
						Beefy = false;
						Soulless = false;
						Debuffless = true;
						Unbreakable = false;
						Main.PlaySound(SoundID.Unlock);
						player.GetModPlayer<Playerone>(mod).DebufflessBought = false;
					}
					else
					{
						Shiny = false;
						Glitch = false;
						Beefy = false;
						Soulless = false;
						Debuffless = true;
						Unbreakable = true;
						Main.PlaySound(SoundID.Unlock);
						player.GetModPlayer<Playerone>(mod).DebufflessBought = false;
						player.GetModPlayer<Playerone>(mod).UnbreakableUsed = false;
					}
				}
			}
		}
		
		#region deleteAfter0.10.1.6
        private static List<Item> reforgeBackupItems;
        public override bool NewPreReforge()
        {
            if(ModLoader.version <= new Version(0, 10, 1, 5))
                reforgeBackupItems = new List<Item>(items.Select(x => x.Clone()));
            return base.NewPreReforge();
        }
        public override void PostReforge()
        {
            if (ModLoader.version <= new Version(0, 10, 1, 5))
                items = new List<Item>(reforgeBackupItems.Select(x => x.Clone()));
            reforgeBackupItems = null;
        }
        #endregion
		
		public override bool CanRightClick()
		{
			return false;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			bool bob1 = true;
			bool bob2 = true;
			bool bob3 = true;
			foreach (Item item in items)
			{
				player.VanillaUpdateAccessory(player.whoAmI, item, hideVisual, ref bob1, ref bob2, ref bob3);
				player.VanillaUpdateEquip(item);
			}
			if (Shiny)
			{
				player.GetModPlayer<Playerone>(mod).Shiny = true;
			}
			else
			{
				player.GetModPlayer<Playerone>(mod).Shiny = false;
			}
			if (Glitch)
			{
				player.GetModPlayer<Playerone>(mod).Glitch = true;
			}
			else
			{
				player.GetModPlayer<Playerone>(mod).Glitch = false;
			}
			if (Beefy)
			{
				player.GetModPlayer<Playerone>(mod).Beefy = true;
			}
			else
			{
				player.GetModPlayer<Playerone>(mod).Beefy = false;
			}
			if (Soulless)
			{
				player.GetModPlayer<Playerone>(mod).Soulless = true;
			}
			else
			{
				player.GetModPlayer<Playerone>(mod).Soulless = false;
			}
			if (Debuffless)
			{
				player.GetModPlayer<Playerone>(mod).Debuffless = true;
			}
			else
			{
				player.GetModPlayer<Playerone>(mod).Debuffless = false;
			}
			if (Unbreakable)
			{
				player.GetModPlayer<Playerone>(mod).Unbreakable = true;
			}
			else
			{
				player.GetModPlayer<Playerone>(mod).Unbreakable = false;
			}
		}
		
		Color color1 = Main.DiscoColor;
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			foreach (TooltipLine line in tooltips)
			{
                if (line.Name == "ItemName" && line.mod == "Terraria")
                {
					line.overrideColor = color1;
				}
			}
			Color colora = Color.SlateBlue;
			Color colorb = Color.SeaGreen;
			float Eggroll = Math.Abs(Main.GameUpdateCount) / 5f;
			float Pie = 1f * (float) Math.Sin(Eggroll);
			Color color2 = Color.Lerp(colora, colorb, Pie);
			if (Shiny)
			{
				TooltipLine line2 = new TooltipLine(mod, "Bonus1", "SHINY BONUS!!");
				line2.overrideColor = color2;
				tooltips.Add(line2);
			}
			if (Glitch)
			{
				TooltipLine line2 = new TooltipLine(mod, "Bonus2", "GLITCH BONUS!!");
				line2.overrideColor = color2;
				tooltips.Add(line2);
			}
			if (Beefy)
			{
				TooltipLine line2 = new TooltipLine(mod, "Bonus3", "BEEFY BONUS!!");
				line2.overrideColor = color2;
				tooltips.Add(line2);
			}
			if (Soulless)
			{
				TooltipLine line2 = new TooltipLine(mod, "Bonus4", "SOULLESS BONUS!!");
				line2.overrideColor = color2;
				tooltips.Add(line2);
			}
			if (Debuffless)
			{
				TooltipLine line2 = new TooltipLine(mod, "Bonus5", "DEBUFFLESS BONUS!!");
				line2.overrideColor = color2;
				tooltips.Add(line2);
			}
			if (Unbreakable)
			{
				TooltipLine line3 = new TooltipLine(mod, "Bonus6", "UNBREAKABLE!!");
				line3.overrideColor = color2;
				tooltips.Add(line3);
			}
			Player player = Main.LocalPlayer;
			if (player.GetModPlayer<Playerone>(mod).Hero)
			{
				TooltipLine line4 = new TooltipLine(mod, "Congrats", "THE TRUE HERO!!");
				line4.overrideColor = color2;
				tooltips.Add(line4);
			}
		}
		public override TagCompound Save()
		{
			return new TagCompound 
			{
				{"items", items},
				{"Shiny", Shiny},
				{"Glitch", Glitch},
				{"Beefy", Beefy},
				{"Soulless", Soulless},
				{"Debuffless", Debuffless},
				{"Unbreakable", Unbreakable},
			};
		}

		public override void Load(TagCompound tag)
		{
			items = tag.Get<List<Item>>("items");
			Shiny = tag.GetBool("Shiny");
			Glitch = tag.GetBool("Glitch");
			Beefy = tag.GetBool("Beefy");
			Soulless = tag.GetBool("Soulless");
			Debuffless = tag.GetBool("Debuffless");
			Unbreakable = tag.GetBool("Unbreakable");
		}
	}
}