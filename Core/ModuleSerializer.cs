using System;
using BaseLibrary.Utility;
using MonoMod.RuntimeDetour.HookGen;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ModularTools.Core
{
	[Autoload(false)]
	public class ModuleSerializer : TagSerializer<BaseModule, TagCompound>
	{
		private static ModuleSerializer Instance = new ModuleSerializer();

		private delegate bool orig_TryGetSerializer(Type type, out TagSerializer serializer);

		private delegate bool hook_TryGetSerializer(orig_TryGetSerializer orig, Type type, out TagSerializer serializer);

		internal static void Load()
		{
			Instance = new ModuleSerializer();

			HookEndpointManager.Add<hook_TryGetSerializer>(typeof(TagSerializer).GetMethod("TryGetSerializer", ReflectionUtility.DefaultFlags_Static), (hook_TryGetSerializer)((orig_TryGetSerializer orig, Type type, out TagSerializer serializer) =>
			{
				if (type == typeof(BaseModule) || type.IsSubclassOf(typeof(BaseModule)))
				{
					serializer = Instance;
					return true;
				}

				return orig(type, out serializer);
			}));
		}

		public override TagCompound Serialize(BaseModule value) => new TagCompound
		{
			["Mod"] = value.Mod.Name,
			["Name"] = value.Name,
			["Data"] = value.Save()
		};

		public override BaseModule Deserialize(TagCompound tag)
		{
			if (ModContent.TryFind(tag.GetString("Mod"), tag.GetString("Name"), out BaseModule modItem))
			{
				BaseModule module = modItem.Clone();
				module.Load(tag.GetCompound("Data"));
				return module;
			}

			// todo: UnloadedModule
			throw new Exception();
		}
	}
}