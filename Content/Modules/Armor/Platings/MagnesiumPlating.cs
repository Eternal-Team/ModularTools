﻿/*using System;
using ModularTools.Core;
using ModularTools.DataTags;
using Terraria;
using Terraria.ID;

namespace ModularTools.Content.Modules;

public class MagnesiumPlating : BaseModule
{
	public override void SetStaticDefaults()
	{
		Tooltip.SetDefault("Increases heat transfer rate to 12 W/m2\nProvides 5 kJ of heat capacity");
		HeatCapacity = 5000;

		ModuleData.Defense.Set(Type, 4);

		// ModuleTags.Plating.Set(Type, true);
		//
		// AddValidModularItems(ModularItemTags.Armor);
		// AddIncompatibleModules(ModuleTags.Plating);
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
		player.statDefense += ModuleData.Defense.Get(Type);
	}
}*/