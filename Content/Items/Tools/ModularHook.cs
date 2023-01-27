// using ModularTools.Core;
// using Terraria;
// using Terraria.ID;
// using Terraria.ModLoader;
//
// namespace ModularTools.Content.Items.Tools;
//
// public class ModularHook : ModularItem
// {
// 	public override string Texture => ModularTools.TexturePath + "Items/ModularHook";
//
// 	public override void SetDefaults()
// 	{
// 		Item.noUseGraphic = true;
// 		Item.damage = 0;
// 		Item.knockBack = 7f;
// 		Item.useStyle = 5;
// 		Item.width = 18;
// 		Item.height = 28;
// 		Item.UseSound = SoundID.Item1;
// 		Item.useAnimation = 20;
// 		Item.useTime = 20;
// 		Item.rare = 1;
// 		Item.noMelee = true;
// 		Item.value = 20000;
// 		Item.shootSpeed = 18f;
// 		Item.shoot = ModContent.ProjectileType<Projectiles.ModularHook>();
// 	}
//
// 	public override void UpdateInventory(Player player)
// 	{
// 		foreach (BaseModule module in InstalledModules) module.OnUpdate(this, player);
// 	}
// }