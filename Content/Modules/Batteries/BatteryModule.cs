using BaseLibrary.Utility;
using ModularTools.Core;
using ModularTools.DataTags;

namespace ModularTools.Content.Modules
{
	public abstract class BatteryModule : BaseModule
	{
		protected abstract long EnergyCapacity { get; }

		public override void SetStaticDefaults()
		{
			ModuleTags.Battery.Set(Type, true);
			ModuleData.EnergyCapacity.Set(Type, EnergyCapacity);

			AddValidModularItems(ModularItemTags.All);
		}

		public override void OnInstalled(ModularItem item)
		{
			item.EnergyStorage.ModifyCapacity(EnergyCapacity);

			ulong max = 0;
			foreach (BaseModule module in item.InstalledModules)
			{
				if (ModuleData.EnergyTransfer.TryGet(module.Type, out ulong val) && val > max) max = val;
			}

			item.EnergyStorage.SetMaxTransfer(max);
		}

		public override void OnRemoved(ModularItem item)
		{
			item.EnergyStorage.ModifyCapacity(-EnergyCapacity);

			ulong max = 0;
			foreach (BaseModule module in item.InstalledModules)
			{
				if (ModuleData.EnergyTransfer.TryGet(module.Type, out ulong val) && val > max) max = val;
			}

			item.EnergyStorage.SetMaxTransfer(max);
		}
	}
}