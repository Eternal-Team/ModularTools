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
			item.damage = 40;
			item.DamageType = ModContent.GetInstance<ModularDamageClass>();
			item.width = 20;
			item.height = 12;
			item.useTime = 12;
			item.useAnimation = 12;
			item.noUseGraphic = true;
			item.noMelee = true;
			item.hammer = 40;
			item.useStyle = ItemUseStyleID.Shoot;
			item.holdStyle = ItemHoldStyleID.HoldHeavy;
			item.knockBack = 6;
			item.value = Item.buyPrice(gold: 22, silver: 50);
			item.rare = ItemRarityID.Cyan;
			item.UseSound = SoundID.Item23;
			item.autoReuse = true;
		}

		public override void UpdateInventory(Player player)
		{
			foreach (BaseModule module in InstalledModules) module.OnUpdate(this, player);
		}

		public Vector2 GetOrigin(Player player) => player.direction == 1 ? new Vector2(52f, 18f) : new Vector2(52f, 4f);
	}
}