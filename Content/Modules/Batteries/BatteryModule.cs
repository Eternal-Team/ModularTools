using BaseLibrary.Utility;
using ModularTools.Core;

namespace ModularTools.Content.Modules
{
	public abstract class BatteryModule : BaseModule
	{
		protected abstract long EnergyCapacity { get; }

		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault($"Increases energy storage by {TextUtility.ToSI(EnergyCapacity)}J");

			ModuleTags.Battery.Set(Type, true);

			AddValidModularItems(ModularItemTags.All);
		}

		public override void OnInstalled(ModularItem item)
		{
			item.EnergyStorage.ModifyCapacity(EnergyCapacity);
		}

		public override void OnRemoved(ModularItem item)
		{
			item.EnergyStorage.ModifyCapacity(-EnergyCapacity);
		}
	}
}