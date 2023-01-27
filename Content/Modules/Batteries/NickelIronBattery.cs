using ModularTools.Content.Items.Tools;
using ModularTools.Core;
using ModularTools.DataTags;
using Terraria.ID;

namespace ModularTools.Content.Modules;

public class NickelIronBattery : BaseModule
{
	public override void SetStaticDefaults()
	{
		ModuleTags.Battery.Set(this);

		ModuleData.EnergyTransfer.Set(Type, 2500);
		ModuleData.EnergyCapacity.Set(Type, 3600 * 25 * 5);

		AddValidModularItem<ModularBore>();
		AddIncompatibleModules(ModuleTags.Battery);
	}

	public override void AddRecipes()
	{
		CreateRecipe()
			.AddIngredient(ItemID.IronBar, 12)
			.Register();
	}
}