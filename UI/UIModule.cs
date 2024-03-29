﻿using BaseLibrary.UI;
using BaseLibrary.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ModularTools.Core;
using Terraria.ModLoader;

namespace ModularTools.UI;

public class UIModule : BaseElement, IGridElement
{
	private UIPanel mPanel;
	public BaseModule Module { get; }

	public Color Color
	{
		get => mPanel.Settings.BorderColor;
		set => mPanel.Settings.BorderColor = value;
	}

	public UIModule(BaseModule module)
	{
		Module = module;

		mPanel = new UIPanel
		{
			Width = { Percent = 100 },
			Height = { Percent = 100 }
		};
		Add(mPanel);

		UITexture image = new UITexture(ModContent.Request<Texture2D>(module.Texture))
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

		UIText text = new UIText(module.DisplayName.Get())
		{
			Width = { Pixels = -56, Percent = 100 },
			Height = { Percent = 100 },
			X = { Pixels = 56 }
		};
		mPanel.Add(text);
	}

	public bool Selected { get; set; }
}