using System.Collections.Generic;
using BaseLibrary;
using Microsoft.Xna.Framework;
using ModularTools.Core;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace ModularTools
{
	public class UpgradeStationUI : UIState
	{
		public bool Visible = false;
		private UIPanel modulePanel;

		public override void OnInitialize()
		{
			RemoveAllChildren();

			DraggableUIPanel panel = new DraggableUIPanel();
			panel.Width.Set(1000f, 0f);
			panel.Height.Set(550f, 0f);
			panel.HAlign = panel.VAlign = 0.5f;
			panel.SetPadding(8);
			Append(panel);

			modulePanel = new UIPanel();
			modulePanel.Width.Set(1000f-58f, 0f);
			modulePanel.Height.Set(0f, 100f);
			modulePanel.Left.Set(58f,0f);;
			modulePanel.SetPadding(8);
			panel.Append(modulePanel);

			int y = 0;
			foreach (Item item in Main.LocalPlayer.inventory)
			{
				if (item.IsAir) continue;

				if (item.modItem is ModularItem modularItem)
				{
					UIPanel mPanel = new UIPanel();
					mPanel.Width.Set(50f, 0f);
					mPanel.Height.Set(50f, 0f);
					mPanel.Top.Set(50f * y++, 0f);
					mPanel.SetPadding(8);
					mPanel.OnClick += (evt, element) => OpenItem(modularItem);
					panel.Append(mPanel);

					Main.instance.LoadItem(modularItem.Type);
					UIImage image = new UIImage(TextureAssets.Item[modularItem.Type]) { ScaleToFit = true };
					image.Width.Set(0f, 100f);
					image.Height.Set(0f, 100f);
					mPanel.Append(image);
				}
			}
		}

		private void OpenItem(ModularItem item)
		{
			modulePanel.RemoveAllChildren();

			int y = 0;
			foreach (int type in ModuleLoader.validModulesForItem[item.Type])
			{
				BaseModule module = ModuleLoader.modules[type];

				UIPanel mPanel = new UIPanel();
				mPanel.Width.Set(400f, 0f);
				mPanel.Height.Set(100f, 0f);
				mPanel.Top.Set(108f * y++, 0f);
				mPanel.SetPadding(8);
				// mPanel.OnClick += (evt, element) => OpenItem(modularItem);
				modulePanel.Append(mPanel);

				UIText text = new UIText(module.DisplayName.GetDefault() + "\n" + module.Tooltip.GetDefault());
				text.Width.Set(0f, 100f);
				text.Height.Set(0f, 100f);
				mPanel.Append(text);

				// Main.instance.LoadItem(modularItem.Type);
				// UIImage image = new UIImage(TextureAssets.Item[modularItem.Type]) { ScaleToFit = true };
				// image.Width.Set(0f, 100f);
				// image.Height.Set(0f, 100f);
				// mPanel.Append(image);
			}
		}
	}

	// todo: add upgrade station
	public class UpgradeStationUISystem : ModSystem
	{
		public static UpgradeStationUISystem Instance => ModContent.GetInstance<UpgradeStationUISystem>();

		public UpgradeStationUI upgradeState;
		private UserInterface upgradeUI;

		public override void Load()
		{
			if (!Main.dedServ)
			{
				upgradeState = new UpgradeStationUI();
				upgradeUI = new UserInterface();
				upgradeUI.SetState(upgradeState);
			}
		}

		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
		{
			int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
			if (mouseTextIndex != -1)
			{
				layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
					"ModularTools: UpgradeUI",
					delegate
					{
						if (upgradeState.Visible) upgradeUI.Draw(Main.spriteBatch, Main._drawInterfaceGameTime);
						return true;
					},
					InterfaceScaleType.UI)
				);
			}
		}

		public override void UpdateUI(GameTime gameTime)
		{
			if (upgradeState.Visible) upgradeUI.Update(gameTime);
		}
	}
}