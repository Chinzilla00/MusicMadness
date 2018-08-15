using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Terraria.GameContent.UI;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using Terraria.Utilities;
using Terraria.ModLoader.IO;
using Terraria.Localization;
using Terraria.World.Generation;
using Terraria.GameContent.Generation;
using Terraria.DataStructures;
using System.Reflection;
using Terraria.IO;
using Terraria.ModLoader.UI;
using CalamityMod;

namespace MusicMadness
{
    public class MeWorld : ModWorld
    {
        private const int saveVersion = 0;
        public static bool downedTheCorpse = false;

        public override void Initialize()
        {
            downedTheCorpse = false;
        }

        public override TagCompound Save()
        {
            var downed = new List<string>();
            if (downedTheCorpse) downed.Add("Corpse");

            return new TagCompound {
                {"downed", downed}
            };
        }

        public override void Load(TagCompound tag)
        {
            var downed = tag.GetList<string>("downed");
            downedTheCorpse = downed.Contains("Corpse");
        }

        public override void NetSend(BinaryWriter writer)
        {
            BitsByte flags = new BitsByte();
            flags[0] = downedTheCorpse;
            writer.Write(flags);
        }
        public override void NetReceive(BinaryReader reader)
        {
            BitsByte flags = reader.ReadByte();
            downedTheCorpse = flags[0];
        }

        public override void PreWorldGen()
        {
            int maxtilesY = 250;

            Mod Calamity = ModLoader.GetMod("CalamityMod");
            if (Calamity != null)
            {
                maxtilesY += 200;
            }

            FieldInfo info = typeof(WorldFileData).GetField("WorldSizeY", BindingFlags.Instance | BindingFlags.Public);
            int get = (int)info.GetValue(Main.ActiveWorldFileData);
            info.SetValue(Main.ActiveWorldFileData, maxtilesY);

            info = typeof(WorldGen).GetField("lastMaxTilesY", BindingFlags.Static | BindingFlags.NonPublic);
            get = (int)info.GetValue(null);
            info.SetValue(null, maxtilesY);

            Main.maxTilesY += maxtilesY;

            Main.bottomWorld += (float)(maxtilesY * 16);
            Main.maxSectionsX = Main.maxTilesX / 200;
            Main.maxSectionsY = Main.maxTilesY / 150;

            Main.tile = new Tile[Main.maxTilesX, Main.maxTilesY];

            WorldGen.clearWorld();
        }

        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
        {
            int PotsIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Pots"));
            tasks[PotsIndex] = (new PassLegacy("Pots", delegate { }));

            Mod Calamity = ModLoader.GetMod("CalamityMod");
            if (Calamity != null)
            {
                CalamityTasks1();
            }
        }

        public void CalamityTasks1()
        {
            FieldInfo info1 = typeof(CalamityWorld).GetField("fuhX", BindingFlags.Static | BindingFlags.NonPublic);
            int fuhX = (int)info1.GetValue(null);

            FieldInfo info2 = typeof(CalamityWorld).GetField("fuhY", BindingFlags.Static | BindingFlags.NonPublic);
            int fuhY = (int)info2.GetValue(null);

            int num26 = WorldGen.genRand.Next((int)((double)Main.maxTilesX * 0.1), (int)((double)Main.maxTilesX * 0.15));
            int num27 = Main.maxTilesY - 100;
            fuhX = num26;
            fuhY = num27;
            CalamityWorld.UnderworldIsland(num26, num27, 180, 201, 120, 136);
            CalamityWorld.UnderworldIsland(num26 - 50, num27 - 30, 100, 111, 60, 71);
            CalamityWorld.UnderworldIsland(num26 + 50, num27 - 30, 100, 111, 60, 71);
            CalamityWorld.ChasmGenerator(fuhX - 110, fuhY - 10, WorldGen.genRand.Next(150) + 150, false);
            CalamityWorld.ChasmGenerator(fuhX + 110, fuhY - 10, WorldGen.genRand.Next(150) + 150, false);
            CalamityWorld.UnderworldIsland(num26 - 150, num27 - 30, 60, 66, 35, 41);
            CalamityWorld.UnderworldIsland(num26 + 150, num27 - 30, 60, 66, 35, 41);
            CalamityWorld.UnderworldIsland(num26 - 180, num27 - 20, 60, 66, 35, 41);
            CalamityWorld.UnderworldIsland(num26 + 180, num27 - 20, 60, 66, 35, 41);
            CalamityWorld.UnderworldIslandHouse(fuhX, fuhY + 30, 1323);
            CalamityWorld.UnderworldIslandHouse(fuhX - 22, fuhY + 15, 1322);
            CalamityWorld.UnderworldIslandHouse(fuhX + 22, fuhY + 15, 535);
            CalamityWorld.UnderworldIslandHouse(fuhX - 50, fuhY - 30, 112);
            CalamityWorld.UnderworldIslandHouse(fuhX + 50, fuhY - 30, 906);
            CalamityWorld.UnderworldIslandHouse(fuhX - 150, fuhY - 30, 218);
            CalamityWorld.UnderworldIslandHouse(fuhX + 150, fuhY - 30, 3019);
            CalamityWorld.UnderworldIslandHouse(fuhX - 180, fuhY - 20, 278);
            CalamityWorld.UnderworldIslandHouse(fuhX + 180, fuhY - 20, 220);
        }
    }
}