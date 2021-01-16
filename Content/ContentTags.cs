using ModularTools.Core;
using Terraria.ModLoader.Tags;

namespace ModularTools.Content
{
	public class ModuleGroup : TagGroup
	{
		public override int TypeCount => ModuleLoader.Count;
	}

	public static class ModuleTags
	{
		public static readonly TagData Plating = ContentTags.Get<ModuleGroup>(nameof(Plating));
		public static readonly TagData Battery = ContentTags.Get<ModuleGroup>(nameof(Battery));
	}

	public static class ModularItemTags
	{
		public static readonly TagData Armor = ContentTags.Get<ItemTags>(nameof(Armor));
		public static readonly TagData Tool = ContentTags.Get<ItemTags>(nameof(Tool));
		public static readonly TagData All = ContentTags.Get<ItemTags>(nameof(All));
	}
}