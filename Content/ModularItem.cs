using System.Collections.Generic;
using BaseLibrary;
using ModularTools.Core;

namespace ModularTools.Content
{
	public abstract class ModularItem : BaseItem
	{
		public sealed override void AutoStaticDefaults()
		{
			base.AutoStaticDefaults();

			ModuleLoader.validModulesForItem[Type] = new List<int>();
		}
	}
}