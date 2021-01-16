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

		public override string EquipTexture => ModularTools.TexturePath + "Armor/ModularArmor";

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
			ArmorIDs.Body.Sets.UsesNewFramingCode[Item.bodySlot] = true;
			ArmorIDs.Body.Sets.showsShouldersWhileJumping[Item.bodySlot] = true;

			ModularItemTags.Armor.Set(Type, true);
		}

		public override void SetDefaults()
		{
			Item.width = 26;
			Item.height = 20;
			Item.value = 10000;
			Item.rare = ItemRarityID.Cyan;
			Item.defense = 1;
		}

		public override void UpdateEquip(Player player)
		{
			if (EnergyStorage.Energy > 0) Lighting.AddLight((int)(player.Center.X / 16f), (int)(player.Center.Y / 16f), 0f, 0.1f, 0.3f);

			foreach (BaseModule module in InstalledModules) module.OnUpdate(this, player);
		}
	}
}