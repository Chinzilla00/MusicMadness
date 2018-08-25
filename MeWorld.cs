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
        public static bool Wumpus = true;
        public static int ScorchedBoneBlocks = 0;

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
            Wumpus = false;
        }

        public override void ResetNearbyTileEffects()
        {
            Playerone modPlayer = Main.LocalPlayer.GetModPlayer<Playerone>(mod);
            ScorchedBoneBlocks = 0;
        }

        public override void TileCountsAvailable(int[] tileCounts)
        {
            ScorchedBoneBlocks = tileCounts[mod.TileType("ScorchedBoneBlock")];
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

                    for (int e = 0; e < Main.chest.Length; e++)
                    {
                        if (Main.chest[e] != null)
                        {
                            Main.chest[e].y += 250;
                        }
                    }

                    for (int h = 0; h < Main.npc.Length; h++)
                    {
                        if (Main.npc[h] != null && Main.npc[h].type == NPCID.Guide)
                        {
                            Main.npc[h].homeTileY += 250;
                            Main.npc[h].position.Y += 4000;
                            break;
                        }
                    }

                    BoneBreaker();

                }));
            }
        }
        public void BoneBreaker()
        {
            for (int q = 40; q <= 75; q++)
            {
                for (int p = 40; p < Main.maxTilesX - 40; p++)
                {
                    Main.tile[p, q].active(true);
                    Main.tile[p, q].type = (ushort)(mod.TileType<ScorchedBoneBlock>());
                }
            }
            for (int q = 150; q >= 115; q--)
            {
                for (int p = 40; p < Main.maxTilesX - 40; p++)
                {
                    Main.tile[p, q].active(true);
                    Main.tile[p, q].type = (ushort)(mod.TileType<ScorchedBoneBlock>());
                }
            }
            float widthScale = (Main.maxTilesX / 400f);
            int numberToGenerate = WorldGen.genRand.Next(3, (int)(5f * widthScale));
            for (int k = 0; k < numberToGenerate; k++)
            {
                bool success = false;
                int attempts = 0;
                while (!success)
                {
                    attempts++;
                    if (attempts > 800)
                    {
                        success = true;
                        continue;
                    }
                    int i = WorldGen.genRand.Next(200, Main.maxTilesX - 200);
                    int j = 71;
                    if (Main.tile[i, j].type == (ushort)(mod.TileType<ScorchedBoneBlock>()))
                    {
                        j++;
                        if (j <= 150)
                        {
                            for (int l = i - 12; l < i + 12; l++)
                            {
                                for (int m = j; m > j - 18; m--)
                                {
                                    if (Main.tile[l, m].active())
                                    {
                                        int type = (int)Main.tile[l, m].type;
                                    }
                                }
                            }
                            success = PlaceBonecano1(i, j + 20);
                        }
                    }
                }
            }
            float widthScale2 = (Main.maxTilesX / 400f);
            int numberToGenerate2 = WorldGen.genRand.Next(3, (int)(5f * widthScale2));
            for (int k = 0; k < numberToGenerate2; k++)
            {
                bool success = false;
                int attempts = 0;
                while (!success)
                {
                    attempts++;
                    if (attempts > 800)
                    {
                        success = true;
                        continue;
                    }
                    int i = WorldGen.genRand.Next(200, Main.maxTilesX - 200);
                    int j = 117;
                    if (Main.tile[i, j].type == (ushort)(mod.TileType<ScorchedBoneBlock>()))
                    {
                        j++;
                        if (j <= 150)
                        {
                            for (int l = i - 12; l < i + 12; l++)
                            {
                                for (int m = j; m > j - 18; m--)
                                {
                                    if (Main.tile[l, m].active())
                                    {
                                        int type = (int)Main.tile[l, m].type;
                                    }
                                }
                            }
                            success = PlaceBonecano2(i, j - 20);
                        }
                    }
                }
            }
        }

        int[,] BonecanoShape = new int[,]
        {
        {0,0,0,0,0,0,0,0,0,1,1,0,0,1,1,0,0,0,0,0,0,0,0,0 },
        {0,0,0,0,0,0,0,0,1,1,0,0,0,0,1,1,0,0,0,0,0,0,0,0 },
        {0,0,0,0,0,0,0,0,1,1,0,0,0,0,1,1,0,0,0,0,0,0,0,0 },
        {0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0 },
        {0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0 },
        {0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0 },
        {0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0 },
        {0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0 },
        {0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0 },
        {0,0,0,0,0,1,1,1,0,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0 },
        {0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,1,1,1,1,0,0,0,0 },
        {0,0,0,0,1,1,1,1,1,0,0,0,0,0,0,1,1,1,1,1,0,0,0,0 },
        {0,0,0,1,1,1,1,1,1,0,0,0,0,0,0,1,1,1,1,1,1,0,0,0 },
        {0,0,1,1,1,1,1,1,0,0,0,0,0,0,0,0,1,1,1,1,1,1,0,0 },
        {0,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,0 },
        {0,1,1,1,1,1,1,1,1,2,2,2,2,2,2,1,1,1,1,1,1,1,1,0 },
        {1,1,1,1,1,1,1,1,1,3,3,3,3,3,3,1,1,1,1,1,1,1,1,1 },
        };

        public bool PlaceBonecano1(int i, int j)
        {
            for (int y = 0; y < BonecanoShape.GetLength(0); y++)
            {
                for (int x = 0; x < BonecanoShape.GetLength(1); x++)
                {
                    int k = i - x;
                    int l = j - y;
                    if (WorldGen.InWorld(k, l, 30))
                    {
                        Tile tile = Framing.GetTileSafely(k, l);
                        switch (BonecanoShape[y, x])
                        {
                            case 1:
                                tile.type = (ushort)(mod.TileType<ScorchedBoneBlock>());
                                tile.active(true);
                                break;
                            case 2:
                                tile.type = (ushort)(mod.TileType<SpikeyUp>());
                                tile.active(true);
                                break;
                            case 3:
                                tile.type = (ushort)(mod.TileType<SpikeyUpBottom>());
                                tile.active(true);
                                break;
                        }
                    }
                }
            }
            return true;
        }

        public bool PlaceBonecano2(int i, int j)
        {
            for (int y = 0; y < BonecanoShape.GetLength(0); y++)
            {
                for (int x = 0; x < BonecanoShape.GetLength(1); x++)
                {
                    int k = i + x;
                    int l = j + y;
                    if (WorldGen.InWorld(k, l, 30))
                    {
                        Tile tile = Framing.GetTileSafely(k, l);
                        switch (BonecanoShape[y, x])
                        {
                            case 1:
                                tile.type = (ushort)(mod.TileType<ScorchedBoneBlock>());
                                tile.active(true);
                                break;
                            case 2:
                                tile.type = (ushort)(mod.TileType<SpikeyDown>());
                                tile.active(true);
                                break;
                            case 3:
                                tile.type = (ushort)(mod.TileType<SpikeyDownBottom>());
                                tile.active(true);
                                break;
                        }
                    }
                }
            }
            return true;
        }
    }
}
       