using Terraria;
using Terraria.ModLoader;

public class SimpleIdentifiersTest : Mod
{
#if COMPILE_ERROR
	public void MethodA()
	{
		projectile.FieldA = 1;
		mod.FieldA = 1;
		player.FieldA = 1;
		item.FieldA = 1;
	}
#endif

	public void MethodB()
	{
		int item, mod, player, projectile;
		item = 1;
		mod = 2;
		player = 3;
		projectile = 4;
	}

	public void MethodC()
	{
		Dummy item = new();
		item.FieldA = 0;
	}
}