using ModularTools.Core;
using Terraria.ModLoader;

namespace ModularTools
{
	public class ModularTools : Mod
	{
		public const string AssetPath = "ModularTools/Assets/";

		public static ModularTools Instance => ModContent.GetInstance<ModularTools>();

		internal ModHotKey hotKey;

		public override void Load()
		{
			ModuleSerializer.Load();

			hotKey = RegisterHotKey("Open Upgrade UI", "K");
		}

		public override void AddRecipes()
		{
			foreach (BaseModule module in ModuleLoader.modules)
			{
				module.AddRecipes();
			}
		}
	}
}