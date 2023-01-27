using Terraria.ModLoader;

namespace ModularTools.Content;

public class ModularDamageClass : DamageClass
{
	public override void SetStaticDefaults()
	{
		ClassName.SetDefault("energy damage");
	}
}