using Terraria.ModLoader;

namespace ModularTools.Content
{
	public class ModularDamageClass: DamageClass
	{
		public override void SetupContent()
		{
			ClassName.SetDefault("energy damage");
		}
	}
}