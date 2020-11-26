using ModularTools.Core;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ModularTools.Content.Items.Armor
{
	[AutoloadEquip(EquipType.Body)]
	public class ModularChestplate : ModularItem
	{
		public override string Texture => ModularTools.AssetPath + "Textures/Armor/ModularChestplate";
		public override string ArmTexture => ModularTools.AssetPath + "Textures/Armor/ModularChestplate_Arms";
		public override string FemaleTexture => ModularTools.AssetPath + "Textures/Armor/ModularChestplate_Female";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Modular Chestplate");
		}

		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = 10000;
			item.rare = ItemRarityID.Cyan;
			item.defense = 1;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.DirtBlock)
				.Register();
		}
		
		public override void UpdateEquip(Player player)
		{
			foreach (BaseModule module in InstalledModules) module.OnUpdate(this, player);
		}
	}
}