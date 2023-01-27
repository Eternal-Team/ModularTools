using ModularTools.Content.Items.Ingredients;
using ModularTools.Core;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace ModularTools;

public partial class MTPlayer : ModPlayer
{
	// public HeatStorage PlayerHeat = new()
	// {
	// 	Capacity = 80f * 3500f,
	// 	Temperature = Utility.ToKelvin(37),
	// 	Area = 1.9f,
	// 	ThermalConductivity = 30f
	// };
	//
	// public override void PreUpdate()
	// {
	// 	UpdateEnvironment();
	// }

	public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
	{
		Player.QuickSpawnItem(new EntitySource_Death(Player), ModContent.ItemType<Brain>());
	}
}