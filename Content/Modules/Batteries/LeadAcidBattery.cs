using ModularTools.Content.Items.Tools;
using ModularTools.Core;
using ModularTools.DataTags;
using Terraria.ID;

namespace ModularTools.Content.Modules;

public class LeadAcidBattery : BaseModule
{
	public override string Texture => ModularTools.TexturePath + "Projectiles/ModularHook";

	public override void SetStaticDefaults()
	{
		ModuleTags.Battery.Set(this);

		ModuleData.EnergyTransfer.Set(Type, 1000);
		ModuleData.EnergyCapacity.Set(Type, 3600 * 40 * 5);

		AddValidModularItem<ModularBore>();
		AddIncompatibleModules(ModuleTags.Battery);
		AddRequiredModule<AluminiumPlating>();
	}

	public override void AddRecipes()
	{
		CreateRecipe()
			.AddRecipeGroup(RecipeGroupID.Wood, 50)
			.AddIngredient(ItemID.IronBar, 12)
			.Register();
	}
}