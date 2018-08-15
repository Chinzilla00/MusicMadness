using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.UI.Chat;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using MusicMadness.UI;
using Terraria.Utilities;

namespace MusicMadness.UI
{
	public class AccessoryUI : UIState
	{
		public UIPanel AccessoryUIPanel;
		public static bool visible = false;
		public static Item Meme;
		public static bool BobRoss = false;
		
		public override void OnInitialize()
		{
			Item Meme = new Item();
			VanillaItemSlotWrapperONE AccessoryUIPanel = new VanillaItemSlotWrapperONE(Terraria.UI.ItemSlot.Context.BankItem, 1f);
			AccessoryUIPanel.SetPadding(0);
			AccessoryUIPanel.Left.Set(20f, 0f);
			AccessoryUIPanel.Top.Set(300f, 0f);
			AccessoryUIPanel.Width.Set(50f, 0f);
			AccessoryUIPanel.Height.Set(50f, 0f);
			base.Append(AccessoryUIPanel);
		}
		public override void OnDeactivate()
		{
			if (!VanillaItemSlotWrapperONE.item.IsAir)
			{
				Main.LocalPlayer.QuickSpawnClonedItem(VanillaItemSlotWrapperONE.item, VanillaItemSlotWrapperONE.item.stack);
				VanillaItemSlotWrapperONE.item.TurnToAir();
			}
		}
		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			Mod mod = ModLoader.GetMod("MusicMadness");
			if(!VanillaItemSlotWrapperONE.item.IsAir && !VanillaItemSlotWrapperTWO.item.IsAir && VanillaItemSlotWrapperONE.item.type != mod.ItemType("MusicEmblem"))
			{
				int slotX = 200;
				int slotY = 300;
				string costText = Lang.inter[46].Value + ": ";
				string coinsText = "";
				int[] coins = Utils.CoinsSplit(VanillaItemSlotWrapperONE.item.value * 5);
				if (coins[3] > 0)
				{
					coinsText = coinsText + "[c/" + Colors.AlphaDarken(Colors.CoinPlatinum).Hex3() + ":" + coins[3] + " " + Lang.inter[15].Value + "] ";
				}
				if (coins[2] > 0)
				{
					coinsText = coinsText + "[c/" + Colors.AlphaDarken(Colors.CoinGold).Hex3() + ":" + coins[2] + " " + Lang.inter[16].Value + "] ";
				}
				if (coins[1] > 0)
				{
					coinsText = coinsText + "[c/" + Colors.AlphaDarken(Colors.CoinSilver).Hex3() + ":" + coins[1] + " " + Lang.inter[17].Value + "] ";
				}
				if (coins[0] > 0)
				{
					coinsText = coinsText + "[c/" + Colors.AlphaDarken(Colors.CoinCopper).Hex3() + ":" + coins[0] + " " + Lang.inter[18].Value + "] ";
				}
				int reforgeX = slotX + 70;
				int reforgeY = slotY + 40;
				ItemSlot.DrawSavings(Main.spriteBatch, slotX + 130, Main.instance.invBottom, true);
				ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, costText, new Vector2((slotX + 50), slotY), new Color(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor), 0f, Vector2.Zero, Vector2.One, -1f, 2f);
				ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, coinsText, new Vector2((slotX + 50) + Main.fontMouseText.MeasureString(costText).X, (float)slotY), Color.Firebrick, 0f, Vector2.Zero, Vector2.One, -1f, 2f);
				bool hoveringOverReforgeButton = Main.mouseX > reforgeX - 15 && Main.mouseX < reforgeX + 15 && Main.mouseY > reforgeY - 15 && Main.mouseY < reforgeY + 15 && !PlayerInput.IgnoreMouseInterface;
				Texture2D reforgeTexture = Main.reforgeTexture[hoveringOverReforgeButton ? 1 : 0];
				Main.spriteBatch.Draw(reforgeTexture, new Vector2(reforgeX, reforgeY), null, Color.Firebrick, 0f, reforgeTexture.Size() / 2f, 0.8f, SpriteEffects.None, 0f);
				if (hoveringOverReforgeButton)
				{
					Main.hoverItemName = "Enhance!";
					Main.LocalPlayer.mouseInterface = true;
					if (Main.mouseLeftRelease && Main.mouseLeft && Main.LocalPlayer.CanBuyItem(VanillaItemSlotWrapperONE.item.value * 5, -1) && VanillaItemSlotWrapperONE.item.value != 0)
					{
						Main.LocalPlayer.BuyItem(VanillaItemSlotWrapperONE.item.value * 5, -1);
						Meme = VanillaItemSlotWrapperONE.item.Clone();
						VanillaItemSlotWrapperONE.item.TurnToAir();
						UI.MusicBoxUI.Bean = VanillaItemSlotWrapperTWO.item.Clone();
						VanillaItemSlotWrapperTWO.item.TurnToAir();
						Item item = new Item();
						item.SetDefaults(mod.ItemType("MusicEmblem"));
						(item.modItem as Items.MusicEmblem).AddItem(null);
						Main.LocalPlayer.QuickSpawnClonedItem(item, 1);
					}
				}
			}
			
			CalculatedStyle innerDimensions = base.GetInnerDimensions();
			Vector2 drawPos = new Vector2(innerDimensions.X + 5f, innerDimensions.Y + 30f);

			float BoxX = innerDimensions.X;
			float BoxY = innerDimensions.Y;
			
			Utils.DrawBorderStringFourWay(spriteBatch, Main.fontItemStack, "Accessory / Music Emblem", BoxX + (float)(20 * 4), BoxY + 308f, Color.White, Color.Black, new Vector2(0.3f), 0.75f);
		}
	}
	public class VanillaItemSlotWrapperONE : UIElement
	{	
		int context;
		float scale;
		public static Item item = new Item();
		public VanillaItemSlotWrapperONE(int context = Terraria.UI.ItemSlot.Context.BankItem, float scale = 1f)
		{
			this.context = context;
			VanillaItemSlotWrapperONE.item.Prefix(0);
			this.scale = scale;
			
			this.Width.Set(Main.inventoryBack9Texture.Width * scale, 0f);
			this.Height.Set(Main.inventoryBack9Texture.Height * scale, 0f);
		}
		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			Mod mod = ModLoader.GetMod("MusicMadness");
			bool Moosic = false;
			int[] vanillaMusicBoxes = new int[] { 1610, 1963, 1964, 1965, 2742, 3044, 3235, 3236, 3237, 3370, 3371, 3796, 3869, 562, 563, 564, 565, 566, 567, 568, 569, 570, 571, 572, 573, 574, 1596, 1597, 1598, 1599, 1600, 1601, 1602, 1603, 1604, 1605, 1606, 1607, 1608, 1609 };
			if(vanillaMusicBoxes.Contains(Main.mouseItem.type) || UI.MusicBoxUI.itemToMusicReference.ContainsKey(Main.mouseItem.type))
			{
				Moosic = true;
			}
			float oldScale = Main.inventoryScale;
			Main.inventoryScale = scale;
			Rectangle rectangle = base.GetDimensions().ToRectangle();
			bool canSwap = Main.mouseItem.IsAir || (Main.mouseItem.accessory && !Moosic);
			if (ContainsPoint(Main.MouseScreen) && !PlayerInput.IgnoreMouseInterface && (canSwap || Main.mouseItem.type == mod.ItemType("MusicEmblem")))
			{
				Main.LocalPlayer.mouseInterface = true;
				Terraria.UI.ItemSlot.Handle(ref item, context);
			}
			Terraria.UI.ItemSlot.Draw(spriteBatch, ref item, context, rectangle.TopLeft());
			Main.inventoryScale = oldScale;
		}
	}
	public class MusicBoxUI : UIState
	{
		public UIPanel MusicBoxUIPanel;
		public static bool visible = false;
		public static Dictionary<int, int> itemToMusicReference;
		public static Item Bean;

		public override void OnInitialize()
		{	
			Item Bean = new Item();
			VanillaItemSlotWrapperTWO MusicBoxUIPanel = new VanillaItemSlotWrapperTWO(Terraria.UI.ItemSlot.Context.BankItem, 1f);
			MusicBoxUIPanel.SetPadding(0);
			MusicBoxUIPanel.Left.Set(20f, 0f);
			MusicBoxUIPanel.Top.Set(380f, 0f);
			MusicBoxUIPanel.Width.Set(50f, 0f);
			MusicBoxUIPanel.Height.Set(50f, 0f);
			base.Append(MusicBoxUIPanel);
			FieldInfo itemToMusicField = typeof(SoundLoader).GetField("itemToMusic", BindingFlags.Static | BindingFlags.NonPublic);
			itemToMusicReference = (Dictionary<int, int>)itemToMusicField.GetValue(null);
		}
		public override void OnDeactivate()
		{
			if (!VanillaItemSlotWrapperTWO.item.IsAir)
			{
				Main.LocalPlayer.QuickSpawnClonedItem(VanillaItemSlotWrapperTWO.item, VanillaItemSlotWrapperTWO.item.stack);
				VanillaItemSlotWrapperTWO.item.TurnToAir();
			}
		}
		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			CalculatedStyle innerDimensions = base.GetInnerDimensions();
			Vector2 drawPos = new Vector2(innerDimensions.X + 5f, innerDimensions.Y + 30f);

			float BoxX = innerDimensions.X;
			float BoxY = innerDimensions.Y;
			
			Utils.DrawBorderStringFourWay(spriteBatch, Main.fontItemStack, "Music Box / Broken Music Emblem", BoxX + (float)(20 * 4), BoxY + 388f, Color.White, Color.Black, new Vector2(0.3f), 0.75f);
		}
	}
	public class VanillaItemSlotWrapperTWO : UIElement
	{
		int context;
		float scale;
		public static Item item = new Item();
		public VanillaItemSlotWrapperTWO(int context = Terraria.UI.ItemSlot.Context.BankItem, float scale = 1f)
		{
			this.context = context;
			VanillaItemSlotWrapperTWO.item.Prefix(0);
			this.scale = scale;

			this.Width.Set(Main.inventoryBack9Texture.Width * scale, 0f);
			this.Height.Set(Main.inventoryBack9Texture.Height * scale, 0f);
		}
		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			Mod mod = ModLoader.GetMod("MusicMadness");
			bool Moosic = false;
			int[] vanillaMusicBoxes = new int[] { 1610, 1963, 1964, 1965, 2742, 3044, 3235, 3236, 3237, 3370, 3371, 3796, 3869, 562, 563, 564, 565, 566, 567, 568, 569, 570, 571, 572, 573, 574, 1596, 1597, 1598, 1599, 1600, 1601, 1602, 1603, 1604, 1605, 1606, 1607, 1608, 1609 };
			if(vanillaMusicBoxes.Contains(Main.mouseItem.type) || UI.MusicBoxUI.itemToMusicReference.ContainsKey(Main.mouseItem.type))
			{
				Moosic = true;
			}
			float oldScale = Main.inventoryScale;
			Main.inventoryScale = scale;
			Rectangle rectangle = base.GetDimensions().ToRectangle();
			bool canSwap = Main.mouseItem.IsAir || ((Moosic && Main.mouseItem.type != mod.ItemType("MusicEmblem")) || Main.mouseItem.type == mod.ItemType("BrokenMusicEmblem"));
			if (ContainsPoint(Main.MouseScreen) && !PlayerInput.IgnoreMouseInterface && canSwap)
			{
				Main.LocalPlayer.mouseInterface = true;
				Terraria.UI.ItemSlot.Handle(ref item, context);
			}
			Terraria.UI.ItemSlot.Draw(spriteBatch, ref item, context, rectangle.TopLeft());
			Main.inventoryScale = oldScale;
		}
	}
	public class BuyableUI : UIState
	{
		public UIPanel BuyableUIPanel;
		public static bool visible = false;
		public static Item Embee;
		public Texture2D[] Meblob;
		
		public override void OnInitialize()
		{
			Item Embee = new Item();
			Meblob = new Texture2D[] {ModLoader.GetTexture("MusicMadness/UI/Meblob1"), ModLoader.GetTexture("MusicMadness/UI/Meblob2"), ModLoader.GetTexture("MusicMadness/UI/Meblob3"), ModLoader.GetTexture("MusicMadness/UI/Meblob4"), ModLoader.GetTexture("MusicMadness/UI/Meblob5"), ModLoader.GetTexture("MusicMadness/UI/Meblob6"), ModLoader.GetTexture("MusicMadness/UI/Meblob7"), ModLoader.GetTexture("MusicMadness/UI/Meblob8"), ModLoader.GetTexture("MusicMadness/UI/Meblob9"), ModLoader.GetTexture("MusicMadness/UI/Meblob10")};
			BuyableUIPanel = new UIPanel();
			BuyableUIPanel.SetPadding(0);
			BuyableUIPanel.Left.Set(300f, 0f);
			BuyableUIPanel.Top.Set(340f, 0f);
			BuyableUIPanel.Width.Set(220f, 0f);
			BuyableUIPanel.Height.Set(140f, 0f);
			base.Append(BuyableUIPanel);
		}
		
		protected override void DrawChildren(SpriteBatch spriteBatch)
		{
			base.DrawChildren(spriteBatch);
			Mod mod = ModLoader.GetMod("MusicMadness");
			if (!VanillaItemSlotWrapperONE.item.IsAir && (VanillaItemSlotWrapperTWO.item.IsAir || VanillaItemSlotWrapperTWO.item.type == mod.ItemType("BrokenMusicEmblem")) && VanillaItemSlotWrapperONE.item.type == mod.ItemType("MusicEmblem"))
			{
				Player player = Main.LocalPlayer;
				if(player.GetModPlayer<Playerone>(mod).ShinyBuyable && VanillaItemSlotWrapperTWO.item.IsAir)
				{
					int slotX = 324;
					int slotY = 352;
					string costText = Lang.inter[46].Value + ": ";
					string coinsText = "";
					int[] coins = Utils.CoinsSplit(VanillaItemSlotWrapperONE.item.value * 5);
					if (coins[3] > 0)
					{
						coinsText = coinsText + "[c/" + Colors.AlphaDarken(Colors.CoinPlatinum).Hex3() + ":" + coins[3] + " " + Lang.inter[15].Value + "] ";
					}
					if (coins[2] > 0)
					{
						coinsText = coinsText + "[c/" + Colors.AlphaDarken(Colors.CoinGold).Hex3() + ":" + coins[2] + " " + Lang.inter[16].Value + "] ";
					}
					if (coins[1] > 0)
					{
						coinsText = coinsText + "[c/" + Colors.AlphaDarken(Colors.CoinSilver).Hex3() + ":" + coins[1] + " " + Lang.inter[17].Value + "] ";
					}
					if (coins[0] > 0)
					{
						coinsText = coinsText + "[c/" + Colors.AlphaDarken(Colors.CoinCopper).Hex3() + ":" + coins[0] + " " + Lang.inter[18].Value + "] ";
					}
					ItemSlot.DrawSavings(Main.spriteBatch, slotX + 130, Main.instance.invBottom, true);
					ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, costText, new Vector2((slotX + 50), slotY), new Color(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor), 0f, Vector2.Zero, Vector2.One, -1f, 2f);
					ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, coinsText, new Vector2((slotX + 50) + Main.fontMouseText.MeasureString(costText).X, (float)slotY), Color.Coral, 0f, Vector2.Zero, Vector2.One, -1f, 2f);
					int homeX = slotX - 12;
					int homeY = slotY;
					bool hoveringOverHomeButton = Main.mouseX > homeX - 12 && Main.mouseX < homeX + 12 && Main.mouseY > homeY - 12 && Main.mouseY < homeY + 12 && !PlayerInput.IgnoreMouseInterface;
					Texture2D homeTexture = Meblob[hoveringOverHomeButton ? 5 : 0];
					Main.spriteBatch.Draw(homeTexture, new Vector2(homeX, homeY), null, Color.White, 0f, homeTexture.Size() / 2f, 0.8f, SpriteEffects.None, 0f);
					if (hoveringOverHomeButton)
					{
						Main.hoverItemName = "Buy Shiny?";
						Main.LocalPlayer.mouseInterface = true;
						if (Main.mouseLeftRelease && Main.mouseLeft && Main.LocalPlayer.CanBuyItem(VanillaItemSlotWrapperONE.item.value * 5, -1) && VanillaItemSlotWrapperONE.item.value != 0)
						{
							Main.LocalPlayer.BuyItem(VanillaItemSlotWrapperONE.item.value * 5, -1);
							Embee = VanillaItemSlotWrapperONE.item.Clone();
							VanillaItemSlotWrapperONE.item.TurnToAir();
							player.GetModPlayer<Playerone>(mod).ShinyBought = true;
							player.GetModPlayer<Playerone>(mod).UnbreakableUsed = false;
							Embee.SetDefaults(mod.ItemType("MusicEmblem"));
							(Embee.modItem as Items.MusicEmblem).AddItem(null);
							Main.LocalPlayer.QuickSpawnClonedItem(Embee, 1);
						}
					}
				}
				else if(player.GetModPlayer<Playerone>(mod).ShinyBuyable && !VanillaItemSlotWrapperTWO.item.IsAir)
				{
					int slotX = 324;
					int slotY = 352;
					string costText = Lang.inter[46].Value + ": ";
					string coinsText = "";
					int[] coins = Utils.CoinsSplit(VanillaItemSlotWrapperONE.item.value * 5);
					if (coins[3] > 0)
					{
						coinsText = coinsText + "[c/" + Colors.AlphaDarken(Colors.CoinPlatinum).Hex3() + ":" + coins[3] + " " + Lang.inter[15].Value + "] ";
					}
					if (coins[2] > 0)
					{
						coinsText = coinsText + "[c/" + Colors.AlphaDarken(Colors.CoinGold).Hex3() + ":" + coins[2] + " " + Lang.inter[16].Value + "] ";
					}
					if (coins[1] > 0)
					{
						coinsText = coinsText + "[c/" + Colors.AlphaDarken(Colors.CoinSilver).Hex3() + ":" + coins[1] + " " + Lang.inter[17].Value + "] ";
					}
					if (coins[0] > 0)
					{
						coinsText = coinsText + "[c/" + Colors.AlphaDarken(Colors.CoinCopper).Hex3() + ":" + coins[0] + " " + Lang.inter[18].Value + "] ";
					}
					ItemSlot.DrawSavings(Main.spriteBatch, slotX + 130, Main.instance.invBottom, true);
					ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, costText, new Vector2((slotX + 50), slotY), new Color(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor), 0f, Vector2.Zero, Vector2.One, -1f, 2f);
					ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, coinsText, new Vector2((slotX + 50) + Main.fontMouseText.MeasureString(costText).X, (float)slotY), Color.Coral, 0f, Vector2.Zero, Vector2.One, -1f, 2f);
					int homeX = slotX - 12;
					int homeY = slotY;
					bool hoveringOverHomeButton = Main.mouseX > homeX - 12 && Main.mouseX < homeX + 12 && Main.mouseY > homeY - 12 && Main.mouseY < homeY + 12 && !PlayerInput.IgnoreMouseInterface;
					Texture2D homeTexture = Meblob[hoveringOverHomeButton ? 5 : 0];
					Main.spriteBatch.Draw(homeTexture, new Vector2(homeX, homeY), null, Color.White, 0f, homeTexture.Size() / 2f, 0.8f, SpriteEffects.None, 0f);
					if (hoveringOverHomeButton)
					{
						Main.hoverItemName = "Buy Shiny?";
						Main.LocalPlayer.mouseInterface = true;
						if (Main.mouseLeftRelease && Main.mouseLeft && Main.LocalPlayer.CanBuyItem(VanillaItemSlotWrapperONE.item.value * 5, -1) && VanillaItemSlotWrapperONE.item.value != 0)
						{
							Main.LocalPlayer.BuyItem(VanillaItemSlotWrapperONE.item.value * 5, -1);
							Embee = VanillaItemSlotWrapperONE.item.Clone();
							VanillaItemSlotWrapperONE.item.TurnToAir();
							VanillaItemSlotWrapperTWO.item.TurnToAir();
							player.GetModPlayer<Playerone>(mod).ShinyBought = true;
							player.GetModPlayer<Playerone>(mod).UnbreakableUsed = true;
							Embee.SetDefaults(mod.ItemType("MusicEmblem"));
							(Embee.modItem as Items.MusicEmblem).AddItem(null);
							Main.LocalPlayer.QuickSpawnClonedItem(Embee, 1);
						}
					}
				}
				if(player.GetModPlayer<Playerone>(mod).GlitchBuyable && VanillaItemSlotWrapperTWO.item.IsAir)
				{
					int slotX = 324;
					int slotY = 376;
					string costText = Lang.inter[46].Value + ": ";
					string coinsText = "";
					int[] coins = Utils.CoinsSplit(VanillaItemSlotWrapperONE.item.value * 5);
					if (coins[3] > 0)
					{
						coinsText = coinsText + "[c/" + Colors.AlphaDarken(Colors.CoinPlatinum).Hex3() + ":" + coins[3] + " " + Lang.inter[15].Value + "] ";
					}
					if (coins[2] > 0)
					{
						coinsText = coinsText + "[c/" + Colors.AlphaDarken(Colors.CoinGold).Hex3() + ":" + coins[2] + " " + Lang.inter[16].Value + "] ";
					}
					if (coins[1] > 0)
					{
						coinsText = coinsText + "[c/" + Colors.AlphaDarken(Colors.CoinSilver).Hex3() + ":" + coins[1] + " " + Lang.inter[17].Value + "] ";
					}
					if (coins[0] > 0)
					{
						coinsText = coinsText + "[c/" + Colors.AlphaDarken(Colors.CoinCopper).Hex3() + ":" + coins[0] + " " + Lang.inter[18].Value + "] ";
					}
					ItemSlot.DrawSavings(Main.spriteBatch, slotX + 130, Main.instance.invBottom, true);
					ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, costText, new Vector2((slotX + 50), slotY), new Color(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor), 0f, Vector2.Zero, Vector2.One, -1f, 2f);
					ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, coinsText, new Vector2((slotX + 50) + Main.fontMouseText.MeasureString(costText).X, (float)slotY), Color.LimeGreen, 0f, Vector2.Zero, Vector2.One, -1f, 2f);
					int homeX = slotX - 12;
					int homeY = slotY;
					bool hoveringOverHomeButton = Main.mouseX > homeX - 12 && Main.mouseX < homeX + 12 && Main.mouseY > homeY - 12 && Main.mouseY < homeY + 12 && !PlayerInput.IgnoreMouseInterface;
					Texture2D homeTexture = Meblob[hoveringOverHomeButton ? 6 : 1];
					Main.spriteBatch.Draw(homeTexture, new Vector2(homeX, homeY), null, Color.White, 0f, homeTexture.Size() / 2f, 0.8f, SpriteEffects.None, 0f);
					if (hoveringOverHomeButton)
					{
						Main.hoverItemName = "Buy Glitch?";
						Main.LocalPlayer.mouseInterface = true;
						if (Main.mouseLeftRelease && Main.mouseLeft && Main.LocalPlayer.CanBuyItem(VanillaItemSlotWrapperONE.item.value * 5, -1) && VanillaItemSlotWrapperONE.item.value != 0)
						{
							Main.LocalPlayer.BuyItem(VanillaItemSlotWrapperONE.item.value * 5, -1);
							Embee = VanillaItemSlotWrapperONE.item.Clone();
							VanillaItemSlotWrapperONE.item.TurnToAir();
							player.GetModPlayer<Playerone>(mod).GlitchBought = true;
							player.GetModPlayer<Playerone>(mod).UnbreakableUsed = false;
							Embee.SetDefaults(mod.ItemType("MusicEmblem"));
							(Embee.modItem as Items.MusicEmblem).AddItem(null);
							Main.LocalPlayer.QuickSpawnClonedItem(Embee, 1);
						}
					}
				}
				else if(player.GetModPlayer<Playerone>(mod).GlitchBuyable && !VanillaItemSlotWrapperTWO.item.IsAir)
				{
					int slotX = 324;
					int slotY = 376;
					string costText = Lang.inter[46].Value + ": ";
					string coinsText = "";
					int[] coins = Utils.CoinsSplit(VanillaItemSlotWrapperONE.item.value * 5);
					if (coins[3] > 0)
					{
						coinsText = coinsText + "[c/" + Colors.AlphaDarken(Colors.CoinPlatinum).Hex3() + ":" + coins[3] + " " + Lang.inter[15].Value + "] ";
					}
					if (coins[2] > 0)
					{
						coinsText = coinsText + "[c/" + Colors.AlphaDarken(Colors.CoinGold).Hex3() + ":" + coins[2] + " " + Lang.inter[16].Value + "] ";
					}
					if (coins[1] > 0)
					{
						coinsText = coinsText + "[c/" + Colors.AlphaDarken(Colors.CoinSilver).Hex3() + ":" + coins[1] + " " + Lang.inter[17].Value + "] ";
					}
					if (coins[0] > 0)
					{
						coinsText = coinsText + "[c/" + Colors.AlphaDarken(Colors.CoinCopper).Hex3() + ":" + coins[0] + " " + Lang.inter[18].Value + "] ";
					}
					ItemSlot.DrawSavings(Main.spriteBatch, slotX + 130, Main.instance.invBottom, true);
					ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, costText, new Vector2((slotX + 50), slotY), new Color(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor), 0f, Vector2.Zero, Vector2.One, -1f, 2f);
					ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, coinsText, new Vector2((slotX + 50) + Main.fontMouseText.MeasureString(costText).X, (float)slotY), Color.LimeGreen, 0f, Vector2.Zero, Vector2.One, -1f, 2f);
					int homeX = slotX - 12;
					int homeY = slotY;
					bool hoveringOverHomeButton = Main.mouseX > homeX - 12 && Main.mouseX < homeX + 12 && Main.mouseY > homeY - 12 && Main.mouseY < homeY + 12 && !PlayerInput.IgnoreMouseInterface;
					Texture2D homeTexture = Meblob[hoveringOverHomeButton ? 6 : 1];
					Main.spriteBatch.Draw(homeTexture, new Vector2(homeX, homeY), null, Color.White, 0f, homeTexture.Size() / 2f, 0.8f, SpriteEffects.None, 0f);
					if (hoveringOverHomeButton)
					{
						Main.hoverItemName = "Buy Glitch?";
						Main.LocalPlayer.mouseInterface = true;
						if (Main.mouseLeftRelease && Main.mouseLeft && Main.LocalPlayer.CanBuyItem(VanillaItemSlotWrapperONE.item.value * 5, -1) && VanillaItemSlotWrapperONE.item.value != 0)
						{
							Main.LocalPlayer.BuyItem(VanillaItemSlotWrapperONE.item.value * 5, -1);
							Embee = VanillaItemSlotWrapperONE.item.Clone();
							VanillaItemSlotWrapperONE.item.TurnToAir();
							VanillaItemSlotWrapperTWO.item.TurnToAir();
							player.GetModPlayer<Playerone>(mod).GlitchBought = true;
							player.GetModPlayer<Playerone>(mod).UnbreakableUsed = true;
							Embee.SetDefaults(mod.ItemType("MusicEmblem"));
							(Embee.modItem as Items.MusicEmblem).AddItem(null);
							Main.LocalPlayer.QuickSpawnClonedItem(Embee, 1);
						}
					}
				}
				if(player.GetModPlayer<Playerone>(mod).BeefyBuyable && VanillaItemSlotWrapperTWO.item.IsAir)
				{
					int slotX = 324;
					int slotY = 400;
					string costText = Lang.inter[46].Value + ": ";
					string coinsText = "";
					int[] coins = Utils.CoinsSplit(VanillaItemSlotWrapperONE.item.value * 5);
					if (coins[3] > 0)
					{
						coinsText = coinsText + "[c/" + Colors.AlphaDarken(Colors.CoinPlatinum).Hex3() + ":" + coins[3] + " " + Lang.inter[15].Value + "] ";
					}
					if (coins[2] > 0)
					{
						coinsText = coinsText + "[c/" + Colors.AlphaDarken(Colors.CoinGold).Hex3() + ":" + coins[2] + " " + Lang.inter[16].Value + "] ";
					}
					if (coins[1] > 0)
					{
						coinsText = coinsText + "[c/" + Colors.AlphaDarken(Colors.CoinSilver).Hex3() + ":" + coins[1] + " " + Lang.inter[17].Value + "] ";
					}
					if (coins[0] > 0)
					{
						coinsText = coinsText + "[c/" + Colors.AlphaDarken(Colors.CoinCopper).Hex3() + ":" + coins[0] + " " + Lang.inter[18].Value + "] ";
					}
					ItemSlot.DrawSavings(Main.spriteBatch, slotX + 130, Main.instance.invBottom, true);
					ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, costText, new Vector2((slotX + 50), slotY), new Color(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor), 0f, Vector2.Zero, Vector2.One, -1f, 2f);
					ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, coinsText, new Vector2((slotX + 50) + Main.fontMouseText.MeasureString(costText).X, (float)slotY), Color.Maroon, 0f, Vector2.Zero, Vector2.One, -1f, 2f);
					int homeX = slotX - 12;
					int homeY = slotY;
					bool hoveringOverHomeButton = Main.mouseX > homeX - 12 && Main.mouseX < homeX + 12 && Main.mouseY > homeY - 12 && Main.mouseY < homeY + 12 && !PlayerInput.IgnoreMouseInterface;
					Texture2D homeTexture = Meblob[hoveringOverHomeButton ? 7 : 2];
					Main.spriteBatch.Draw(homeTexture, new Vector2(homeX, homeY), null, Color.White, 0f, homeTexture.Size() / 2f, 0.8f, SpriteEffects.None, 0f);
					if (hoveringOverHomeButton)
					{
						Main.hoverItemName = "Buy Beefy?";
						Main.LocalPlayer.mouseInterface = true;
						if (Main.mouseLeftRelease && Main.mouseLeft && Main.LocalPlayer.CanBuyItem(VanillaItemSlotWrapperONE.item.value * 5, -1) && VanillaItemSlotWrapperONE.item.value != 0)
						{
							Main.LocalPlayer.BuyItem(VanillaItemSlotWrapperONE.item.value * 5, -1);
							Embee = VanillaItemSlotWrapperONE.item.Clone();
							VanillaItemSlotWrapperONE.item.TurnToAir();
							player.GetModPlayer<Playerone>(mod).BeefyBought = true;
							player.GetModPlayer<Playerone>(mod).UnbreakableUsed = false;
							Embee.SetDefaults(mod.ItemType("MusicEmblem"));
							(Embee.modItem as Items.MusicEmblem).AddItem(null);
							Main.LocalPlayer.QuickSpawnClonedItem(Embee, 1);
						}
					}
				}
				else if(player.GetModPlayer<Playerone>(mod).BeefyBuyable && !VanillaItemSlotWrapperTWO.item.IsAir)
				{
					int slotX = 324;
					int slotY = 400;
					string costText = Lang.inter[46].Value + ": ";
					string coinsText = "";
					int[] coins = Utils.CoinsSplit(VanillaItemSlotWrapperONE.item.value * 5);
					if (coins[3] > 0)
					{
						coinsText = coinsText + "[c/" + Colors.AlphaDarken(Colors.CoinPlatinum).Hex3() + ":" + coins[3] + " " + Lang.inter[15].Value + "] ";
					}
					if (coins[2] > 0)
					{
						coinsText = coinsText + "[c/" + Colors.AlphaDarken(Colors.CoinGold).Hex3() + ":" + coins[2] + " " + Lang.inter[16].Value + "] ";
					}
					if (coins[1] > 0)
					{
						coinsText = coinsText + "[c/" + Colors.AlphaDarken(Colors.CoinSilver).Hex3() + ":" + coins[1] + " " + Lang.inter[17].Value + "] ";
					}
					if (coins[0] > 0)
					{
						coinsText = coinsText + "[c/" + Colors.AlphaDarken(Colors.CoinCopper).Hex3() + ":" + coins[0] + " " + Lang.inter[18].Value + "] ";
					}
					ItemSlot.DrawSavings(Main.spriteBatch, slotX + 130, Main.instance.invBottom, true);
					ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, costText, new Vector2((slotX + 50), slotY), new Color(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor), 0f, Vector2.Zero, Vector2.One, -1f, 2f);
					ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, coinsText, new Vector2((slotX + 50) + Main.fontMouseText.MeasureString(costText).X, (float)slotY), Color.Maroon, 0f, Vector2.Zero, Vector2.One, -1f, 2f);
					int homeX = slotX - 12;
					int homeY = slotY;
					bool hoveringOverHomeButton = Main.mouseX > homeX - 12 && Main.mouseX < homeX + 12 && Main.mouseY > homeY - 12 && Main.mouseY < homeY + 12 && !PlayerInput.IgnoreMouseInterface;
					Texture2D homeTexture = Meblob[hoveringOverHomeButton ? 7 : 2];
					Main.spriteBatch.Draw(homeTexture, new Vector2(homeX, homeY), null, Color.White, 0f, homeTexture.Size() / 2f, 0.8f, SpriteEffects.None, 0f);
					if (hoveringOverHomeButton)
					{
						Main.hoverItemName = "Buy Beefy?";
						Main.LocalPlayer.mouseInterface = true;
						if (Main.mouseLeftRelease && Main.mouseLeft && Main.LocalPlayer.CanBuyItem(VanillaItemSlotWrapperONE.item.value * 5, -1) && VanillaItemSlotWrapperONE.item.value != 0)
						{
							Main.LocalPlayer.BuyItem(VanillaItemSlotWrapperONE.item.value * 5, -1);
							Embee = VanillaItemSlotWrapperONE.item.Clone();
							VanillaItemSlotWrapperONE.item.TurnToAir();
							VanillaItemSlotWrapperTWO.item.TurnToAir();
							player.GetModPlayer<Playerone>(mod).BeefyBought = true;
							player.GetModPlayer<Playerone>(mod).UnbreakableUsed = true;
							Embee.SetDefaults(mod.ItemType("MusicEmblem"));
							(Embee.modItem as Items.MusicEmblem).AddItem(null);
							Main.LocalPlayer.QuickSpawnClonedItem(Embee, 1);
						}
					}
				}
				if(player.GetModPlayer<Playerone>(mod).SoullessBuyable && VanillaItemSlotWrapperTWO.item.IsAir)
				{
					int slotX = 324;
					int slotY = 424;
					string costText = Lang.inter[46].Value + ": ";
					string coinsText = "";
					int[] coins = Utils.CoinsSplit(VanillaItemSlotWrapperONE.item.value * 5);
					if (coins[3] > 0)
					{
						coinsText = coinsText + "[c/" + Colors.AlphaDarken(Colors.CoinPlatinum).Hex3() + ":" + coins[3] + " " + Lang.inter[15].Value + "] ";
					}
					if (coins[2] > 0)
					{
						coinsText = coinsText + "[c/" + Colors.AlphaDarken(Colors.CoinGold).Hex3() + ":" + coins[2] + " " + Lang.inter[16].Value + "] ";
					}
					if (coins[1] > 0)
					{
						coinsText = coinsText + "[c/" + Colors.AlphaDarken(Colors.CoinSilver).Hex3() + ":" + coins[1] + " " + Lang.inter[17].Value + "] ";
					}
					if (coins[0] > 0)
					{
						coinsText = coinsText + "[c/" + Colors.AlphaDarken(Colors.CoinCopper).Hex3() + ":" + coins[0] + " " + Lang.inter[18].Value + "] ";
					}
					ItemSlot.DrawSavings(Main.spriteBatch, slotX + 130, Main.instance.invBottom, true);
					ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, costText, new Vector2((slotX + 50), slotY), new Color(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor), 0f, Vector2.Zero, Vector2.One, -1f, 2f);
					ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, coinsText, new Vector2((slotX + 50) + Main.fontMouseText.MeasureString(costText).X, (float)slotY), Color.IndianRed, 0f, Vector2.Zero, Vector2.One, -1f, 2f);
					int homeX = slotX - 12;
					int homeY = slotY;
					bool hoveringOverHomeButton = Main.mouseX > homeX - 12 && Main.mouseX < homeX + 12 && Main.mouseY > homeY - 12 && Main.mouseY < homeY + 12 && !PlayerInput.IgnoreMouseInterface;
					Texture2D homeTexture = Meblob[hoveringOverHomeButton ? 8 : 3];
					Main.spriteBatch.Draw(homeTexture, new Vector2(homeX, homeY), null, Color.White, 0f, homeTexture.Size() / 2f, 0.8f, SpriteEffects.None, 0f);
					if (hoveringOverHomeButton)
					{
						Main.hoverItemName = "Buy Soulless?";
						Main.LocalPlayer.mouseInterface = true;
						if (Main.mouseLeftRelease && Main.mouseLeft && Main.LocalPlayer.CanBuyItem(VanillaItemSlotWrapperONE.item.value * 5, -1) && VanillaItemSlotWrapperONE.item.value != 0)
						{
							Main.LocalPlayer.BuyItem(VanillaItemSlotWrapperONE.item.value * 5, -1);
							Embee = VanillaItemSlotWrapperONE.item.Clone();
							VanillaItemSlotWrapperONE.item.TurnToAir();
							player.GetModPlayer<Playerone>(mod).SoullessBought = true;
							player.GetModPlayer<Playerone>(mod).UnbreakableUsed = false;
							Embee.SetDefaults(mod.ItemType("MusicEmblem"));
							(Embee.modItem as Items.MusicEmblem).AddItem(null);
							Main.LocalPlayer.QuickSpawnClonedItem(Embee, 1);
						}
					}
				}
				else if(player.GetModPlayer<Playerone>(mod).SoullessBuyable && !VanillaItemSlotWrapperTWO.item.IsAir)
				{
					int slotX = 324;
					int slotY = 424;
					string costText = Lang.inter[46].Value + ": ";
					string coinsText = "";
					int[] coins = Utils.CoinsSplit(VanillaItemSlotWrapperONE.item.value * 5);
					if (coins[3] > 0)
					{
						coinsText = coinsText + "[c/" + Colors.AlphaDarken(Colors.CoinPlatinum).Hex3() + ":" + coins[3] + " " + Lang.inter[15].Value + "] ";
					}
					if (coins[2] > 0)
					{
						coinsText = coinsText + "[c/" + Colors.AlphaDarken(Colors.CoinGold).Hex3() + ":" + coins[2] + " " + Lang.inter[16].Value + "] ";
					}
					if (coins[1] > 0)
					{
						coinsText = coinsText + "[c/" + Colors.AlphaDarken(Colors.CoinSilver).Hex3() + ":" + coins[1] + " " + Lang.inter[17].Value + "] ";
					}
					if (coins[0] > 0)
					{
						coinsText = coinsText + "[c/" + Colors.AlphaDarken(Colors.CoinCopper).Hex3() + ":" + coins[0] + " " + Lang.inter[18].Value + "] ";
					}
					ItemSlot.DrawSavings(Main.spriteBatch, slotX + 130, Main.instance.invBottom, true);
					ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, costText, new Vector2((slotX + 50), slotY), new Color(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor), 0f, Vector2.Zero, Vector2.One, -1f, 2f);
					ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, coinsText, new Vector2((slotX + 50) + Main.fontMouseText.MeasureString(costText).X, (float)slotY), Color.IndianRed, 0f, Vector2.Zero, Vector2.One, -1f, 2f);
					int homeX = slotX - 12;
					int homeY = slotY;
					bool hoveringOverHomeButton = Main.mouseX > homeX - 12 && Main.mouseX < homeX + 12 && Main.mouseY > homeY - 12 && Main.mouseY < homeY + 12 && !PlayerInput.IgnoreMouseInterface;
					Texture2D homeTexture = Meblob[hoveringOverHomeButton ? 8 : 3];
					Main.spriteBatch.Draw(homeTexture, new Vector2(homeX, homeY), null, Color.White, 0f, homeTexture.Size() / 2f, 0.8f, SpriteEffects.None, 0f);
					if (hoveringOverHomeButton)
					{
						Main.hoverItemName = "Buy Soulless?";
						Main.LocalPlayer.mouseInterface = true;
						if (Main.mouseLeftRelease && Main.mouseLeft && Main.LocalPlayer.CanBuyItem(VanillaItemSlotWrapperONE.item.value * 5, -1) && VanillaItemSlotWrapperONE.item.value != 0)
						{
							Main.LocalPlayer.BuyItem(VanillaItemSlotWrapperONE.item.value * 5, -1);
							Embee = VanillaItemSlotWrapperONE.item.Clone();
							VanillaItemSlotWrapperONE.item.TurnToAir();
							VanillaItemSlotWrapperTWO.item.TurnToAir();
							player.GetModPlayer<Playerone>(mod).SoullessBought = true;
							player.GetModPlayer<Playerone>(mod).UnbreakableUsed = true;
							Embee.SetDefaults(mod.ItemType("MusicEmblem"));
							(Embee.modItem as Items.MusicEmblem).AddItem(null);
							Main.LocalPlayer.QuickSpawnClonedItem(Embee, 1);
						}
					}
				}
				if(player.GetModPlayer<Playerone>(mod).DebufflessBuyable && VanillaItemSlotWrapperTWO.item.IsAir)
				{
					int slotX = 324;
					int slotY = 448;
					string costText = Lang.inter[46].Value + ": ";
					string coinsText = "";
					int[] coins = Utils.CoinsSplit(VanillaItemSlotWrapperONE.item.value * 5);
					if (coins[3] > 0)
					{
						coinsText = coinsText + "[c/" + Colors.AlphaDarken(Colors.CoinPlatinum).Hex3() + ":" + coins[3] + " " + Lang.inter[15].Value + "] ";
					}
					if (coins[2] > 0)
					{
						coinsText = coinsText + "[c/" + Colors.AlphaDarken(Colors.CoinGold).Hex3() + ":" + coins[2] + " " + Lang.inter[16].Value + "] ";
					}
					if (coins[1] > 0)
					{
						coinsText = coinsText + "[c/" + Colors.AlphaDarken(Colors.CoinSilver).Hex3() + ":" + coins[1] + " " + Lang.inter[17].Value + "] ";
					}
					if (coins[0] > 0)
					{
						coinsText = coinsText + "[c/" + Colors.AlphaDarken(Colors.CoinCopper).Hex3() + ":" + coins[0] + " " + Lang.inter[18].Value + "] ";
					}
					ItemSlot.DrawSavings(Main.spriteBatch, slotX + 130, Main.instance.invBottom, true);
					ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, costText, new Vector2((slotX + 50), slotY), new Color(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor), 0f, Vector2.Zero, Vector2.One, -1f, 2f);
					ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, coinsText, new Vector2((slotX + 50) + Main.fontMouseText.MeasureString(costText).X, (float)slotY), Color.RoyalBlue, 0f, Vector2.Zero, Vector2.One, -1f, 2f);
					int homeX = slotX - 12;
					int homeY = slotY;
					bool hoveringOverHomeButton = Main.mouseX > homeX - 12 && Main.mouseX < homeX + 12 && Main.mouseY > homeY - 12 && Main.mouseY < homeY + 12 && !PlayerInput.IgnoreMouseInterface;
					Texture2D homeTexture = Meblob[hoveringOverHomeButton ? 9 : 4];
					Main.spriteBatch.Draw(homeTexture, new Vector2(homeX, homeY), null, Color.White, 0f, homeTexture.Size() / 2f, 0.8f, SpriteEffects.None, 0f);
					if (hoveringOverHomeButton)
					{
						Main.hoverItemName = "Buy Debuffless?";
						Main.LocalPlayer.mouseInterface = true;
						if (Main.mouseLeftRelease && Main.mouseLeft && Main.LocalPlayer.CanBuyItem(VanillaItemSlotWrapperONE.item.value * 5, -1) && VanillaItemSlotWrapperONE.item.value != 0)
						{
							Main.LocalPlayer.BuyItem(VanillaItemSlotWrapperONE.item.value * 5, -1);
							Embee = VanillaItemSlotWrapperONE.item.Clone();
							VanillaItemSlotWrapperONE.item.TurnToAir();
							player.GetModPlayer<Playerone>(mod).DebufflessBought = true;
							player.GetModPlayer<Playerone>(mod).UnbreakableUsed = false;
							Embee.SetDefaults(mod.ItemType("MusicEmblem"));
							(Embee.modItem as Items.MusicEmblem).AddItem(null);
							Main.LocalPlayer.QuickSpawnClonedItem(Embee, 1);
						}
					}
				}
				else if(player.GetModPlayer<Playerone>(mod).DebufflessBuyable && !VanillaItemSlotWrapperTWO.item.IsAir)
				{
					int slotX = 324;
					int slotY = 448;
					string costText = Lang.inter[46].Value + ": ";
					string coinsText = "";
					int[] coins = Utils.CoinsSplit(VanillaItemSlotWrapperONE.item.value * 5);
					if (coins[3] > 0)
					{
						coinsText = coinsText + "[c/" + Colors.AlphaDarken(Colors.CoinPlatinum).Hex3() + ":" + coins[3] + " " + Lang.inter[15].Value + "] ";
					}
					if (coins[2] > 0)
					{
						coinsText = coinsText + "[c/" + Colors.AlphaDarken(Colors.CoinGold).Hex3() + ":" + coins[2] + " " + Lang.inter[16].Value + "] ";
					}
					if (coins[1] > 0)
					{
						coinsText = coinsText + "[c/" + Colors.AlphaDarken(Colors.CoinSilver).Hex3() + ":" + coins[1] + " " + Lang.inter[17].Value + "] ";
					}
					if (coins[0] > 0)
					{
						coinsText = coinsText + "[c/" + Colors.AlphaDarken(Colors.CoinCopper).Hex3() + ":" + coins[0] + " " + Lang.inter[18].Value + "] ";
					}
					ItemSlot.DrawSavings(Main.spriteBatch, slotX + 130, Main.instance.invBottom, true);
					ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, costText, new Vector2((slotX + 50), slotY), new Color(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor), 0f, Vector2.Zero, Vector2.One, -1f, 2f);
					ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, coinsText, new Vector2((slotX + 50) + Main.fontMouseText.MeasureString(costText).X, (float)slotY), Color.RoyalBlue, 0f, Vector2.Zero, Vector2.One, -1f, 2f);
					int homeX = slotX - 12;
					int homeY = slotY;
					bool hoveringOverHomeButton = Main.mouseX > homeX - 12 && Main.mouseX < homeX + 12 && Main.mouseY > homeY - 12 && Main.mouseY < homeY + 12 && !PlayerInput.IgnoreMouseInterface;
					Texture2D homeTexture = Meblob[hoveringOverHomeButton ? 9 : 4];
					Main.spriteBatch.Draw(homeTexture, new Vector2(homeX, homeY), null, Color.White, 0f, homeTexture.Size() / 2f, 0.8f, SpriteEffects.None, 0f);
					if (hoveringOverHomeButton)
					{
						Main.hoverItemName = "Buy Debuffless?";
						Main.LocalPlayer.mouseInterface = true;
						if (Main.mouseLeftRelease && Main.mouseLeft && Main.LocalPlayer.CanBuyItem(VanillaItemSlotWrapperONE.item.value * 5, -1) && VanillaItemSlotWrapperONE.item.value != 0)
						{
							Main.LocalPlayer.BuyItem(VanillaItemSlotWrapperONE.item.value * 5, -1);
							Embee = VanillaItemSlotWrapperONE.item.Clone();
							VanillaItemSlotWrapperONE.item.TurnToAir();
							VanillaItemSlotWrapperTWO.item.TurnToAir();
							player.GetModPlayer<Playerone>(mod).DebufflessBought = true;
							player.GetModPlayer<Playerone>(mod).UnbreakableUsed = true;
							Embee.SetDefaults(mod.ItemType("MusicEmblem"));
							(Embee.modItem as Items.MusicEmblem).AddItem(null);
							Main.LocalPlayer.QuickSpawnClonedItem(Embee, 1);
						}
					}
				}
			}
		}
		
		protected override void DrawSelf(SpriteBatch spriteBatch)
		{	
			CalculatedStyle innerDimensions = base.GetInnerDimensions();
			Vector2 drawPos = new Vector2(innerDimensions.X + 5f, innerDimensions.Y + 30f);

			float BoxX = innerDimensions.X;
			float BoxY = innerDimensions.Y;
			
			Utils.DrawBorderStringFourWay(spriteBatch, Main.fontItemStack, "", BoxX + 290f, BoxY + 400f, Color.White, Color.Black, new Vector2(0.3f), 0.75f);
		}
	}
}