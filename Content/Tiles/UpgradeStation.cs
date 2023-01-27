using Microsoft.Xna.Framework;
using ModularTools.UI;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace ModularTools.Content.Tiles;

public class UpgradeStation : ModTile
{
	public override string Texture => ModularTools.TexturePath + "Tiles/UpgradeStation";

	public override void SetStaticDefaults()
	{
		Main.tileSolidTop[Type] = false;
		Main.tileFrameImportant[Type] = true;
		Main.tileNoAttach[Type] = true;
		Main.tileLavaDeath[Type] = false;
		TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
		TileObjectData.newTile.Height = 3;
		TileObjectData.newTile.Origin = new Point16(0, 2);
		TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16 };
		// TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(ModContent.GetInstance<TileEntities.QEChest>().Hook_AfterPlacement, -1, 0, false);
		TileObjectData.addTile(Type);
		// disableSmartCursor = true;

		ModTranslation name = CreateMapEntryName();
		AddMapEntry(Color.Purple, name);
	}

	public override bool RightClick(int i, int j)
	{
		UpgradeStationUISystem.Instance.HandleUI();

		return true;
	}
}