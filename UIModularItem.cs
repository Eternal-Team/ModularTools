using BaseLibrary.UI;
using BaseLibrary.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ModularTools.Core;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.UI;

namespace ModularTools
{
	public class UIModularItem : BaseElement
	{
		public ModularItem modularItem;
		private static Texture2D bg;
		public bool selected;

		public UIModularItem(ModularItem modularItem)
		{
			if (bg == null) bg = DrawingUtility.GetTexturePremultiplied(ModularTools.AssetPath + "Textures/UI/Upgrade/ModularItemBackground");
			this.modularItem = modularItem;
		}

		private Transition fade = new Transition(0f, 1f, 0.05f);

		protected override void Draw(SpriteBatch spriteBatch)
		{
			fade.Update(IsMouseHovering || selected);

			spriteBatch.Draw(bg, Dimensions, Color.White * fade);

			Item item = modularItem.item;
			Main.instance.LoadItem(item.type);
			Texture2D itemTexture = TextureAssets.Item[item.type].Value;

			Rectangle rect = Main.itemAnimations[item.type] != null ? Main.itemAnimations[item.type].GetFrame(itemTexture) : itemTexture.Frame();
			Color newColor = Color.White;
			float pulseScale = 1f;
			ItemSlot.GetItemLight(ref newColor, ref pulseScale, item);
			int height = rect.Height;
			int width = rect.Width;
			float drawScale = 1f;

			float availableWidth = InnerDimensions.Width;
			if (width > availableWidth || height > availableWidth)
			{
				if (width > height) drawScale = availableWidth / width;
				else drawScale = availableWidth / height;
			}

			Vector2 position = Dimensions.Center();
			Vector2 origin = rect.Size() * 0.5f;

			if (ItemLoader.PreDrawInInventory(item, spriteBatch, position - rect.Size() * 0.5f * drawScale, rect, item.GetAlpha(newColor), item.GetColor(Color.White), origin, drawScale * pulseScale))
			{
				spriteBatch.Draw(itemTexture, position, rect, item.GetAlpha(newColor), 0f, origin, drawScale * pulseScale, SpriteEffects.None, 0f);
				if (item.color != Color.Transparent) spriteBatch.Draw(itemTexture, position, rect, item.GetColor(Color.White), 0f, origin, drawScale * pulseScale, SpriteEffects.None, 0f);
			}

			ItemLoader.PostDrawInInventory(item, spriteBatch, position - rect.Size() * 0.5f * drawScale, rect, item.GetAlpha(newColor), item.GetColor(Color.White), origin, drawScale * pulseScale);
		}
	}
}