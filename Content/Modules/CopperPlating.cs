using ModularTools.Content.Items.Armor;
using ModularTools.Core;
using Terraria;
using Terraria.ID;

namespace ModularTools.Content.Modules
{
	public class CopperPlating : BaseModule
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Provides 10 W of passive cooling\nProvides 100 J of heat capacity\nProvides 3 defense");

			AddValidModularItem<ModularHelmet>();
			AddValidModularItem<ModularChestplate>();
			AddValidModularItem<ModularLeggings>();
		}

		public override void AddRecipes()
		{
			ModuleRecipe.Create(Mod, this)
				.AddIngredient(ItemID.CopperBar, 12)
				.Register();
		}

		public override void OnInstalled(ModularItem item)
		{
			// item.HeatStorage.ModifyCapacity(100);
		}

		public override void OnRemoved(ModularItem item)
		{
			// item.HeatStorage.ModifyCapacity(-100);
		}

		public override void OnUpdate(ModularItem item, Player player)
		{
			// item.HeatStorage.ExtractHeat(10);

			player.statDefense += 3;
		}
	}
}