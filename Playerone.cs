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
using MusicMadness.Tiles;

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

        public List<Point> TouchedTiles;

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
        int squ1 = 180;
        int squ2 = 180;
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
            if (player.position.Y <= (250 * 16) && player.position.Y > (150 * 16))
            {
                player.KillMe(PlayerDeathReason.ByCustomReason(player.name + " Ran Out of Oxygen"), 99999999d, 0, false);
            }
            if (ZoneScorchedBone)
            {
                if (!player.HasBuff(mod.BuffType("Escapeless")))
                {
                    if (squ1 > 0)
                    {
                        squ1--;
                    }
                    if (squ1 == 0)
                    {
                        player.AddBuff(mod.BuffType("Escapeless"), 3600, false);
                        squ1 = 180;
                    }
                }
                if (!player.HasBuff(BuffID.NoBuilding))
                {
                    if (squ2 > 0)
                    {
                        squ2--;
                    }
                    if (squ2 == 0)
                    {
                        player.AddBuff(BuffID.NoBuilding, 3600, false);
                        squ2 = 180;
                    }
                }
                player.gravity = 1f;
            }
            int pR = (int)(player.Right.X / 16);
            int pL = (int)(player.Left.X / 16);
            int pT = (int)(player.Top.Y / 16);
            int pB = (int)(player.Bottom.Y / 16);
            for (int x = pL; x <= pR; x++)
            {
                for (int y = pT; y <= pB; y++)
                {
                    if (Main.tile[x, y].type == mod.TileType("SpikeyUp"))
                    {
                        player.KillMe(PlayerDeathReason.ByCustomReason(player.name + " Nailed It!"), 99999999d, 0, false);
                    }
                    if (Main.tile[x, y].type == mod.TileType("SpikeyUpBottom"))
                    {
                        player.KillMe(PlayerDeathReason.ByCustomReason(player.name + " Nailed It!"), 99999999d, 0, false);
                    }
                    if (Main.tile[x, y].type == mod.TileType("SpikeyDown"))
                    {
                        player.KillMe(PlayerDeathReason.ByCustomReason(player.name + " Nailed It!"), 99999999d, 0, false);
                    }
                    if (Main.tile[x, y].type == mod.TileType("SpikeyDownBottom"))
                    {
                        player.KillMe(PlayerDeathReason.ByCustomReason(player.name + " Nailed It!"), 99999999d, 0, false);
                    }
                }
            }
        }
		
		public Vector2 oldPosition = Vector2.Zero;

        public bool ZoneScorchedBone = false;

        public override void PostUpdateBuffs()
		{
			if (oldPosition != Vector2.Zero && (oldPosition.X <= player.position.X - 64f || oldPosition.X >= player.position.X + 64f || oldPosition.Y <= player.position.Y - 64f || oldPosition.Y >= player.position.Y + 64f) && player.HasBuff(mod.BuffType("Escapeless")))
			{
				player.KillMe(PlayerDeathReason.ByCustomReason(player.name + " tried to escape..."), 99999999d, 0, false);
			}
			oldPosition = player.position;
		}

        public override void UpdateBiomes()
        {
            ZoneScorchedBone = (MeWorld.ScorchedBoneBlocks > 200);
        }

        public override bool CustomBiomesMatch(Player other)
        {
            Playerone modOther = other.GetModPlayer<Playerone>(mod);
            return ZoneScorchedBone == modOther.ZoneScorchedBone;
        }

        public override void CopyCustomBiomesTo(Player other)
        {
            Playerone modOther = other.GetModPlayer<Playerone>(mod);
            modOther.ZoneScorchedBone = ZoneScorchedBone;
        }

        public override void SendCustomBiomes(BinaryWriter writer)
        {
            BitsByte flags = new BitsByte();
            flags[0] = ZoneScorchedBone;
            writer.Write(flags);
        }

        public override void ReceiveCustomBiomes(BinaryReader reader)
        {
            BitsByte flags = reader.ReadByte();
            ZoneScorchedBone = flags[0];
        }

        public override Texture2D GetMapBackgroundImage()
        {
            if (ZoneScorchedBone)
            {
                return mod.GetTexture("BackgroundDoom");
            }
            return null;
        }

        public static readonly PlayerLayer MiscEffectsBack = new PlayerLayer("MusicMadness", "MiscEffectsBack", PlayerLayer.MiscEffectsBack, delegate (PlayerDrawInfo drawInfo)
        {
            if (MeWorld.ScorchedBoneBlocks > 180)
            {
                Player drawPlayer = drawInfo.drawPlayer;
                Mod mod = ModLoader.GetMod("MusicMadness");
                Playerone modPlayer = drawPlayer.GetModPlayer<Playerone>(mod);
                Texture2D texture = mod.GetTexture("BackgroundDoomActual");
                int drawX = (int)(drawInfo.position.X + drawPlayer.width / 2f - Main.screenPosition.X);
                int drawY = (int)(drawInfo.position.Y + drawPlayer.height / 2f - Main.screenPosition.Y);
                DrawData data = new DrawData(texture, new Vector2(drawX, drawY), null, Color.White, 0f, new Vector2(texture.Width / 2f, texture.Height / 2f), 3f, SpriteEffects.None, 0);
                Main.playerDrawData.Add(data);
            }
        });

        public override void ModifyDrawLayers(List<PlayerLayer> layers)
        {
            MiscEffectsBack.visible = true;
            layers.Insert(0, MiscEffectsBack);
        }
    }
}