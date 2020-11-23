using ModularTools.Core;
using Terraria.ID;
using Terraria.ModLoader.IO;

namespace ModularTools.Content
{
	public class TestModule : BaseModule
	{
		public int testInt;

		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("This is a test module");
		}

		public override void AddRecipes()
		{
			ModuleRecipe.Create(Mod,this)
				.AddIngredient(ItemID.CopperBar, 10)
				.Register();
		}

		public override TagCompound Save()
		{
			return new TagCompound
			{
				["TestInt"] = testInt
			};
		}

		public override void Load(TagCompound tag)
		{
			testInt = tag.GetInt("TestInt");
		}
	}
}