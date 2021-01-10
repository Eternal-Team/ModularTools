using System;
using System.Collections.Generic;
using System.Linq;
using BaseLibrary;
using BaseLibrary.Utility;
using EnergyLibrary;
using Microsoft.Xna.Framework;
using ModularTools.Content.Items.Armor;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ModularTools.Core
{
	public abstract class ModularItem : BaseItem, IEnergyStorage, IHeatStorage
	{
		public static class Group
		{
			public static readonly List<int> Armor = new List<int> { ModContent.ItemType<ModularHelmet>(), ModContent.ItemType<ModularChestplate>(), ModContent.ItemType<ModularLeggings>() };
		}

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

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			tooltips.Add(new TooltipLine(Mod, "MT:Energy", TextUtility.WithColor($"Energy: {EnergyStorage.Energy}/{EnergyStorage.Capacity} J", new Color(28, 218, 232))));
			tooltips.Add(new TooltipLine(Mod, "MT:Heat", TextUtility.WithColor($"Temperature: {Math.Round(HeatStorage.Temperature)}", new Color(255, 91, 20)) + "\n" +
			                                             TextUtility.WithColor($"Transfering: {Math.Round(HeatStorage.TransferCoefficient * HeatStorage.Area, 1)} W", new Color(185, 255, 20))));
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

		public bool IsInstalled(int moduleType)
		{
			return InstalledModules.Any(module => module.Type == moduleType);
		}
	}
}