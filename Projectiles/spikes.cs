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

namespace MusicMadness.Projectiles
{
    public class spikes : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Spikes");
        }

        public override void SetDefaults()
        {
            projectile.width = 3072;
            projectile.height = 32;
            projectile.timeLeft = 600;
            projectile.penetrate = 1;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        public override void AI()
		{
			Player targeted = Main.player[(int)projectile.ai[1]];
            if (projectile.position.Y > 16 * (Main.worldSurface) && projectile.penetrate != -1)
            {
				if (projectile.position.X != (targeted.Center.X - 984))
				{
					projectile.position.X = (targeted.Center.X - 984);
				}
                if (projectile.position.Y >= Main.maxTilesY - 200)
                {
                    projectile.velocity.Y = -2f;
                }
                if (projectile.position.Y < Main.maxTilesY - 200)
                {
                    projectile.velocity.Y = -2f;
                }
                if (projectile.position.Y <= 16 * (Main.worldSurface + 450))
                {
                    projectile.velocity.Y = -2.2f;
                }
                if (projectile.position.Y <= 16 * (Main.worldSurface + 350))
                {
                    projectile.velocity.Y = -2.4f;
                }
                if (projectile.position.Y <= 16 * (Main.worldSurface + 300))
                {
                    projectile.velocity.Y = -3f;
                }
                if (projectile.position.Y <= 16 * (Main.worldSurface + 250))
                {
                    projectile.velocity.Y = -3.5f;
                }
                if (projectile.position.Y <= 16 * (Main.worldSurface + 200))
                {
                    projectile.velocity.Y = -4f;
                }
                if (projectile.position.Y <= 16 * (Main.worldSurface + 150))
                {
                    projectile.velocity.Y = -5f;
                }
                if (projectile.position.Y <= 16 * (Main.worldSurface + 50))
                {
                    projectile.velocity.Y = -6f;
                }
            }
            else if (projectile.position.Y <= 16 * (Main.worldSurface) && projectile.penetrate != -1)
            {
				projectile.penetrate = -1;
			}
			if (targeted.dead || !targeted.active)
			{
				projectile.penetrate = -1;
			}
			if (targeted.Top.Y <= projectile.Bottom.Y && projectile.penetrate != -1)
			{
				targeted.KillMe(PlayerDeathReason.ByCustomReason(targeted.name + " Hit there head on a Spike, Fell down, THEN Boiled."), 99999999d, 0, false);
			}
			if (projectile.timeLeft == 1)
			{
				projectile.timeLeft = 600;
			}
		}
	}
}