using System.Collections.Generic;
using EnergyLibrary;
using ModularTools.Core;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ModularTools.Content.Items.Armor
{
	[AutoloadEquip(EquipType.Head)]
	public class ModularHelmet : ModularItem
	{
		public override string Texture => ModularTools.TexturePath + "Items/ModularHelmet";

		public override string ArmorTexture => ModularTools.TexturePath + "Armor/ModularArmor_Head";

		public float insulation = 1000f;

		public override void OnCreate(ItemCreationContext context)
		{
			InstalledModules = new List<BaseModule>();
			EnergyStorage = new EnergyStorage(0);
			HeatStorage = new HeatStorage
			{
				Capacity = 2f * 500f,
				Temperature = Utility.ToKelvin(37),
				Area = 0.3f,
				TransferCoefficient = 5f
			};
		}

		public override void SetStaticDefaults()
		{
			ArmorIDs.Head.Sets.PreventBeardDraw[item.headSlot] = true;
			ArmorIDs.Head.Sets.UseAltFaceHeadDraw[item.headSlot] = true;
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

		public override bool IsArmorSet(Item head, Item body, Item legs) => body.type == ModContent.ItemType<ModularChestplate>() && legs.type == ModContent.ItemType<ModularLeggings>();

		public override void UpdateEquip(Player player)
		{
			foreach (BaseModule module in InstalledModules) module.OnUpdate(this, player);
		}
	}
}