using Microsoft.Xna.Framework;
using ModularTools.Core;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ModularTools.Content.Items.Tools
{
	public class ModularJackhammer : ModularItem, IRotatableItem
	{
		public override string Texture => ModularTools.TexturePath + "Items/ModularJackhammer";

		public override void SetDefaults()
		{
			Item.damage = 40;
			Item.DamageType = ModContent.GetInstance<ModularDamageClass>();
			Item.width = 20;
			Item.height = 12;
			Item.useTime = 12;
			Item.useAnimation = 12;
			Item.noUseGraphic = true;
			Item.noMelee = true;
			Item.hammer = 40;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.holdStyle = ItemHoldStyleID.HoldHeavy;
			Item.knockBack = 6;
			Item.value = Item.buyPrice(gold: 22, silver: 50);
			Item.rare = ItemRarityID.Cyan;
			Item.UseSound = SoundID.Item23;
			Item.autoReuse = true;
		}

		public override void UpdateInventory(Player player)
		{
			foreach (BaseModule module in InstalledModules) module.OnUpdate(this, player);
		}

		public Vector2 GetOrigin(Player player) => player.direction == 1 ? new Vector2(52f, 18f) : new Vector2(52f, 4f);
	}
}