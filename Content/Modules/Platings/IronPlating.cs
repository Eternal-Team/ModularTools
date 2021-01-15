using System;
using ModularTools.Core;
using Terraria;
using Terraria.ID;

namespace ModularTools.Content.Modules
{
	public class IronPlating : BaseModule
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Increases heat transfer rate to 8 W/m2\nProvides 3.5 kJ of heat capacity\nProvides 5 defense");
			HeatCapacity = 3500;

			ModuleTags.Plating.Set(Type, true);

			AddValidModularItems(ModularItemTags.Armor);
			AddIncompatibleModules(ModuleTags.Plating);
		}

		public override void AddRecipes()
		{
			Create()
				.AddIngredient(ItemID.IronBar, 12)
				.Register();
		}

		public override void OnInstalled(ModularItem item)
		{
			item.HeatStorage.TransferCoefficient = Math.Max(8f, item.HeatStorage.TransferCoefficient);
		}

		public override void OnRemoved(ModularItem item)
		{
		}

		public override void OnUpdate(ModularItem item, Player player)
		{
			player.statDefense += 5;
		}
	}
}