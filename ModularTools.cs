using ModularTools.Core;
using Terraria.ModLoader;

namespace ModularTools
{
	public class ModularTools : Mod
	{
		public const string AssetPath = "ModularTools/Assets/";

		public static ModularTools Instance => ModContent.GetInstance<ModularTools>();

		public override void Load()
		{
			ModuleSerializer.Load();
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