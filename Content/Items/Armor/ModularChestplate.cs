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
		public override string Texture => ModularTools.AssetPath + "Textures/Armor/ModularChestplate";
		public override string ArmTexture => ModularTools.AssetPath + "Textures/Armor/ModularChestplate_Arms";
		public override string FemaleTexture => ModularTools.AssetPath + "Textures/Armor/ModularChestplate_Female";

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