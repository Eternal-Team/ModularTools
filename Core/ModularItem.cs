using System.Collections.Generic;
using System.Linq;
using BaseLibrary;
using EnergyLibrary;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ModularTools.Core
{
	public abstract class ModularItem : BaseItem, IEnergyStorage, IHeatStorage
	{
		public List<BaseModule> InstalledModules;
		public EnergyStorage EnergyStorage;
		public HeatStorage HeatStorage;

		public sealed override void AutoStaticDefaults()
		{
			base.AutoStaticDefaults();

			ModuleLoader.validModulesForItem[Type] = new List<int>();
		}

		public override void OnCreate(ItemCreationContext context)
		{
			InstalledModules = new List<BaseModule>();
			EnergyStorage = new EnergyStorage(0);
			HeatStorage = new HeatStorage
			{
				Capacity = 1f,
				Temperature = Utility.ToKelvin(0f),
				Area = 1f,
				TransferCoefficient = 0f
			};
		}

		public override ModItem Clone(Item item)
		{
			ModularItem modularItem = (ModularItem)base.Clone(item);
			modularItem.InstalledModules = new List<BaseModule>(InstalledModules);
			modularItem.EnergyStorage = EnergyStorage.Clone();
			modularItem.HeatStorage = HeatStorage.Clone();
			return modularItem;
		}

		public override TagCompound Save() => new TagCompound
		{
			["Modules"] = InstalledModules,
			["Energy"] = EnergyStorage.Save()
			// ["Heat"] = HeatStorage.Save(),
		};

		public override void Load(TagCompound tag)
		{
			InstalledModules = tag.GetList<BaseModule>("Modules").ToList();
			EnergyStorage.Load(tag.GetCompound("Energy"));
			// HeatStorage.Load(tag.GetCompound("Heat"));
		}

		public EnergyStorage GetEnergyStorage() => EnergyStorage;
		public HeatStorage GetHeatStorage() => HeatStorage;
	}
}