using BaseLibrary;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ModularTools.Content.Items
{
	public class UpgradeStation : BaseItem
	{
		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 16;
			Item.maxStack = 99;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.rare = ItemRarityID.Pink;
			Item.value = Item.sellPrice(gold: 8);
			Item.createTile = ModContent.TileType<Tiles.UpgradeStation>();
		}
	}
}