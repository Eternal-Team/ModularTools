using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ModLoader;

namespace ModularTools.Content.Projectiles
{
	internal class ModularHook : ModProjectile
	{
		public override string Texture => ModularTools.TexturePath + "Projectiles/ModularHook";

		private static Asset<Texture2D> chainTexture;

		public override void Load() => chainTexture = ModContent.GetTexture(ModularTools.TexturePath + "Projectiles/ModularHook_Chain");

		public override void Unload()
		{
			if (chainTexture != null)
			{
				chainTexture.Dispose();
				chainTexture = null;
			}
		}

		public override void SetDefaults()
		{
			Projectile.netImportant = true;
			Projectile.width = 18;
			Projectile.height = 18;
			Projectile.aiStyle = 7;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.timeLeft *= 10;
		}

		public override bool? CanUseGrapple(Player player)
		{
			int hooksOut = 0;
			for (int i = 0; i < 1000; i++)
			{
				if (Main.projectile[i].active && Main.projectile[i].owner == Main.myPlayer && Main.projectile[i].type == Projectile.type)
				{
					hooksOut++;
				}
			}

			return hooksOut <= 2;
		}

		// Return true if it is like: Hook, CandyCaneHook, BatHook, GemHooks
		//public override bool? SingleGrappleHook(Player player)
		//{
		//	return true;
		//}

		// Use this to kill oldest hook. For hooks that kill the oldest when shot, not when the newest latches on: Like SkeletronHand
		// You can also change the projectile like: Dual Hook, Lunar Hook
		//public override void UseGrapple(Player player, ref int type)
		//{
		//	int hooksOut = 0;
		//	int oldestHookIndex = -1;
		//	int oldestHookTimeLeft = 100000;
		//	for (int i = 0; i < 1000; i++)
		//	{
		//		if (Main.projectile[i].active && Main.projectile[i].owner == projectile.whoAmI && Main.projectile[i].type == projectile.type)
		//		{
		//			hooksOut++;
		//			if (Main.projectile[i].timeLeft < oldestHookTimeLeft)
		//			{
		//				oldestHookIndex = i;
		//				oldestHookTimeLeft = Main.projectile[i].timeLeft;
		//			}
		//		}
		//	}
		//	if (hooksOut > 1)
		//	{
		//		Main.projectile[oldestHookIndex].Kill();
		//	}
		//}

		public override float GrappleRange()
		{
			return 500f;
		}

		public override void NumGrappleHooks(Player player, ref int numHooks)
		{
			numHooks = 2;
		}

		public override void GrappleRetreatSpeed(Player player, ref float speed)
		{
			speed = 18f;
		}

		public override void GrapplePullSpeed(Player player, ref float speed)
		{
			speed = 10;
		}

		// note: offset from grapple point, e.g. Static Hook
		// public override void GrappleTargetPoint(Player player, ref float grappleX, ref float grappleY)
		// {
		// 	Vector2 dirToPlayer = projectile.DirectionTo(player.Center);
		// 	const float hangDist = 50f;
		// 	grappleX += dirToPlayer.X * hangDist;
		// 	grappleY += dirToPlayer.Y * hangDist;
		// }

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 playerCenter = Main.player[Projectile.owner].MountedCenter;
			Vector2 center = Projectile.Center;
			Vector2 distToProj = playerCenter - Projectile.Center;
			float projRotation = distToProj.ToRotation() - MathHelper.PiOver2;
			float distance = distToProj.Length();

			while (distance > 30f && !float.IsNaN(distance))
			{
				distToProj.Normalize();
				distToProj *= 24f;

				center += distToProj;
				distToProj = playerCenter - center;
				distance = distToProj.Length();

				spriteBatch.Draw(chainTexture.Value, center - Main.screenPosition, null, lightColor, projRotation, chainTexture.Size() * 0.5f, 1f, SpriteEffects.None, 0f);
			}

			return true;
		}
	}
}