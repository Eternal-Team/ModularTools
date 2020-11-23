using ModularTools.Content;
using ModularTools.Core;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ModularTools
{
	public class MTPlayer : ModPlayer
	{
		public override TagCompound Save()
		{
			TestModule module = ModuleLoader.CreateInstance<TestModule>();
			module.testInt = 420;
			
			return new TagCompound
			{
				["Test"] = module
			};
		}

		public override void Load(TagCompound tag)
		{
			TestModule module = tag.Get<TestModule>("Test");
		}
	}
}