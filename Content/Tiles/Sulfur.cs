using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ModularTools.Content.Tiles
{
	public class Sulfur : ModTile
	{
		public override string Texture => ModularTools.TexturePath + "Tiles/Sulfur";

		public override void SetDefaults()
		{
			Main.tileShine[Type] = 300;
			Main.tileNoFail[Type] = true;
			Main.tileLighted[Type] = true;
			Main.tileFrameImportant[Type] = true;

			AddMapEntry(Color.Yellow);
		}

		public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
		{
			Item.NewItem(i * 16, j * 16, 16, 16, ModContent.ItemType<Items.Ingredients.Sulfur>(), Main.rand.Next(1, 4));
		}

		public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
		{
			Tile tile = Main.tile[i, j];
			int style = WorldGen._genRand.Next(4);
			tile.frameX = (short)(style * 18);

			Tile tileTop = Main.tile[i, j - 1];
			Tile tileBottom = Main.tile[i, j + 1];
			Tile tileLeft = Main.tile[i - 1, j];
			Tile tileRight = Main.tile[i + 1, j];
			int num23 = -1;
			int num24 = -1;
			int num25 = -1;
			int num26 = -1;
			if (tileTop != null && !tileTop.IsActuated && tileTop.Slope != SlopeID.HalfBrick && tileTop.Slope != SlopeID.SlopeDownLeft && tileTop.Slope != SlopeID.SlopeDownRight) num24 = tileTop.type;
			if (tileBottom != null && !tileBottom.IsActuated && tileBottom.Slope != SlopeID.HalfBrick && tileBottom.Slope != SlopeID.SlopeUpLeft && tileBottom.Slope != SlopeID.SlopeUpRight) num23 = tileBottom.type;
			if (tileLeft != null && !tileLeft.IsActuated) num25 = tileLeft.type;
			if (tileRight != null && !tileRight.IsActuated) num26 = tileRight.type;

			if (num23 >= 0 && Main.tileSolid[num23] && !Main.tileSolidTop[num23]) tile.frameY = 0;
			else if (num25 >= 0 && Main.tileSolid[num25] && !Main.tileSolidTop[num25]) tile.frameY = 54;
			else if (num26 >= 0 && Main.tileSolid[num26] && !Main.tileSolidTop[num26]) tile.frameY = 36;
			else if (num24 >= 0 && Main.tileSolid[num24] && !Main.tileSolidTop[num24]) tile.frameY = 18;
			else WorldGen.KillTile(i, j);

			return false;
		}

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = 0.3529412f;
			g = 0.3529412f;
			b = 0f;
		}
	}
}