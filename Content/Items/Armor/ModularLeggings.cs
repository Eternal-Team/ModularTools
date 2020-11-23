using ModularTools.Core;
using Terraria.ID;
using Terraria.ModLoader;

namespace ModularTools.Content.Items.Armor
{
	[AutoloadEquip(EquipType.Legs)]
	public class ModularLeggings : ModularItem
	{
		public override string Texture => ModularTools.AssetPath + "Textures/Armor/ModularLeggings";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Modular Leggings");
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