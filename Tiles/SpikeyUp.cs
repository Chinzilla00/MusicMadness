using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace MusicMadness.Tiles
{
    public class SpikeyUp : ModTile
    {
        public bool touch1 = true;
        public bool touch2 = false;
        public bool touch3 = false;
        public bool touch4 = false;

        public override void SetDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Insta-Kill Spikes");
            AddMapEntry(new Color(255, 0, 0), name);
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.addTile(Type);
            disableSmartCursor = true;
        }

        public override bool Dangersense(int i, int j, Player player)
        {
            return true;
        }

        public override bool CanExplode(int i, int j)
        {
            return false;
        }

        public override bool CanKillTile(int i, int j, ref bool blockDamaged)
        {
            return false;
        }

        public override bool CanPlace(int i, int j)
        {
            return true;
        }

        public override void AnimateIndividualTile(int type, int i, int j, ref int frameXOffset, ref int frameYOffset)
        {
            frameXOffset = Main.tileFrame[type];
        }
    }
}