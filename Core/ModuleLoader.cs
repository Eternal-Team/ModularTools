using System;
using System.Collections.Generic;
using ModularTools.DataTags;
using Terraria.ModLoader;

namespace ModularTools.Core;

internal static class ModuleLoader
{
	#region Loader
	private static List<BaseModule> modules = new();

	internal static int NextTypeID;

	public static int Count => NextTypeID;

	internal static void RegisterModule(BaseModule module)
	{
		module.Type = NextTypeID++;
		modules.Add(module);

		// validItemsForModule.Add(module.Type, new List<int>());

		requiredModules.Add(module.Type, new List<int>());
		incompatibleModules.Add(module.Type, new List<int>());
		incompatibleGroups.Add(module.Type, new List<ModuleGroup>());
	}

	public static int ModuleType<T>() where T : BaseModule => ModContent.GetInstance<T>()?.Type ?? -1;

	public static BaseModule GetModule(int type) => modules[type];

	internal static void AddRecipes()
	{
		foreach (BaseModule module in modules)
		{
			module.AddRecipes();
		}
	}
	#endregion

	// private static Dictionary<int, List<int>> validItemsForModule = new();

	private static Dictionary<int, List<int>> validModulesForItem = new();

	private static Dictionary<int, List<int>> requiredModules = new();
	private static Dictionary<int, List<int>> incompatibleModules = new();
	private static Dictionary<int, List<ModuleGroup>> incompatibleGroups = new();

	public static void RegisterModularItem(ModularItem item)
	{
		validModulesForItem.TryAdd(item.Type, new List<int>());
	}

	public static List<int> GetRequiredModules(int type)
	{
		if (type < 0 || type > Count) throw new Exception();
		return requiredModules[type];
	}

	public static List<int> GetRequiredModules<T>() where T : BaseModule => GetRequiredModules(ModuleType<T>());

	public static List<int> GetIncompatibleModules(int type)
	{
		if (type < 0 || type > Count) throw new Exception();

		return incompatibleModules[type];
	}

	public static List<int> GetIncompatibleModules<T>() where T : BaseModule => GetIncompatibleModules(ModuleType<T>());

	public static List<ModuleGroup> GetIncompatibleGroups(int type)
	{
		if (type < 0 || type > Count) throw new Exception();

		return incompatibleGroups[type];
	}

	public static List<ModuleGroup> GetIncompatibleGroups<T>() where T : BaseModule => GetIncompatibleGroups(ModuleType<T>());

	public static List<int> GetValidModulesForItem(int type)
	{
		if (type < 0 || type > ItemLoader.ItemCount) throw new Exception();

		return validModulesForItem[type];
	}
}