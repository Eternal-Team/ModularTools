using System;
using System.Collections.Generic;
using System.Linq;
using BaseLibrary;
using BaseLibrary.Utility;
using EnergyLibrary;
using Microsoft.Xna.Framework;
using ModularTools.Content;
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
			ModularItemTags.All.Set(Type, true);
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

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			tooltips.Add(new TooltipLine(Mod, "MT:Energy", TextUtility.WithColor($"Energy: {EnergyStorage.Energy}/{EnergyStorage.Capacity} J", new Color(28, 218, 232))));
			tooltips.Add(new TooltipLine(Mod, "MT:Temperature", TextUtility.WithColor($"Temperature: {Math.Round(HeatStorage.Temperature)}", new Color(255, 91, 20))));
			tooltips.Add(new TooltipLine(Mod, "MT:Transfering", TextUtility.WithColor($"Transfering: {Math.Round(HeatStorage.TransferCoefficient * HeatStorage.Area, 1)} W", new Color(185, 255, 20))));
			tooltips.Add(new TooltipLine(Mod, "MT:Modules", $"{InstalledModules.Count} installed modules"));
		}

		public override TagCompound Save() => new TagCompound
		{
			["Modules"] = InstalledModules,
			["Energy"] = EnergyStorage.Save(),
			["Heat"] = HeatStorage.Save()
		};

		public override void Load(TagCompound tag)
		{
			InstalledModules = tag.GetList<BaseModule>("Modules").ToList();
			EnergyStorage.Load(tag.GetCompound("Energy"));
			HeatStorage.Load(tag.GetCompound("Heat"));
		}

		public EnergyStorage GetEnergyStorage() => EnergyStorage;

		public HeatStorage GetHeatStorage() => HeatStorage;

		#region Utility
		public bool IsInstalled(int moduleType) => InstalledModules.Any(module => module.Type == moduleType);
		public bool IsInstalled<T>() where T : BaseModule => IsInstalled(ModuleLoader.ModuleType<T>());

		public bool CanInstall(int moduleType) => !IsInstalled(moduleType) && !ModuleLoader.GetIncompatibleModules(moduleType).Any(IsInstalled) && ModuleLoader.GetRequirements(moduleType).All(IsInstalled);
		public bool CanInstall<T>() where T : BaseModule => CanInstall(ModuleLoader.ModuleType<T>());

		public bool CanUninstall(int moduleType) => IsInstalled(moduleType) && !InstalledModules.Any(module => ModuleLoader.GetRequirements(module.Type).Contains(moduleType));
		public bool CanUninstall<T>() where T : BaseModule => CanUninstall(ModuleLoader.ModuleType<T>());
		#endregion
	}
}