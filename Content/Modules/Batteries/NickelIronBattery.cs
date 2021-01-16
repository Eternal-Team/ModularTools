using Terraria.ID;

namespace ModularTools.Content.Modules
{
	public class NickelIronBattery : BatteryModule
	{
		protected override long EnergyCapacity => 3600 * 25 * 5;

		public override void AddRecipes()
		{
			Create()
				.AddIngredient(ItemID.IronBar, 12)
				.Register();
		}
	}
}