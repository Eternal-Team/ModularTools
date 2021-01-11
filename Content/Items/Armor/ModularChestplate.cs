using System.Collections.Generic;
using EnergyLibrary;
using ModularTools.Core;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ModularTools.Content.Items.Armor
{
	[AutoloadEquip(EquipType.Body)]
	public class ModularChestplate : ModularItem
	{
		public override string Texture => ModularTools.TexturePath + "Items/ModularChestplate";

		public override string ArmorTexture => ModularTools.TexturePath + "Armor/ModularArmor";

		public float insulation = 1000f;

		public override void OnCreate(ItemCreationContext context)
		{
			InstalledModules = new List<BaseModule>();
			EnergyStorage = new EnergyStorage(0);
			HeatStorage = new HeatStorage
			{
				Capacity = 10f * 500f,
				Temperature = Utility.ToKelvin(37),
				Area = 1f,
				TransferCoefficient = 5f
			};
		}

		public override void SetStaticDefaults()
		{
			ArmorIDs.Body.Sets.UsesNewFramingCode[item.bodySlot] = true;
			ArmorIDs.Body.Sets.showsShouldersWhileJumping[item.bodySlot] = true;
		}

		public override void SetDefaults()
		{
			item.width = 26;
			item.height = 20;
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