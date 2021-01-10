using BaseLibrary;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ModularTools.Content.Items.Ingredients
{
	public class Sulfur : BaseItem
	{
		public override string Texture => ModularTools.TexturePath + "Items/Sulfur";

		public override void SetDefaults()
		{
			item.width = 16;
			item.height = 16;

			item.maxStack = 999;
			item.value = Item.buyPrice(silver: 10);
			item.createTile = ModContent.TileType<Tiles.Sulfur>();
			item.consumable = true;
			item.useStyle = ItemUseStyleID.Swing;
			item.useTurn = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.autoReuse = true;
			item.rare = ItemRarityID.Blue;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.DirtBlock)
				.Register();
		}
	}
}