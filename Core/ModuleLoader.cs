using System.Collections.Generic;
using System.Linq;
using Terraria.ModLoader;
using Terraria.ModLoader.Tags;

namespace ModularTools.Core
{
	internal static class ModuleLoader
	{
		internal static List<BaseModule> modules = new List<BaseModule>();

		internal static Dictionary<int, List<int>> validItemsForModule = new Dictionary<int, List<int>>();
		internal static Dictionary<int, List<int>> validModulesForItem = new Dictionary<int, List<int>>();

		internal static Dictionary<int, List<int>> requirements = new Dictionary<int, List<int>>();

		internal static Dictionary<int, List<int>> blacklistTypes = new Dictionary<int, List<int>>();
		internal static Dictionary<int, List<TagData>> blacklistGroups = new Dictionary<int, List<TagData>>();

		internal static int NextTypeID;

		public static int Count => NextTypeID;

		internal static void RegisterModule(BaseModule module)
		{
			module.Type = NextTypeID++;
			modules.Add(module);

			validItemsForModule.Add(module.Type, new List<int>());

			requirements.Add(module.Type, new List<int>());
			blacklistTypes.Add(module.Type, new List<int>());
			blacklistGroups.Add(module.Type, new List<TagData>());
		}

		public static int ModuleType<T>() where T : BaseModule => ModContent.GetInstance<T>()?.Type ?? -1;

		public static BaseModule GetModule(int type) => modules[type];

		public static IEnumerable<int> GetRequirements<T>() where T : BaseModule => requirements[ModuleType<T>()];

		public static IEnumerable<int> GetRequirements(int type) => requirements[type];

		public static IEnumerable<int> GetIncompatibleModules<T>() where T : BaseModule
		{
			int type = ModuleType<T>();

			foreach (int module in blacklistTypes[type]) yield return module;
			foreach (int module in blacklistGroups[type].SelectMany(tag => tag.GetEntries()))
			{
				if (module != type) yield return module;
			}
		}

		public static IEnumerable<int> GetIncompatibleModules(int type)
		{
			foreach (int module in blacklistTypes[type]) yield return module;
			foreach (int module in blacklistGroups[type].SelectMany(tag => tag.GetEntries()))
			{
				if (module != type) yield return module;
			}
		}

		public static T CreateInstance<T>() where T : BaseModule
		{
			T instance = (T)ModContent.GetInstance<T>().Clone();
			instance.SetDefaults();
			return instance;
		}
	}
}