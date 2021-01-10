using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using Terraria;

namespace ModularTools
{
	internal static partial class Hooking
	{
		internal static List<Point> PoolLocations;

		private static void GenerationPass_SmallHoles(ILContext il)
		{
			PoolLocations = new List<Point>();

			ILCursor cursor = new ILCursor(il);

			if (cursor.TryGotoNext(MoveType.After, i => i.MatchCall<WorldGen>("TileRunner")))
			{
				cursor.Emit(OpCodes.Ldloc, 4);
				cursor.Emit(OpCodes.Ldloc, 5);
				cursor.Emit(OpCodes.Ldloc, 3);

				cursor.EmitDelegate<Action<int, int, int>>((poolX, poolY, poolType) =>
				{
					if (poolType == -2) PoolLocations.Add(new Point(poolX, poolY));
				});
			}
		}
	}
}