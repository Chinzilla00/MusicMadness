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
	public class RisingBoneLava : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rising Bone Lava");
		}
		
		public override void SetDefaults()
		{
			projectile.width = 4800; 
			projectile.height = 1600;
			projectile.timeLeft = 600;
			projectile.penetrate = 1;
			projectile.hide = false;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
		}
		
		
		public override void AI()
		{
			Player targeted = Main.player[(int)projectile.ai[0]];
			if (projectile.position.Y >= Main.worldSurface)
			{
				if (projectile.position.X != (targeted.Center.X - 984))
				{
					projectile.position.X = (targeted.Center.X - 984);
				}
				if (projectile.position.Y >= Main.maxTilesY - 200)
				{
					projectile.velocity.Y = -3f;
				}
				if (projectile.position.Y < Main.maxTilesY - 200)
				{
					projectile.velocity.Y = -3f;
				}
				if (projectile.position.Y <= 16 * (Main.worldSurface + 450))
				{
					projectile.velocity.Y = -3.2f;
				}
				if (projectile.position.Y <= 16 * (Main.worldSurface + 350))
				{
					projectile.velocity.Y = -3.4f;
				}
				if (projectile.position.Y <= 16 * (Main.worldSurface + 300))
				{
					projectile.velocity.Y = -4f;
				}
				if (projectile.position.Y <= 16 * (Main.worldSurface + 250))
				{
					projectile.velocity.Y = -4.5f;
				}
				if (projectile.position.Y <= 16 * (Main.worldSurface + 200))
				{
					projectile.velocity.Y = -5f;
				}
				if (projectile.position.Y <= 16 * (Main.worldSurface + 150))
				{
					projectile.velocity.Y = -6f;
				}
				if (projectile.position.Y <= 16 * (Main.worldSurface + 50))
				{
					projectile.velocity.Y = -7f;
				}
			}
			else
			{
				projectile.timeLeft = 0;
			}
			if (targeted.dead || !targeted.active)
			{
				projectile.timeLeft = 0;
			}
			if (targeted.Bottom.Y >= projectile.Top.Y && projectile.penetrate != -1)
			{
				targeted.KillMe(PlayerDeathReason.ByCustomReason(targeted.name + " Boiled."), 99999999d, 0, false);
			}
			if (projectile.timeLeft == 1)
			{
				projectile.timeLeft = 600;
			}
		}
	}
}