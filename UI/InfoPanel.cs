using System.Collections.Generic;
using System.Linq;
using BaseLibrary.UI;
using BaseLibrary.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ModularTools.Core;
using ModularTools.DataTags;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Chat;

namespace ModularTools.UI;

internal class InfoPanel : UIPanel
{
	// todo: highlight if requirements are met/incompatibilities found
	#region Required & Incompatible
	private class UIModuleGroup : BaseElement
	{
		private ModuleGroup group;
		private UITexture texture;
		private UIText text;

		public UIModuleGroup(ModuleGroup group)
		{
			this.group = group;

			texture = new UITexture(null)
			{
				Height = { Percent = 100 },
				Width = { Percent = -1 },
				Settings =
				{
					ScaleMode = ScaleMode.Stretch
				}
			};
			Add(texture);

			text = new UIText("Group: " + group.DisplayName.Get())
			{
				Width = { Percent = 100 },
				Height = { Percent = 100 },
				Settings =
				{
					VerticalAlignment = VerticalAlignment.Center
				}
			};
			Add(text);
		}

		public override void Recalculate()
		{
			base.Recalculate();

			text.Width.Pixels = -texture.Dimensions.Width - 8;
			text.X.Pixels = texture.Dimensions.Width + 8;
			text.Recalculate();
		}

		private int currentIndex;
		private int timer;

		protected override void Draw(SpriteBatch spriteBatch)
		{
			var modules = group.GetEntries();
			if (modules is null || modules.Count == 0) return;

			BaseModule module = ModuleLoader.GetModule(modules[currentIndex]);
			texture.Texture = ModContent.Request<Texture2D>(module.Texture);
			texture.HoverText = module.DisplayName;

			if (++timer >= 15)
			{
				currentIndex++;
				if (currentIndex >= modules.Count) currentIndex = 0;
				timer = 0;
			}
		}
	}

	private class UIModule : BaseElement
	{
		private UITexture texture;
		private UIText text;

		public UIModule(BaseModule module)
		{
			texture = new UITexture(ModContent.Request<Texture2D>(module.Texture))
			{
				Height = { Percent = 100 },
				Width = { Percent = -1 },
				Settings =
				{
					ScaleMode = ScaleMode.Stretch
				}
			};
			Add(texture);

			text = new UIText(module.DisplayName)
			{
				Width = { Percent = 100 },
				Height = { Percent = 100 },
				Settings =
				{
					VerticalAlignment = VerticalAlignment.Center
				}
			};
			Add(text);
		}

		public override void Recalculate()
		{
			base.Recalculate();

			text.Width.Pixels = -texture.Dimensions.Width - 8;
			text.X.Pixels = texture.Dimensions.Width + 8;
			text.Recalculate();
		}
	}

	private UIGrid<BaseElement> gridRequired;
	private UIGrid<BaseElement> gridIncompatible;

	private void SetupRequiredIncompatible()
	{
		BaseElement holder = new()
		{
			Width = { Percent = 50 },
			Height = { Percent = 65 },
			X = { Percent = 50 },
			Y = { Percent = 10 },
			PositionIncludeDims = false
		};
		Add(holder);

		UIPanel panelRequirements = new()
		{
			Width = { Percent = 100 },
			Height = { Percent = 50 },
			Settings = PanelSettings
		};
		holder.Add(panelRequirements);

		UIPanel panelIncompatible = new()
		{
			Width = { Percent = 100 },
			Height = { Percent = 50 },
			Y = { Percent = 100 },
			Settings = PanelSettings
		};
		holder.Add(panelIncompatible);

		UIText textRequirements = new("Requirements")
		{
			Width = { Percent = 100 },
			Height = { Pixels = 20 }
		};
		panelRequirements.Add(textRequirements);

		gridRequired = new UIGrid<BaseElement>
		{
			Width = { Percent = 100 },
			Height = { Percent = 100, Pixels = -28 },
			Y = { Pixels = 28 }
		};
		panelRequirements.Add(gridRequired);

		UIText textIncompatible = new("Incompatible")
		{
			Width = { Percent = 100 },
			Height = { Pixels = 20 }
		};
		panelIncompatible.Add(textIncompatible);

		gridIncompatible = new UIGrid<BaseElement>
		{
			Width = { Percent = 100 },
			Height = { Percent = 100, Pixels = -28 },
			Y = { Pixels = 28 }
		};
		panelIncompatible.Add(gridIncompatible);
	}
	#endregion

	#region Stats
	private UIGrid<BaseElement> gridStats;

	private class UIStat : BaseElement
	{
		private UITexture texture;
		private UIText text;

		public UIStat(DataTagData data, BaseModule module)
		{
			texture = new UITexture(ModContent.Request<Texture2D>(data.Texture))
			{
				Height = { Percent = 100 },
				Width = { Percent = -1 },
				Settings =
				{
					ScaleMode = ScaleMode.Stretch
				}
			};
			Add(texture);

			text = new UIText(data.GetText(module.Type))
			{
				Width = { Percent = 100 },
				Height = { Percent = 100 },
				Settings =
				{
					VerticalAlignment = VerticalAlignment.Center
				}
			};
			Add(text);
		}

		public override void Recalculate()
		{
			base.Recalculate();

			text.Width.Pixels = -texture.Dimensions.Width - 8;
			text.X.Pixels = texture.Dimensions.Width + 8;
			text.Recalculate();
		}
	}

	private void SetupStats()
	{
		UIPanel panelStats = new()
		{
			Width = { Percent = 50 },
			Height = { Percent = 65 },
			Y = { Percent = 10 },
			PositionIncludeDims = false,
			Settings = PanelSettings
		};
		Add(panelStats);

		UIText textStats = new("Stats")
		{
			Width = { Percent = 100 },
			Height = { Pixels = 20 }
		};
		panelStats.Add(textStats);

		gridStats = new UIGrid<BaseElement>
		{
			Width = { Percent = 100 },
			Height = { Percent = 100, Pixels = -28 },
			Y = { Pixels = 28 }
		};
		panelStats.Add(gridStats);
	}
	#endregion

	#region Recipe
	private class UIRecipeGroup : BaseElement
	{
		private int currentIndex;
		private int timer;
		private List<Item> items;
		private UIItem item;

		public UIRecipeGroup(int id, int stack)
		{
			items = RecipeGroup.recipeGroups[id].ValidItems.Select(type => new Item(type, stack)).ToList();

			item = new UIItem(items.First())
			{
				Width = { Percent = 100 },
				Height = { Percent = 100 }
			};
			Add(item);
		}

		protected override void Draw(SpriteBatch spriteBatch)
		{
			if (++timer >= 15)
			{
				currentIndex++;
				if (currentIndex >= items.Count) currentIndex = 0;
				timer = 0;
				item.item = items[currentIndex];
			}
		}
	}

	private class UIItem : BaseElement
	{
		public Item item;

		public UIItem(Item item)
		{
			this.item = item;
		}

		protected override void Draw(SpriteBatch spriteBatch)
		{
			if (item is null || item.IsAir) return;

			Main.instance.LoadItem(item.type);
			Texture2D itemTexture = TextureAssets.Item[item.type].Value;

			Rectangle rect = Main.itemAnimations[item.type] != null ? Main.itemAnimations[item.type].GetFrame(itemTexture) : itemTexture.Frame();
			Color newColor = Color.White;
			float pulseScale = 1f;
			ItemSlot.GetItemLight(ref newColor, ref pulseScale, item);
			int height = rect.Height;
			int width = rect.Width;
			float drawScale = 1.1f;

			float availableWidth = InnerDimensions.Width;
			if (width > availableWidth || height > availableWidth)
			{
				if (width > height) drawScale = availableWidth / width;
				else drawScale = availableWidth / height;
			}

			Vector2 position = Dimensions.TopLeft() + Dimensions.Size() * 0.5f;
			Vector2 origin = rect.Size() * 0.5f;

			if (ItemLoader.PreDrawInInventory(item, spriteBatch, position, rect, item.GetAlpha(newColor), item.GetColor(Color.White), origin, drawScale * pulseScale))
			{
				spriteBatch.Draw(itemTexture, position, rect, item.GetAlpha(newColor), 0f, origin, drawScale * pulseScale, SpriteEffects.None, 0f);
				if (item.color != Color.Transparent) spriteBatch.Draw(itemTexture, position, rect, item.GetColor(Color.White), 0f, origin, drawScale * pulseScale, SpriteEffects.None, 0f);
			}

			ItemLoader.PostDrawInInventory(item, spriteBatch, position, rect, item.GetAlpha(newColor), item.GetColor(Color.White), origin, drawScale * pulseScale);
			if (ItemID.Sets.TrapSigned[item.type]) spriteBatch.Draw(TextureAssets.Wire.Value, position + new Vector2(40f, 40f), new Rectangle(4, 58, 8, 8), Color.White, 0f, new Vector2(4f), 1f, SpriteEffects.None, 0f);
			if (item.stack > 1)
			{
				string text = item.stack.ToString();
				const float texscale = 0.8f;
				Vector2 size = FontAssets.MouseText.Value.MeasureString(text) * texscale;
				ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.MouseText.Value, text, InnerDimensions.BottomRight() - new Vector2(4f, 0f), Color.White, 0f, size, new Vector2(texscale));
			}

			if (IsMouseHovering)
			{
				Main.LocalPlayer.cursorItemIconEnabled = false;
				Main.ItemIconCacheUpdate(0);
				Main.HoverItem = item.Clone();
				Main.hoverItemName = Main.HoverItem.Name;
			}
		}
	}

	private UIGrid<BaseElement> gridRecipe;

	public void SetupRecipe()
	{
		UIPanel panelRecipe = new()
		{
			Width = { Percent = 100 },
			Height = { Percent = 20 },
			Y = { Percent = 75 },
			PositionIncludeDims = false,
			Settings = PanelSettings
		};
		Add(panelRecipe);

		UIText textRecipe = new("Recipe")
		{
			Width = { Percent = 100 },
			Height = { Pixels = 20 }
		};
		panelRecipe.Add(textRecipe);

		gridRecipe = new UIGrid<BaseElement>(20)
		{
			Width = { Percent = 100 },
			Height = { Percent = 100, Pixels = -28 },
			Y = { Pixels = 28 }
		};
		panelRecipe.Add(gridRecipe);
	}
	#endregion

	private ModularItem item;
	private BaseModule module;
	private UI.UIModule uiModule;
	
	private UIText textModule;
	private UIText textModuleDescription;

	private UIText buttonInstall;
	private UIText buttonUninstall;

	private readonly UIPanelSettings PanelSettings = new()
	{
		BorderColor = Color.Transparent,
		BackgroundColor = DrawingUtility.Colors.PanelSelected * 1.2f
		// 61, 78, 143
	};

	public InfoPanel()
	{
		#region Panels
		UIPanel panelDescription = new()
		{
			Width = { Percent = 100 },
			Height = { Percent = 10 },
			PositionIncludeDims = false,
			Settings = PanelSettings
		};
		Add(panelDescription);

		UIPanel panelInstall = new()
		{
			Width = { Percent = 100 },
			Height = { Percent = 5 },
			Y = { Percent = 100 },
			Settings = PanelSettings
		};
		Add(panelInstall);
		#endregion

		textModule = new UIText("")
		{
			Width = { Percent = 100 },
			Height = { Pixels = 20 }
		};
		panelDescription.Add(textModule);

		textModuleDescription = new UIText("")
		{
			Width = { Percent = 100 },
			Height = { Percent = 20 },
			Y = { Pixels = 28 }
		};
		panelDescription.Add(textModuleDescription);

		SetupStats();

		SetupRequiredIncompatible();

		SetupRecipe();

		buttonInstall = new UIText("Install")
		{
			Width = { Percent = 50, Pixels = -4 },
			Height = { Percent = 100 },
			Settings =
			{
				VerticalAlignment = VerticalAlignment.Center,
				HorizontalAlignment = HorizontalAlignment.Center
			}
		};
		buttonInstall.OnClick += args =>
		{
			args.Handled = true;

			if (item.CanInstall(module.Type))
			{
				BaseModule clone = module.Clone();
				item.InstallModule(clone);
				
				// todo: consume items

				uiModule.Color = Color.Lime;
				// buttonInstall.Settings.Disabled = true;
				// buttonUninstall.Settings.Disabled = false;
			}
		};
		panelInstall.Add(buttonInstall);

		buttonUninstall = new UIText("Uninstall")
		{
			Width = { Percent = 50, Pixels = -4 },
			Height = { Percent = 100 },
			X = { Percent = 100 },
			Settings =
			{
				VerticalAlignment = VerticalAlignment.Center,
				HorizontalAlignment = HorizontalAlignment.Center
			}
		};
		buttonUninstall.OnClick += args =>
		{
			args.Handled = true;

			if (item.CanUninstall(module.Type))
			{
				item.UninstallModule(module.Type);
			
				// todo: return items

				uiModule.Color = Color.Red;
				// buttonInstall.Settings.Disabled = false;
				// buttonUninstall.Settings.Disabled = true;
			}
		};
		panelInstall.Add(buttonUninstall);
	}
			
	public void SetModule(ModularItem item, UI.UIModule ui)
	{
		this.item = item;
		module = ui.Module;
		this.uiModule = ui;
		
		textModule.Text = module.DisplayName.Get();
		textModuleDescription.Text = module.Tooltip.Get();

		gridIncompatible.Clear();
		gridRequired.Clear();

		foreach (ModuleGroup incompatibleGroup in ModuleLoader.GetIncompatibleGroups(module.Type))
		{
			UIModuleGroup uiGroup = new(incompatibleGroup)
			{
				Width = { Percent = 100 },
				Height = { Pixels = 30 }
			};
			gridIncompatible.Add(uiGroup);
		}

		foreach (int incompatibleModule in ModuleLoader.GetIncompatibleModules(module.Type))
		{
			UIModule uiModule = new(ModuleLoader.GetModule(incompatibleModule))
			{
				Width = { Percent = 100 },
				Height = { Pixels = 30 }
			};
			gridIncompatible.Add(uiModule);
		}

		foreach (int requiredModule in ModuleLoader.GetRequiredModules(module.Type))
		{
			UIModule uiModule = new(ModuleLoader.GetModule(requiredModule))
			{
				Width = { Percent = 100 },
				Height = { Pixels = 30 }
			};
			gridRequired.Add(uiModule);
		}

		gridStats.Clear();
		foreach (DataTagData data in ModContent.GetInstance<ModuleDataGroup>().GetDataFor(module.Type))
		{
			UIStat text = new(data, module)
			{
				Width = { Percent = 100 },
				Height = { Pixels = 20 }
			};
			gridStats.Add(text);
		}

		gridRecipe.Clear();
		int size = 40;
		if (ModuleRecipe.recipes.TryGetValue(module.Type, out ModuleRecipe recipe))
		{
			foreach (var (id, stack) in recipe.requiredGroups)
			{
				UIRecipeGroup uiRecipeGroup = new(id, stack)
				{
					Width = { Pixels = size },
					Height = { Pixels = size }
				};
				gridRecipe.Add(uiRecipeGroup);
			}

			foreach (Item ingredient in recipe.requiredItems)
			{
				UIItem uiItem = new(ingredient)
				{
					Width = { Pixels = size },
					Height = { Pixels = size }
				};
				gridRecipe.Add(uiItem);
			}
		}

		// buttonInstall.Settings.Disabled = !selectedItem.CanInstall(module.Type);
		// buttonUninstall.Settings.Disabled = !selectedItem.CanUninstall(module.Type);
	}
}