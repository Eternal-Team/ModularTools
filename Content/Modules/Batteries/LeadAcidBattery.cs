using BaseLibrary.Utility;
using ModularTools.Core;
using Terraria;
using Terraria.ID;

namespace ModularTools.Content.Modules
{
	public class LeadAcidBattery : BaseModule
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault($"Increases energy storage by {TextUtility.ToSI(EnergyCapacity)}");
			EnergyCapacity = 3600 * 40 * 5;

			ModuleTags.Battery.Set(Type, true);

			AddValidModularItems(ModularItemTags.All);
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