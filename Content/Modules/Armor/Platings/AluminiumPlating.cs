/*using System;
using ModularTools.Core;
using ModularTools.DataTags;
using Terraria;
using Terraria.ID;

namespace ModularTools.Content.Modules;

public class AluminiumPlating : BaseModule
{
	public override void SetStaticDefaults()
	{
		Tooltip.SetDefault("Increases heat transfer rate to 12 W/m2\nProvides 4.5 kJ of heat capacity");

		ModuleData.Defense.Set(Type, 4);
		ModuleTags.Plating.Set(Type, true);
		ModuleData.HeatCapacity.Set(Type, 4500);
		
		// AddValidModularItems(ModuleTags.Armor);
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
		item.HeatStorage.ThermalConductivity = Math.Max(14f, item.HeatStorage.ThermalConductivity);
	}

	public override void OnRemoved(ModularItem item)
	{
	}

	public override void OnUpdate(ModularItem item, Player player)
	{
		// todo: this sucks, maybe the ModularItem can handle it
		player.statDefense += ModuleData.Defense.Get(Type);
	}
}*/