using ModularTools.Core;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ModularTools.Content.Items.Tools
{
	// player <-> armor <-> environment
	
	public class ModularPickaxe : ModularItem
	{
		public override string Texture => ModularTools.AssetPath + "Textures/Items/ModularPickaxe";

		public override void SetDefaults()
		{
			item.damage = 20;
			item.DamageType = DamageClass.Melee;
			item.width = 40;
			item.height = 40;
			item.useTime = 10;
			item.useAnimation = 10;
			item.useStyle = ItemUseStyleID.Swing;
			item.knockBack = 6;
			item.value = Item.buyPrice(gold: 1);
			item.rare = ItemRarityID.Green;
			item.UseSound = SoundID.Item1;
			item.autoReuse = false;

			item.pick = 40;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.DirtBlock)
				.Register();
		}

		public override void UpdateInventory(Player player)
		{
			foreach (BaseModule module in InstalledModules) module.OnUpdate(this, player);
		}

		public override bool CanUseItem(Player player) => true;

		public override bool UseItem(Player player)
		{
			foreach (int module in ModuleLoader.validModulesForItem[Type])
			{
				Main.NewText(ModuleLoader.modules[module].DisplayName.GetDefault());
			}

			return true;
		}
	}
}