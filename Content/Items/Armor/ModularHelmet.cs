using ModularTools.Core;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ModularTools.Content.Items.Armor
{
	[AutoloadEquip(EquipType.Head)]
	public class ModularHelmet : ModularItem
	{
		public override string Texture => ModularTools.AssetPath + "Textures/Armor/ModularHelmet";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Modular Helmet");
		}

		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = 10000;
			item.rare = ItemRarityID.Cyan;
			item.defense = 1;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs) => body.type == ModContent.ItemType<ModularChestplate>() && legs.type == ModContent.ItemType<ModularLeggings>();
	}
}