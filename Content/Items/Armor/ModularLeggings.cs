using System.Collections.Generic;
using EnergyLibrary;
using ModularTools.Core;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ModularTools.Content.Items.Armor
{
	[AutoloadEquip(EquipType.Legs)]
	public class ModularLeggings : ModularItem
	{
		public override string Texture => ModularTools.AssetPath + "Textures/Armor/ModularLeggings";

		public float insulation = 1000f;

		public override void OnCreate(ItemCreationContext context)
		{
			InstalledModules = new List<BaseModule>();
			EnergyStorage = new EnergyStorage(0);
			HeatStorage = new HeatStorage
			{
				Capacity = 7f * 500f,
				Temperature = Utility.ToKelvin(37),
				Area = 0.6f,
				TransferCoefficient = 5f
			};
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Modular Leggings");
		}

		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 12;
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