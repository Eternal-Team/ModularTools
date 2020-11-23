using ModularTools.Content.Items.Armor;
using ModularTools.Content.Items.Tools;
using ModularTools.Core;
using Terraria.ID;

namespace ModularTools.Content.Modules
{
	public class CopperPlating : BaseModule
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Provides 10 W of passive cooling\nProvides 100 J of heat capacity");

			AddValidModularItem<ModularHelmet>();
			AddValidModularItem<ModularChestplate>();
			AddValidModularItem<ModularLeggings>();
			AddValidModularItem<ModularPickaxe>();
		}

		public override void AddRecipes()
		{
			ModuleRecipe.Create(Mod, this)
				.AddIngredient(ItemID.CopperBar, 12)
				.Register();
		}

		public override void OnInstalled(ModularItem item)
		{
			item.HeatStorage.ModifyCapacity(100);
		}

		public override void OnRemoved(ModularItem item)
		{
			item.HeatStorage.ModifyCapacity(-100);
		}

		public override void OnUpdate(ModularItem item)
		{
			item.HeatStorage.ExtractHeat(10);
		}
	}
}