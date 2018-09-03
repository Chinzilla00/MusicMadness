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
                Projectile progee = Main.projectile[mod.ProjectileType("RisingBoneLava")];
                if (projectile.position.Y != (progee.position.Y - 500))
                {
                    projectile.position.Y = (progee.position.Y - 500);
                }
            }
            else if (projectile.position.Y <= 16 * (Main.worldSurface) && projectile.timeLeft != 0)
            {
                projectile.timeLeft = 0;
            }
			if (targeted.dead || !targeted.active)
			{
                projectile.timeLeft = 0;
            }
			if (targeted.Top.Y <= projectile.Bottom.Y && projectile.timeLeft != 0)
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