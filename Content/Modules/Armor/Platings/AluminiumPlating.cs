﻿using ModularTools.Content.Items.Tools;
using ModularTools.Core;
using ModularTools.DataTags;
using Terraria.ID;

namespace ModularTools.Content.Modules;

public class AluminiumPlating : BaseModule
{
	public override void SetStaticDefaults()
	{
		ModuleData.Defense.Set(Type, 4);
		ModuleTags.Plating.Set(this);
		ModuleData.HeatCapacity.Set(Type, 4500);
		ModuleData.HeatTransfer.Set(Type, 14);

		AddValidModularItem<ModularBore>();
		AddIncompatibleModules(ModuleTags.Plating);
	}

	public override void AddRecipes()
	{
		CreateRecipe()
			.AddIngredient(ItemID.CopperBar, 12)
			.Register();
	}

	// public override void OnInstalled(ModularItem item)
	// {
	// 	item.HeatStorage.ThermalConductivity = Math.Max(14f, item.HeatStorage.ThermalConductivity);
	// }
}