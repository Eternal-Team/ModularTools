﻿using System.Collections.Generic;
using System.Linq;
using BaseLibrary;
using EnergyLibrary;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ModularTools.Core
{
	public abstract class ModularItem : BaseItem, IEnergyStorage
	{
		public List<BaseModule> InstalledModules;
		public EnergyStorage EnergyStorage;

		public sealed override void AutoStaticDefaults()
		{
			base.AutoStaticDefaults();

			ModuleLoader.validModulesForItem[Type] = new List<int>();
		}

		public override void OnCreate(ItemCreationContext context)
		{
			InstalledModules = new List<BaseModule>();
			EnergyStorage = new EnergyStorage(0);
		}

		public override ModItem Clone(Item item)
		{
			ModularItem modularItem = (ModularItem)base.Clone(item);
			modularItem.InstalledModules = new List<BaseModule>(InstalledModules);
			modularItem.EnergyStorage = EnergyStorage.Clone();
			return modularItem;
		}

		public override TagCompound Save() => new TagCompound
		{
			["Modules"] = InstalledModules,
			["Energy"] = EnergyStorage.Save()
		};

		public override void Load(TagCompound tag)
		{
			InstalledModules = tag.GetList<BaseModule>("Modules").ToList();
			EnergyStorage.Load(tag.GetCompound("Energy"));
		}

		public EnergyStorage GetEnergyStorage() => EnergyStorage;
	}
}