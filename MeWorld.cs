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
            BitsByte fags = new BitsByte();
            fags[0] = downedTheCorpse;
            writer.Write(fags);
        }
        public override void NetReceive(BinaryReader reader)
        {
            BitsByte fags = reader.ReadByte();
            downedTheCorpse = fags[0];
        }

        public override void PostWorldGen()
        {
            int maxtilesY = 250;

            FieldInfo info = typeof(WorldFileData).GetField("WorldSizeY", BindingFlags.Instance | BindingFlags.Public);
            int get = (int)info.GetValue(Main.ActiveWorldFileData);
            info.SetValue(Main.ActiveWorldFileData, Main.maxTilesY + maxtilesY);

            info = typeof(WorldGen).GetField("lastMaxTilesY", BindingFlags.Static | BindingFlags.NonPublic);
            get = (int)info.GetValue(null);
            info.SetValue(null, Main.maxTilesY + maxtilesY);

            Main.maxTilesY += maxtilesY;

            Main.bottomWorld += (float)(maxtilesY * 16);
            Main.maxSectionsX = Main.maxTilesX / 200;
            Main.maxSectionsY = Main.maxTilesY / 150;
        }

        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
        {
            int PotsIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Pots"));
            tasks[PotsIndex] = (new PassLegacy("Pots", delegate { }));

            int num1 = tasks.FindIndex((GenPass genpass) => genpass.Name.Equals("Floating Islands Houses"));
            if (num1 != -1)
            {
                tasks.Insert(num1 + 1, new PassLegacy("InsertBonePunHere", delegate (GenerationProgress progress)
                {
                    progress.Message = "[c/FF0000:Breaking Your Bones...]";
                    
                }));
            }
        }
    }
}