using System.Collections.Generic;
using Terraria.ModLoader;

namespace ModularTools.Core
{
	internal static class ModuleLoader
	{
		internal static List<BaseModule> modules = new List<BaseModule>();

		internal static Dictionary<int, List<int>> validItemsForModule = new Dictionary<int, List<int>>();
		internal static Dictionary<int, List<int>> validModulesForItem = new Dictionary<int, List<int>>();
		
		internal static Dictionary<int, List<int>> requirements = new Dictionary<int, List<int>>();

		internal static int NextTypeID;

		internal static void RegisterModule(BaseModule module)
		{
			module.Type = NextTypeID++;
			modules.Add(module);

			validItemsForModule.Add(module.Type, new List<int>());
			
			requirements.Add(module.Type, new List<int>());
		}

		public static int ModuleType<T>() where T : ModItem => ModContent.GetInstance<T>()?.Type ?? 0;

		public static T CreateInstance<T>() where T : BaseModule
		{
			T instance = (T)ModContent.GetInstance<T>().Clone();
			instance.SetDefaults();
			return instance;
		}
	}
}