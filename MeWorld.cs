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
using Terraria.Graphics;
using Terraria.Map;
using MusicMadness.Tiles;

namespace MusicMadness
{
    public class MeWorld : ModWorld
    {
        private const int saveVersion = 0;
        public static bool downedTheCorpse = false;
        public static int ScorchedBoneBlock = 0;


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

        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
        {
            int PotsIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Pots"));
            tasks[PotsIndex] = (new PassLegacy("Pots", delegate { }));

            int num1 = tasks.FindIndex((GenPass genpass) => genpass.Name.Equals("Final Cleanup"));
            if (num1 != -1)
            {
                tasks.Insert(num1 + 2, new PassLegacy("InsertBonePunHere", delegate (GenerationProgress progress)
                {
                    progress.Message = "Breaking Your Bones...";
                    int maxtilesY = 250;

                    Main.ActiveWorldFileData.WorldSizeY = Main.maxTilesY + maxtilesY;

                    FieldInfo info = typeof(WorldGen).GetField("lastMaxTilesY", BindingFlags.Static | BindingFlags.NonPublic);
                    int get = (int)info.GetValue(null);
                    info.SetValue(null, Main.maxTilesY + maxtilesY);

                    Main.maxTilesY += maxtilesY;

                    Main.bottomWorld += (float)(maxtilesY * 16);
                    Main.maxSectionsY = Main.maxTilesY / 150;

                    Main.Map = new WorldMap(Main.maxTilesX, Main.maxTilesY);
                    MethodInfo methodInfo = typeof(Main).GetMethod("InitMap", BindingFlags.Instance | BindingFlags.NonPublic);
                    methodInfo.Invoke(Main.instance, null);

                    Main.worldSurface += 250.0;
                    Main.rockLayer += 250.0;
                    WorldGen.worldSurface += 250.0;
                    WorldGen.worldSurfaceHigh += 250.0;
                    WorldGen.worldSurfaceLow += 250.0;
                    WorldGen.rockLayer += 250.0;
                    WorldGen.rockLayerHigh += 250.0;
                    WorldGen.rockLayerLow += 250.0;
                    Main.spawnTileY += maxtilesY;
                    Main.dungeonY += maxtilesY;

                    Tile[,] newTiles = new Tile[Main.maxTilesX, Main.maxTilesY];
                    for (int i = 0; i < Main.maxTilesX; i++)
                    {
                        for (int j = 0; j < Main.maxTilesY; j++)
                        {
                            Tile[,] array = newTiles;
                            int bum = i;
                            int bum2 = j;
                            Tile tile = new Tile();
                            array[bum, bum2] = tile;
                            if (j >= 250)
                            {
                                newTiles[i, j].CopyFrom(Main.tile[i, j - 250]);
                                Main.tile[i, j - 250] = null;
                            }
                        }
                    }
                    Main.tile = newTiles;

                    for (int e = 0; e < Main.chest.Length; e++)
                    {
                        if (Main.chest[e] != null)
                        {
                            Main.chest[e].y += 250;
                        }
                    }

                    for (int h = 0; h <= Main.npc.Length; h++)
                    {
                        Main.npc[h].homeTileY = Main.spawnTileY;
                        Main.npc[h].position.Y += 4000f;
                    }

                    BoneBreaker();

                }));
            }
        }
        public void BoneBreaker()
        {
            for (int q = 40; q <= 50; q++)
            {
                for (int p = 40; p < Main.maxTilesX; p++)
                {
                    bool placeSuccessful = false;
                    Tile tile;
                    int tileToPlace = mod.TileType<ScorchedBoneBlock>();
                    while (!placeSuccessful)
                    {
                        WorldGen.PlaceTile(p, q, tileToPlace);
                        tile = Main.tile[p, q];
                        placeSuccessful = tile.active() && tile.type == tileToPlace;
                    }
                }
            }
            for (int q = 150; q >= 140; q--)
            {
                for (int p = 40; p < Main.maxTilesX; p++)
                {
                    bool placeSuccessful = false;
                    Tile tile;
                    int tileToPlace = mod.TileType<ScorchedBoneBlock>();
                    while (!placeSuccessful)
                    {
                        WorldGen.PlaceTile(p, q, tileToPlace);
                        tile = Main.tile[p, q];
                        placeSuccessful = tile.active() && tile.type == tileToPlace;
                    }
                }
            }
        }
    }
    /*internal class ModLiquid
    {
        private readonly Liquid l = new Liquid();
        public bool gravity = true;

        public Liquid liquid
        {
            get
            {
                return l;

            }
        }

        public virtual Texture2D texture
        {
            get { return null; }
        }
        internal int liquidIndex;

        public virtual void Update()
        {
        }

        //Normally trigger if gravity is at false
        public virtual void CustomPhysic()
        {
        }

        public virtual void PreDraw(TileBatch batch)
        {
        }

        public virtual void Draw(TileBatch batch)
        {
        }

        public virtual void PostDraw(TileBatch batch)
        {
        }

        public virtual void playerInteraction(Player target)
        {
        }

        public virtual void npcInteraction(NPC target)
        {
        }

        public virtual void liquidInteraction(ModLiquid target)
        {
        }
    }*/
}