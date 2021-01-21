using ModularTools.DataTags;
using Terraria.ID;

namespace ModularTools.Content.Modules
{
	public class LFPBattery : BatteryModule
	{
		protected override long EnergyCapacity => 3600 * 120 * 5;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			
			ModuleData.EnergyTransfer.Set(Type, 5000);
		}
		
		public override void AddRecipes()
		{
			Create()
				.AddIngredient(ItemID.IronBar, 12)
				.Register();
		}
	}
}