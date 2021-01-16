using BaseLibrary.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace ModularTools.Content.Items.Tools
{
	public interface IRotatableItem
	{
		Vector2 GetOrigin(Player player);
	}

	public class RotatableItemDrawLayer : PlayerDrawLayer
	{
		public override bool GetDefaultVisiblity(PlayerDrawSet drawInfo)
		{
			Item item = drawInfo.drawPlayer.HeldItem;
			return !item.IsAir && item.ModItem is IRotatableItem;
		}

		public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.ArmOverItem);

		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			Player player = drawInfo.drawPlayer;

			ModItem item = player.HeldItem.ModItem;
			Texture2D texture = TextureAssets.Item[item.Type].Value;

			Vector2 compositeOffset_FrontArm = new Vector2(-5 * (!drawInfo.playerEffect.HasFlag(SpriteEffects.FlipHorizontally) ? 1 : -1), 0f);

			Vector2 vector = new Vector2((int)(drawInfo.Position.X - Main.screenPosition.X - drawInfo.drawPlayer.bodyFrame.Width * 0.5f + drawInfo.drawPlayer.width * 0.5f), (int)(drawInfo.Position.Y - Main.screenPosition.Y + drawInfo.drawPlayer.height - drawInfo.drawPlayer.bodyFrame.Height + 4f)) + drawInfo.drawPlayer.bodyPosition + new Vector2(drawInfo.drawPlayer.bodyFrame.Width * 0.5f, drawInfo.drawPlayer.bodyFrame.Height * 0.5f);
			Vector2 value = Main.OffsetsPlayerHeadgear[drawInfo.drawPlayer.bodyFrame.Y / drawInfo.drawPlayer.bodyFrame.Height];
			value.Y -= 2f;
			vector += value * -drawInfo.playerEffect.HasFlag(SpriteEffects.FlipVertically).ToDirectionInt();
			vector += compositeOffset_FrontArm;
			if (drawInfo.compFrontArmFrame.Width != 0 && drawInfo.compFrontArmFrame.X / drawInfo.compFrontArmFrame.Width >= 7)
				vector += new Vector2(!drawInfo.playerEffect.HasFlag(SpriteEffects.FlipHorizontally) ? 1 : -1, !drawInfo.playerEffect.HasFlag(SpriteEffects.FlipVertically) ? 1 : -1);

			float targetItemRotation = MathHelper.PiOver2 - MathUtility.AngleBetween(Main.MouseWorld - player.Center, -Vector2.UnitY);

			drawInfo.compositeFrontArmRotation = targetItemRotation;

			if (player.direction == 1) drawInfo.compositeFrontArmRotation -= MathHelper.Pi;

			drawInfo.DrawDataCache.Add(new DrawData(
				texture,
				vector,
				null,
				Color.White,
				targetItemRotation,
				((IRotatableItem)item).GetOrigin(player),
				1f,
				player.direction == 1 ? SpriteEffects.FlipVertically | SpriteEffects.FlipHorizontally : SpriteEffects.FlipHorizontally,
				0
			));
		}
	}

	public class RotatableItemPlayer : ModPlayer
	{
		public override void PreUpdate()
		{
			if (!Player.HeldItem.IsAir && Player.HeldItem.ModItem is IRotatableItem) Player.direction = Main.MouseWorld.X >= Player.Center.X ? 1 : -1;
		}

		public override void PostUpdate()
		{
			if (!Player.HeldItem.IsAir && Player.HeldItem.ModItem is IRotatableItem) Player.direction = Main.MouseWorld.X >= Player.Center.X ? 1 : -1;
		}

		public override bool PreItemCheck()
		{
			if (!Player.HeldItem.IsAir && Player.HeldItem.ModItem is IRotatableItem) Player.direction = Main.MouseWorld.X >= Player.Center.X ? 1 : -1;

			return true;
		}
	}
}