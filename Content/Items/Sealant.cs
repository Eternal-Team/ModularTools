using BaseLibrary;
using Terraria;

namespace ModularTools.Content.Items;

public class Sealant : BaseItem
{
	public override void SetDefaults()
	{
		Item.width = 20;
		Item.height = 20;

		Item.maxStack = 999;
		Item.value = Item.buyPrice(gold: 1);
	}
}