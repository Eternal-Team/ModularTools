using ModularTools.Core;
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
	}
}