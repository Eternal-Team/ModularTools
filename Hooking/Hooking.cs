using System;
using System.Reflection;
using MonoMod.Cil;
using MonoMod.RuntimeDetour.HookGen;
using Terraria;

namespace ModularTools
{
	internal static partial class Hooking
	{
		public static void Initialize()
		{
			var nestedType = typeof(WorldGen).GetNestedType("<>c__DisplayClass343_0", BindingFlags.NonPublic | BindingFlags.Instance);
			var methodInfo = nestedType.GetMethod("<GenerateWorld>b__12", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

			HookEndpointManager.Modify(methodInfo, new Action<ILContext>(GenerationPass_SmallHoles));
		}
	}
}