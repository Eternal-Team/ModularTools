using BaseLibrary.Utility;
using ModularTools.Content.Items.Tools;
using ModularTools.Core;
using Terraria;
using Terraria.ID;

namespace ModularTools.Content.Modules
{
	public class NickelIronBattery : BaseModule
	{
		public override string Texture => BaseLibrary.BaseLibrary.PlaceholderTexture;

		public override void SetStaticDefaults()
		{
			AddValidModularItem<ModularBore>();

			AddValidModularItems(ModularItem.Group.Armor);

			EnergyCapacity = 3600 * 25 * 5;

			Tooltip.SetDefault($"Increases energy storage by {TextUtility.ToSI(EnergyCapacity)}");
		}

		public override void AddRecipes()
		{
			Create()
				.AddIngredient(ItemID.IronBar, 12)
				.Register();
		}

		public override void OnInstalled(ModularItem item)
		{
		}

		public override void OnRemoved(ModularItem item)
		{
		}

		public override void OnUpdate(ModularItem item, Player player)
		{
		}
	}
}