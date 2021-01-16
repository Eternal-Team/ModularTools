using System;
using ModularTools.Core;
using ModularTools.DataTags;
using Terraria;
using Terraria.ID;

namespace ModularTools.Content.Modules
{
	public class TungstenCarbidePlating : BaseModule
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Increases heat transfer rate to 13 W/m2\nProvides 1 kJ of heat capacity");
			HeatCapacity = 1000;

			ModuleData.Defense.Set(Type, 8);

			ModuleTags.Plating.Set(Type, true);

			AddValidModularItems(ModularItemTags.Armor);
			AddIncompatibleModules(ModuleTags.Plating);
		}

		public override void AddRecipes()
		{
			Create()
				.AddIngredient(ItemID.CopperBar, 12)
				.Register();
		}

		public override void OnInstalled(ModularItem item)
		{
			item.HeatStorage.TransferCoefficient = Math.Max(14f, item.HeatStorage.TransferCoefficient);
		}

		public override void OnRemoved(ModularItem item)
		{
		}

		public override void OnUpdate(ModularItem item, Player player)
		{
			player.statDefense += ModuleData.Defense.Get(Type);
		}
	}
}