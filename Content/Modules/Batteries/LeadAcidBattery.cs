﻿using ModularTools.DataTags;
using Terraria.ID;

namespace ModularTools.Content.Modules
{
	public class LeadAcidBattery : BatteryModule
	{
		protected override long EnergyCapacity => 3600 * 40 * 5;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			
			ModuleData.EnergyTransfer.Set(Type, 1000);
		}

		public override void AddRecipes()
		{
			Create()
				.AddIngredient(ItemID.IronBar, 12)
				.Register();
		}
	}
}