using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ModularTools.Core;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace ModularTools.Content.Items.Tools
{
	public class ModularDrill : ModularItem
	{
		public override string Texture => ModularTools.AssetPath + "Textures/Items/ModularDrill";

		public override void SetDefaults()
		{
			item.damage = 40;
			item.DamageType = ModContent.GetInstance<ModularDamageClass>();
			item.width = 20;
			item.height = 12;
			item.useTime = 12;
			item.useAnimation = 12;
			item.noUseGraphic = true;
			item.noMelee = true;
			item.pick = 40;
			item.tileBoost++;
			item.useStyle = ItemUseStyleID.Shoot;
			item.holdStyle = ItemHoldStyleID.HoldHeavy;
			item.knockBack = 6;
			item.value = Item.buyPrice(gold: 22, silver: 50);
			item.rare = ItemRarityID.Cyan;
			item.UseSound = SoundID.Item23;
			item.autoReuse = true;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.DirtBlock)
				.Register();
		}

		public override void UpdateInventory(Player player)
		{
			foreach (BaseModule module in InstalledModules) module.OnUpdate(this, player);
		}
	}

	public class poop : PlayerDrawLayer
	{
		public override bool GetDefaultVisiblity(PlayerDrawSet drawInfo)
		{
			return drawInfo.drawPlayer.HeldItem?.type == ModContent.ItemType<ModularDrill>();
		}

		public override Position GetDefaultPosition()
		{
			return new BeforeParent(PlayerDrawLayers.ArmOverItem);
		}

		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			Texture2D texture = TextureAssets.Item[ModContent.ItemType<ModularDrill>()].Value;
			Player player = drawInfo.drawPlayer;

			Vector2 position = drawInfo.Center + new Vector2(player.direction == -1 ? 8f : -8f, -2f) - Main.screenPosition;
			position = new Vector2((int)position.X, (int)position.Y);

			float rotation = position.AngleFrom(Main.MouseScreen);
			float scale = 1f;

			Vector2 bodyVect = drawInfo.bodyVect;
			Vector2 compositeOffset_FrontArm = new Vector2(-5 * (!drawInfo.playerEffect.HasFlag(SpriteEffects.FlipHorizontally) ? 1 : -1), 0f);
			bodyVect += compositeOffset_FrontArm;

			Main.NewText(drawInfo.frontShoulderOffset);
			Main.NewText(bodyVect);

			Vector2 vector = new Vector2((int)(drawInfo.Position.X - Main.screenPosition.X - drawInfo.drawPlayer.bodyFrame.Width / 2 + drawInfo.drawPlayer.width / 2), (int)(drawInfo.Position.Y - Main.screenPosition.Y + drawInfo.drawPlayer.height - drawInfo.drawPlayer.bodyFrame.Height + 4f)) + drawInfo.drawPlayer.bodyPosition + new Vector2(drawInfo.drawPlayer.bodyFrame.Width / 2, drawInfo.drawPlayer.bodyFrame.Height / 2);
			Vector2 value = Main.OffsetsPlayerHeadgear[drawInfo.drawPlayer.bodyFrame.Y / drawInfo.drawPlayer.bodyFrame.Height];
			value.Y -= 2f;
			vector += value * -drawInfo.playerEffect.HasFlag(SpriteEffects.FlipVertically).ToDirectionInt();
			float bodyRotation = drawInfo.drawPlayer.bodyRotation;
			vector += compositeOffset_FrontArm;
			Vector2 pp = vector + drawInfo.frontShoulderOffset;
			if (drawInfo.compFrontArmFrame.X / drawInfo.compFrontArmFrame.Width >= 7)
				vector += new Vector2(!drawInfo.playerEffect.HasFlag(SpriteEffects.FlipHorizontally) ? 1 : -1, !drawInfo.playerEffect.HasFlag(SpriteEffects.FlipVertically) ? 1 : -1);

			player.direction = Math.Abs(position.AngleFrom(Main.MouseScreen)) > MathHelper.PiOver2 ? 1 : -1;

			drawInfo.compositeFrontArmRotation = rotation;

			drawInfo.DrawDataCache.Add(new DrawData(
				texture,
				vector,
				null,
				Color.White,
				bodyRotation + rotation,
				bodyVect + new Vector2(25f, -20f),
				scale,
				SpriteEffects.FlipHorizontally,
				0
			));
		}
	}
}