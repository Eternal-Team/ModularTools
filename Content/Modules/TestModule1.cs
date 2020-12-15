using ModularTools.Content.Items.Tools;
using ModularTools.Core;
using Terraria.ID;
using Terraria.ModLoader.IO;

namespace ModularTools.Content.Modules
{
	public class TestModule1 : BaseModule
	{
		public override string Texture => BaseLibrary.BaseLibrary.PlaceholderTexture;

		public int testInt;

		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("This is a test1 module");

			AddRequiredModule<CopperPlating>();

			AddValidModularItem<ModularPickaxe>();
		}

		public override void AddRecipes()
		{
			Create()
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