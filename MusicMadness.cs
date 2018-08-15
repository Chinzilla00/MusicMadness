using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Terraria.GameContent.UI;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using System.Runtime.CompilerServices;
using MusicMadness.UI;
using MusicMadness.NPCs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using Terraria.Utilities;

namespace MusicMadness
{
	public class MusicMadness : Mod
	{
		internal static MusicMadness Instance;
		
		internal UserInterface UserInterfacer;
		internal UserInterface UserInterfacerr;
		internal UserInterface UserInterfacerrr;
		
		internal MusicBoxUI MusicBoxUI;
		internal AccessoryUI AccessoryUI;
		internal BuyableUI BuyableUI;

		public MusicMadness()
		{
			Properties = new ModProperties
			{
				Autoload = true,
				AutoloadSounds = true
			};
		}

		public override void Load()
		{
			Instance = this;
			
			if (!Main.dedServ)
			{
				MusicBoxUI = new MusicBoxUI();
				AccessoryUI = new AccessoryUI();
				BuyableUI = new BuyableUI();
				UserInterfacer = new UserInterface();
				UserInterfacerr = new UserInterface();
				UserInterfacerrr = new UserInterface();
			}
		}

		public override void Unload()
		{
			Instance = null;
		}
		
		public override void UpdateUI(GameTime gameTime)
		{
			if (UserInterfacer != null && MusicBoxUI.visible)
			{
				UserInterfacer.Update(gameTime);
			}
			if (UserInterfacerr != null && AccessoryUI.visible)
			{
				UserInterfacerr.Update(gameTime);
			}
			if (UserInterfacerrr != null && BuyableUI.visible)
			{
				UserInterfacerrr.Update(gameTime);
			}
			if ((Main.LocalPlayer.talkNPC <= 0 || Main.npc[Main.LocalPlayer.talkNPC].type != NPCType("Musician")) && (UI.MusicBoxUI.visible && UI.AccessoryUI.visible))
			{
				Main.PlaySound(SoundID.MenuClose);
				UI.MusicBoxUI.visible = false;
				UI.AccessoryUI.visible = false;
				UI.BuyableUI.visible = false;
				UserInterfacer.SetState(null);
				UserInterfacerr.SetState(null);
				UserInterfacerrr.SetState(null);
			}
        }
		
		public override void UpdateMusic(ref int music, ref MusicPriority priority)
		{
			if (Main.projectile.Any(x => x.active && x.type == ProjectileType("RisingBoneLava")))
			{
				music = GetSoundSlot(SoundType.Music, "Sounds/Music/UnusualCause");
				priority = MusicPriority.BossLow;
			}
		}

		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
		{
			int MouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
			if (MouseTextIndex != -1)
			{
				layers.Insert(MouseTextIndex, new LegacyGameInterfaceLayer("MusicMadness: Combine an Accessory with a Music Box!", delegate { if (MusicBoxUI.visible) { MusicBoxUI.Draw(Main.spriteBatch); } if (AccessoryUI.visible) { AccessoryUI.Draw(Main.spriteBatch); } if (BuyableUI.visible) { BuyableUI.Draw(Main.spriteBatch); } return true; }, InterfaceScaleType.UI));
			}
		}
	}
}