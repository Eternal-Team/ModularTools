using BaseLibrary.UI;
using Microsoft.Xna.Framework;
using ModularTools.Core;
using Terraria.ModLoader;

namespace ModularTools
{
	public class UIModule : BaseElement
	{
		private UIPanel mPanel;
		public BaseModule module;

		public Color Color
		{
			get => mPanel.Settings.BorderColor;
			set => mPanel.Settings.BorderColor = value;
		}

		public UIModule(BaseModule module)
		{
			this.module = module;
			
			mPanel = new UIPanel
			{
				Width = { Percent = 100 },
				Height = { Percent = 100 }
			};
			Add(mPanel);

			UITexture image = new UITexture(ModContent.GetTexture(module.Texture).Value)
			{
				Width = { Pixels = 48 },
				Height = { Pixels = 48 },
				Settings =
				{
					ScaleMode = ScaleMode.Zoom,
					ImageX = { Percent = 50 },
					ImageY = { Percent = 50 }
				}
			};
			mPanel.Add(image);

			UIText text = new UIText(module.DisplayName.GetDefault() + "\n" + module.Tooltip.GetDefault())
			{
				Width = { Pixels = -56, Percent = 100 },
				Height = { Percent = 100 },
				X = { Pixels = 56 }
			};
			mPanel.Add(text);
		}
	}
}