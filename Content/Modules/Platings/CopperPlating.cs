using System;
using ModularTools.Core;
using Terraria;
using Terraria.ID;

namespace ModularTools.Content.Modules
{
	public class CopperPlating : BaseModule
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Increases heat transfer rate to 14 W/m2\nProvides 3 kJ of heat capacity\nProvides 3 defense");

			AddValidModularItems(ModularItem.Group.Armor);

			HeatCapacity = 3000;
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
			player.statDefense += 3;
		}
	}
}