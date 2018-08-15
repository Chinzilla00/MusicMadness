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
using MusicMadness.Items;

namespace MusicMadness
{
	public class Playerone : ModPlayer
	{
		public bool Shiny = false;
		public bool Glitch = false;
		public bool Beefy = false;
		public bool ShinyBuyable = false;
		public bool GlitchBuyable = false;
		public bool BeefyBuyable = false;
		public bool ShinyBought = false;
		public bool GlitchBought = false;
		public bool BeefyBought = false;
		public bool Soulless = false;
		public bool Debuffless = false;
		public bool SoullessBuyable = false;
		public bool DebufflessBuyable = false;
		public bool SoullessBought = false;
		public bool DebufflessBought = false;
		public bool Unbreakable = false;
		public bool UnbreakableUsed = false;
		public bool Hero = false;
		public bool EquippedYourDoom = false;
		public bool Escapeless = false;
		public bool TeleportingAllow = false;
		
		public override TagCompound Save()
		{
			return new TagCompound 
			{
				{"ShinyBuyable", ShinyBuyable},
				{"GlitchBuyable", GlitchBuyable},
				{"BeefyBuyable", BeefyBuyable},
				{"SoullessBuyable", SoullessBuyable},
				{"DebufflessBuyable", DebufflessBuyable},
				{"Hero", Hero},
			};
		}

		public override void Load(TagCompound tag)
		{
			ShinyBuyable = tag.GetBool("ShinyBuyable");
			GlitchBuyable = tag.GetBool("GlitchBuyable");
			BeefyBuyable = tag.GetBool("BeefyBuyable");
			SoullessBuyable = tag.GetBool("SoullessBuyable");
			DebufflessBuyable = tag.GetBool("DebufflessBuyable");
			Hero = tag.GetBool("Hero");
		}
		
		public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
		{
			if (player.difficulty != 2)
			{
				int maxAccessoryIndex = 5 + player.extraAccessorySlots;
				for (int i = 0; i < 0 + maxAccessoryIndex; i++)
				{
					if (player.armor[i].type == mod.ItemType<MusicEmblem>() && Shiny)
					{
						if (Main.rand.NextBool(5))
						{
							int bum = 0;
							for (int j = 0; j < 59; j++)
							{
								if (player.inventory[j].type >= 71 && player.inventory[j].type <= 74)
								{
									int bum2 = Item.NewItem((int)player.position.X, (int)player.position.Y, player.width, player.height, player.inventory[j].type, 1, false, 0, false, false);
									int bum3 = player.inventory[j].stack / 4;
									if (Main.expertMode)
									{
										bum3 = (int)((double)player.inventory[j].stack * 0.30);
									}
									if (player.inventory[j].type == 71)
									{
										bum += bum3;
									}
									if (player.inventory[j].type == 72)
									{
										bum += bum3 * 100;
									}
									if (player.inventory[j].type == 73)
									{
										bum += bum3 * 10000;
									}
									if (player.inventory[j].type == 74)
									{
										bum += bum3 * 1000000;
									}
									if (player.inventory[j].stack <= 0)
									{
										player.inventory[j] = new Item();
									}
									Main.item[bum2].stack = bum3;
									Main.item[bum2].velocity.Y = (float)Main.rand.Next(-20, 1) * 0.2f;
									Main.item[bum2].velocity.X = (float)Main.rand.Next(-20, 21) * 0.2f;
									Main.item[bum2].noGrabDelay = 100;
									if (j == 58)
									{
									Main.mouseItem = player.inventory[j].Clone();
									}
								}
							}
							Main.NewText("BONUS ACTIVATED!!", Color.HotPink);
							if (Main.rand.NextBool(10) && !Unbreakable)
							{
								MusicEmblem emblem = (MusicEmblem)player.armor[i].modItem;
								emblem.Shiny = false;
								Shiny = false;
								Main.NewText("Ability Lost!", Color.HotPink);
							}
						}	
					}
				}
			}
			return true;
		}
		
		public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
		{
			int maxAccessoryIndex = 5 + player.extraAccessorySlots;
			for (int i = 0; i < 0 + maxAccessoryIndex; i++)
			{
				if (player.armor[i].type == mod.ItemType<MusicEmblem>() && Beefy)
				{
					if (Main.rand.NextBool(20))
					{
						damage = (damage / 2);
						Main.NewText("BONUS ACTIVATED!!", Color.HotPink);
						if (Main.rand.NextBool(10) && !Unbreakable)
						{
							MusicEmblem emblem = (MusicEmblem)player.armor[i].modItem;
							emblem.Beefy = false;
							Beefy = false;
							Main.NewText("Ability Lost!", Color.HotPink);
						}
					}
				}
			}
			return true;
		}
		
		public override bool? CanHitNPC (Item item, NPC target)
		{
			if (EquippedYourDoom)
			{
				if (target.type == mod.NPCType("Musician"))
				{
					return true;
				}
			}
			return null;
		}
		
		public override bool? CanHitNPCWithProj (Projectile proj, NPC target)
		{
			if (EquippedYourDoom)
			{
				if (target.type == mod.NPCType("Musician"))
				{
					return true;
				}
			}
			return null;
		}

		int boostType = -1;
		int timer = 3600;
		public override void PostUpdate()
		{
			int maxAccessoryIndex = 5 + player.extraAccessorySlots;
			for (int i = 0; i < 0 + maxAccessoryIndex; i++)
			{
				if (player.armor[i].type == mod.ItemType<MusicEmblem>() && Glitch)
				{
					if (Main.rand.NextBool(7200))
					{
						boostType = Main.rand.Next(5);
						timer = 3600;
						Main.NewText("BONUS ACTIVATED!!", Color.HotPink);
						if (Main.rand.NextBool(5) && !Unbreakable)
						{
							MusicEmblem emblem = (MusicEmblem)player.armor[i].modItem;
							emblem.Glitch = false;
							Glitch = false;
							Main.NewText("Ability Lost!", Color.HotPink);
						}
					}
					if (timer >= 0)
					{
						timer--;
					}
					if (boostType == 0)
					{
						player.meleeDamage = player.meleeDamage * 1.5f;
					}
					if (boostType == 1)
					{
						player.rangedDamage = player.rangedDamage * 1.5f;
					}
					if (boostType == 2)
					{
						player.magicDamage = player.magicDamage * 1.5f;
					}
					if (boostType == 3)
					{
						player.thrownDamage = player.thrownDamage * 1.5f;
					}
					if (boostType == 4)
					{
						player.minionDamage = player.minionDamage * 1.5f;
					}
					if (timer == 0)
					{
						boostType = -1;
					}
				}
				if (player.armor[i].type == mod.ItemType<MusicEmblem>() && Debuffless)
				{
					if (Main.rand.NextBool(7200))
					{
						int buff = Main.rand.Next(22);
						if (Main.debuff[player.buffType[buff]])
						{
							player.buffTime[buff] = 0;
							Main.NewText("BONUS ACTIVATED!!", Color.HotPink);
							if (Main.rand.NextBool(10) && !Unbreakable)
							{
								MusicEmblem emblem = (MusicEmblem)player.armor[i].modItem;
								emblem.Debuffless = false;
								Debuffless = false;
								Main.NewText("Ability Lost!", Color.HotPink);
							}
						}
					}
				}
			}
		}
		
		public Vector2 oldPosition = Vector2.Zero;
		
		public override void PostUpdateBuffs()
		{
			if (oldPosition != Vector2.Zero && (oldPosition.X <= player.position.X - 64f || oldPosition.X >= player.position.X + 64f || oldPosition.Y <= player.position.Y - 64f || oldPosition.Y >= player.position.Y + 64f) && player.HasBuff(mod.BuffType("Escapeless")))
			{
				player.KillMe(PlayerDeathReason.ByCustomReason(player.name + " tried to escape..."), 99999999d, 0, false);
			}
			oldPosition = player.position;
		}
	}
}