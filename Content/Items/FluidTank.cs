using System.Collections.Generic;
using BaseLibrary;
using BaseLibrary.Utility;
using FluidLibrary;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ModularTools.Content.Items
{
	public class FluidTank : BaseItem, IFluidStorage
	{
		public FluidStorage FluidStorage;

		public override void OnCreate(ItemCreationContext context)
		{
			FluidStorage = new FluidStorage(1);
			FluidStorage[0].MaxVolume = 255 * 16;
		}

		public override ModItem Clone(Item item)
		{
			FluidTank modularItem = (FluidTank)base.Clone(item);
			modularItem.FluidStorage = FluidStorage.Clone();
			return modularItem;
		}

		public override void UpdateInventory(Player player)
		{
			FluidStack stack = new FluidStack(FluidLoader.CreateInstance<Water>(), 1, 0);
			FluidStorage.InsertFluid(player, ref stack);
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			FluidStack stack = FluidStorage[0];
			if (stack.Fluid != null)
			{
				tooltips.Add(new TooltipLine(Mod, "MT:1", stack.Fluid.DisplayName.Get()));
				tooltips.Add(new TooltipLine(Mod, "MT:2", $"{stack.Volume / 255f:F2}/{stack.MaxVolume / 255f:F2} buckets"));
			}
		}

		public override TagCompound Save() => new TagCompound
		{
			["Fluid"] = FluidStorage.Save()
		};

		public override void Load(TagCompound tag)
		{
			FluidStorage.Load(tag.GetCompound("Fluid"));
		}

		public FluidStorage GetFluidStorage() => FluidStorage;
	}
}