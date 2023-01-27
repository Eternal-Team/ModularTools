using ModularTools.Core;
using Terraria.ModLoader;

namespace ModularTools;
// TODO: !!!!
// fully working module system
// tools
// resources, machines
// armor

public class ModularTools : Mod
{
	public const string AssetPath = "ModularTools/Assets/";
	public const string TexturePath = "ModularTools/Assets/Textures/";

	public static ModularTools Instance => ModContent.GetInstance<ModularTools>();

	public override void Load()
	{
		Hooking.Initialize();

		ModuleSerializer.Load();
	}
}

public class ModularToolsSystem : ModSystem
{
	public override void AddRecipes()
	{
		ModuleLoader.AddRecipes();
	}
}