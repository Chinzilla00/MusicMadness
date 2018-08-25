using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;
using Terraria.Localization;
using Terraria.DataStructures;
using Terraria.ModLoader.IO;
using Terraria.GameInput;
using Terraria;
using System.Collections;
 
namespace MusicMadness.NPCs
{
	[AutoloadHead]
    public class Musician : ModNPC
    {
		public string coconut1 = "Do you hear that sound?";
		public string coconut2 = "You should try my other mod FredTheZombie!";
		public override bool Autoload(ref string name)
		{
			name = "Musician";
			return mod.Properties.Autoload;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Musician");
			Main.npcFrameCount[npc.type] = 26;
			NPCID.Sets.ExtraFramesCount[npc.type] = 10;
			NPCID.Sets.AttackFrameCount[npc.type] = 5;
			NPCID.Sets.DangerDetectRange[npc.type] = 700;
			NPCID.Sets.AttackType[npc.type] = 3;
			NPCID.Sets.AttackTime[npc.type] = 20;
			NPCID.Sets.AttackAverageChance[npc.type] = 10;
		}
		
        public override void SetDefaults()
        {
            npc.townNPC = true;
            npc.friendly = true;
            npc.width = 18;
            npc.height = 40;
            npc.aiStyle = 7;
            npc.damage = 10;
            npc.defense = 10;
            npc.lifeMax = 250;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.knockBackResist = 0.5f;      
            animationType = NPCID.Guide;
        }

		public override bool CanTownNPCSpawn(int numTownNPCs, int money)
		{
			return Main.player.Any(player => !player.dead) && Main.hardMode && NPC.downedPlantBoss;
		}
		
		public override string TownNPCName()
		{
			WeightedRandom<string> names = new WeightedRandom<string>();
			{
				names.Add("Freddie", 0.5);
				names.Add("Elvis", 0.5);
				names.Add("Jimi", 0.75);
				names.Add("Mick", 0.75);
				names.Add("John");
				names.Add("David");
				names.Add("Kurt", 1.25);
				names.Add("Bruce", 1.25);
				names.Add("Paul", 1.5);
				names.Add("Johnny", 1.5);
			}
			return names;
		}
		
		public override void NPCLoot()
		{
			if (npc.lastInteraction != 255)
			{
				Player player = Main.player[npc.lastInteraction];
				if (player.GetModPlayer<Playerone>().EquippedYourDoom && player.ZoneUnderworldHeight)
				{
					if (Main.netMode == 0)
					{
						Main.NewText("ARE YOU ASKING TO DIE?", Color.DarkRed);
					}
					else
					{
						NetMessage.BroadcastChatMessage(NetworkText.FromLiteral("ARE YOU ASKING TO DIE?"), Color.DarkRed);
					}
					for (int i = 0; i < 255; i++)
					{
						Player player2 = Main.player[i];
						if (!player2.dead && player2.active)
						{
							if (!player2.ZoneUnderworldHeight)
							{
								player2.KillMe(PlayerDeathReason.ByCustomReason(player2.name + " Was not Prepared to Challenge Me..."), 99999999d, 0, false);
							}
							else
							{
								if (Main.netMode == 0)
								{
									Main.NewText("Phase One Has Started...", Color.DarkRed);
									Main.PlaySound(SoundID.Roar, player2.position, 0);
									Mod mod = ModLoader.GetMod("MusicMadness");
									player2.AddBuff(mod.BuffType("Escapeless"), 3600, true);
									int pork = mod.ProjectileType("RisingBoneLava");
									int pork2 = mod.ProjectileType("spikes");
									Projectile proj = Main.projectile[Projectile.NewProjectile((float)(player2.Center.X - 984), (float)(player2.Bottom.Y + 850), 0f, 0f, pork, 0, 0f, player2.whoAmI)];
									Projectile proj2 = Main.projectile[Projectile.NewProjectile((float)(player2.Center.X - 984), (float)(proj.Top.Y - 500), 0f, 0f, pork2, 0, 0f, player2.whoAmI)];
									proj.ai[0] = player2.whoAmI;
									proj2.ai[1] = player2.whoAmI;
								}
								else
								{
									NetMessage.BroadcastChatMessage(NetworkText.FromLiteral("Phase One Has Started..."), Color.DarkRed);
									Main.PlaySound(SoundID.Roar, player2.position, 0);
									Mod mod = ModLoader.GetMod("MusicMadness");
									player2.AddBuff(mod.BuffType("Escapeless"), 3600, true);
									int pork = mod.ProjectileType("RisingBoneLava");
									int pork2 = mod.ProjectileType("spikes");
									Projectile proj = Main.projectile[Projectile.NewProjectile((float)(player2.Center.X - 984), (float)(player2.Bottom.Y + 850), 0f, 0f, pork, 0, 0f, player2.whoAmI)];
									Projectile proj2 = Main.projectile[Projectile.NewProjectile((float)(player2.Center.X - 984), (float)(proj.Top.Y - 500), 0f, 0f, pork2, 0, 0f, player2.whoAmI)];
									proj.ai[0] = player2.whoAmI;
									proj2.ai[1] = player2.whoAmI;
								}
							}
						}
					}
				}
			}
		}
		
        public override string GetChat()
		{
			bool hasMyMod = ModLoader.GetMod("FredTheZombie") != null;
			switch (WorldGen.genRand.Next(5))
			{
				case 0:
					if (hasMyMod)
					{
						coconut1 = "My only fear is the fact that Fred The Zombie is the only Town NPC with Coding Knowledge...";
					}
					return coconut1;
				case 1:
					if (hasMyMod)
					{
						coconut2 = "I can't listen to my Music with Fred the Zombie Screaming about Jelly Beans 24/7";
					}
					return coconut2;
				case 2:
					return "Music is love, Music is life!";
				case 3:
					return "*Ba Dum Tss*";
				default:
					return "Practice Makes Perfect!";
			}
		}
		
		public override void SetChatButtons(ref string button, ref string button2)
		{
			button = "Enhance";
		}

        public override void OnChatButtonClicked(bool firstButton, ref bool shop)
        {
			if (firstButton)
			{
				if (!Main.playerInventory)
				{
					Main.playerInventory = true;
				}
				Main.PlaySound(SoundID.MenuOpen);
				UI.BuyableUI.visible = true;
				MusicMadness.Instance.UserInterfacerrr.SetState(MusicMadness.Instance.BuyableUI);
				UI.MusicBoxUI.visible = true;
				UI.AccessoryUI.visible = true;
				MusicMadness.Instance.UserInterfacer.SetState(MusicMadness.Instance.MusicBoxUI);
				MusicMadness.Instance.UserInterfacerr.SetState(MusicMadness.Instance.AccessoryUI);
			}
		}
		public override void DrawTownAttackSwing(ref Texture2D item, ref int itemSize, ref float scale, ref Vector2 offset)
        {
            scale = 1f;
            item = Main.itemTexture[1305];
            itemSize = 56;
        }

        public override void TownNPCAttackSwing(ref int itemWidth, ref int itemHeight)
        {
            itemWidth = 56;
            itemHeight = 56;
        }
	}
}