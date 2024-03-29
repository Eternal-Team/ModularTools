﻿using System;
using System.Collections.Generic;
using BaseLibrary.Utility;
using ModularTools.Core;
using Terraria.ModLoader;

namespace ModularTools.DataTags;

public class ModuleDataGroup : DataTagGroup
{
	public override int TypeCount => ModuleLoader.Count;
}

/// <summary>
/// Used to register common stats of modules
/// </summary>
public static class ModuleData
{
	// add a switch for cumulative (defense)/max (potion prolongation)
	public static readonly DataTagData<int> Defense = Core.DataTags.Get<ModuleDataGroup, int>(nameof(Defense)).AddLocalization(l => $"Provides {l} defense").AddTexture(ModularTools.TexturePath + "Projectiles/ModularHook");

	public static readonly DataTagData<long> HeatCapacity = Core.DataTags.Get<ModuleDataGroup, long>(nameof(HeatCapacity)).AddLocalization(l => $"Heat Capacity: {TextUtility.ToSI(l, "#.##")}J");
	public static readonly DataTagData<ulong> HeatTransfer = Core.DataTags.Get<ModuleDataGroup, ulong>(nameof(HeatTransfer)).AddLocalization(l => $"Heat Transfer: {TextUtility.ToSI(l, "#.##")}W");

	public static readonly DataTagData<long> EnergyCapacity = Core.DataTags.Get<ModuleDataGroup, long>(nameof(EnergyCapacity)).AddLocalization(l => $"Energy Capacity: {TextUtility.ToSI(l, "#.##")}J");
	public static readonly DataTagData<ulong> EnergyTransfer = Core.DataTags.Get<ModuleDataGroup, ulong>(nameof(EnergyTransfer)).AddLocalization(l => $"Energy Transfer: {TextUtility.ToSI(l, "#.##")}W");
}

/// <summary>
/// Used to sort modules into groups
/// </summary>
public static class ModuleTags
{
	private static ModuleGroup Get(string name) => ModContent.GetInstance<GroupSystem>().Get(name);

	public static readonly ModuleGroup Plating = Get(nameof(Plating));
	public static readonly ModuleGroup Battery = Get(nameof(Battery));
}

public class GroupSystem : ModType
{
	internal Dictionary<string, ModuleGroup> TagNameToData = new(StringComparer.InvariantCultureIgnoreCase);

	protected sealed override void Register() => ModTypeLookup<GroupSystem>.Register(this);

	public sealed override void Unload()
	{
		TagNameToData.Clear();

		TagNameToData = null;
	}

	public ModuleGroup Get(string name)
	{
		if (!TagNameToData.TryGetValue(name, out var data))
		{
			TagNameToData[name] = data = new ModuleGroup(name);
		}

		return data;
	}
}

public class ModuleGroup
{
	public readonly ModTranslation DisplayName;
	private readonly bool[] idToValue = new bool[ModuleLoader.Count];
	private readonly List<int> entryList = new();

	public ModuleGroup(string name)
	{
		DisplayName = LocalizationLoader.GetOrCreateTranslation("Mods.ModularTools.ModuleGroup." + name);
	}

	public List<int> GetEntries() => entryList;

	public void Set(BaseModule module)
	{
		idToValue[module.Type] = true;
		if (!entryList.Contains(module.Type)) entryList.Add(module.Type);
	}
}