// using System.Collections.Generic;
// using EnergyLibrary;
// using ModularTools.Core;
// using Terraria;
// using Terraria.ID;
// using Terraria.ModLoader;
//
// namespace ModularTools.Content.Items.Armor;
//
// [AutoloadEquip(EquipType.Head)]
// public class ModularHelmet : ModularItem
// {
// 	public override string Texture => ModularTools.TexturePath + "Items/ModularHelmet";
//
// 	// public override string EquipTexture => ModularTools.TexturePath + "Armor/ModularHelmet";
//
// 	public float Insulation = 1000f;
//
// 	public override void OnCreate(ItemCreationContext context)
// 	{
// 		InstalledModules = new List<BaseModule>();
// 		EnergyStorage = new EnergyStorage(0);
// 		HeatStorage = new HeatStorage
// 		{
// 			Capacity = 2f * 500f,
// 			Temperature = Utility.ToKelvin(37),
// 			Area = 0.3f,
// 			ThermalConductivity = 5f
// 		};
// 	}
//
// 	public override void SetStaticDefaults()
// 	{
// 		ArmorIDs.Head.Sets.PreventBeardDraw[Item.headSlot] = true;
// 		ArmorIDs.Head.Sets.UseAltFaceHeadDraw[Item.headSlot] = true;
//
// 		// ModularItemTags.Armor.Set(Type, true);
// 	}
//
// 	public override void SetDefaults()
// 	{
// 		Item.width = 18;
// 		Item.height = 18;
// 		Item.value = 10000;
// 		Item.rare = ItemRarityID.Cyan;
// 		Item.defense = 1;
// 	}
//
// 	public override bool IsArmorSet(Item head, Item body, Item legs) => body.type == ModContent.ItemType<ModularChestplate>() && legs.type == ModContent.ItemType<ModularLeggings>();
//
// 	public override void UpdateEquip(Player player)
// 	{
// 		if (EnergyStorage.Energy > 0) Lighting.AddLight((int)(player.Center.X / 16f), (int)(player.Center.Y / 16f), 0f, 0.1f, 0.3f);
//
// 		foreach (BaseModule module in InstalledModules) module.OnUpdate(this, player);
// 	}
// }