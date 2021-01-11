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
	public class ModularBore : ModularItem
	{
		public override string Texture => ModularTools.TexturePath + "Items/ModularBore";

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
		public override bool GetDefaultVisiblity(PlayerDrawSet drawInfo) => drawInfo.drawPlayer.HeldItem?.type == ModContent.ItemType<ModularBore>();

		public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.ArmOverItem);

		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			Texture2D texture = TextureAssets.Item[ModContent.ItemType<ModularBore>()].Value;
			Player player = drawInfo.drawPlayer;

			float scale = 1f;

			Vector2 bodyVect = drawInfo.bodyVect;
			Vector2 compositeOffset_FrontArm = new Vector2(-5 * (!drawInfo.playerEffect.HasFlag(SpriteEffects.FlipHorizontally) ? 1 : -1), 0f);
			bodyVect += compositeOffset_FrontArm;

			Vector2 vector = new Vector2((int)(drawInfo.Position.X - Main.screenPosition.X - drawInfo.drawPlayer.bodyFrame.Width * 0.5f + drawInfo.drawPlayer.width * 0.5f), (int)(drawInfo.Position.Y - Main.screenPosition.Y + drawInfo.drawPlayer.height - drawInfo.drawPlayer.bodyFrame.Height + 4f)) + drawInfo.drawPlayer.bodyPosition + new Vector2(drawInfo.drawPlayer.bodyFrame.Width * 0.5f, drawInfo.drawPlayer.bodyFrame.Height * 0.5f);
			Vector2 value = Main.OffsetsPlayerHeadgear[drawInfo.drawPlayer.bodyFrame.Y / drawInfo.drawPlayer.bodyFrame.Height];
			value.Y -= 2f;
			vector += value * -drawInfo.playerEffect.HasFlag(SpriteEffects.FlipVertically).ToDirectionInt();
			float bodyRotation = drawInfo.drawPlayer.bodyRotation;
			vector += compositeOffset_FrontArm;
			if (drawInfo.compFrontArmFrame.Width != 0 && drawInfo.compFrontArmFrame.X / drawInfo.compFrontArmFrame.Width >= 7)
				vector += new Vector2(!drawInfo.playerEffect.HasFlag(SpriteEffects.FlipHorizontally) ? 1 : -1, !drawInfo.playerEffect.HasFlag(SpriteEffects.FlipVertically) ? 1 : -1);

			float rotation = vector.AngleFrom(Main.MouseScreen) + bodyRotation;
			bool flip = Math.Abs(rotation) <= MathHelper.PiOver2;

			player.direction = flip ? -1 : 1;

			drawInfo.compositeFrontArmRotation = flip ? rotation : MathHelper.Pi + rotation;

			drawInfo.DrawDataCache.Add(new DrawData(
				texture,
				vector,
				null,
				Color.White,
				rotation,
				bodyVect + (flip ? new Vector2(25f, -20f) : new Vector2(35f, -10f)),
				scale,
				SpriteEffects.None,
				0
			));
		}
	}
}