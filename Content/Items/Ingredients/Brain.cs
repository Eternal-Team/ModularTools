using BaseLibrary;
using Terraria;

namespace ModularTools.Content.Items.Ingredients
{
	public class Brain : BaseItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("It feels...squishy");
		}

		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 20;

			item.maxStack = 999;
			item.value = Item.buyPrice(gold: 1);
		}
	}
}