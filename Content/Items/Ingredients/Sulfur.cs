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
			Item.width = 14;
			Item.height = 12;

			Item.maxStack = 999;
			Item.value = Item.buyPrice(silver: 10);
			Item.createTile = ModContent.TileType<Tiles.Sulfur>();
			Item.consumable = true;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.autoReuse = true;
			Item.rare = ItemRarityID.Yellow;
		}
	}
}