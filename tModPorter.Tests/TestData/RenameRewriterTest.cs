using System;
using Terraria;
using Terraria.ModLoader;

public class SimpleIdentifiersTest : Mod
{
	public void UseRenamedModXFields(ModType modType, ModItem modItem, ModNPC modNPC, ModPlayer modPlayer, ModProjectile modProjectile, ModMount modMount) {
		Console.Write(modType.mod);
		Console.Write(modType.mod != null);
		Console.Write(modItem.item);
		Console.Write(modNPC.npc);
		Console.Write(modPlayer.player);
		Console.Write(modProjectile.projectile);
		Console.Write(modMount.mountData);
	}

	public void UseRenamedModXFields(Item item, NPC npc, Projectile projectile, Mount.MountData mount) {
		Console.Write(item.modItem);
		Console.Write(npc.modNPC);
		Console.Write(projectile.modProjectile);
		Console.Write(mount.modMountData);
	}

	public void UseRenamedFieldOnExpression(Func<Tile> f) {
		Console.Write(f().wall);
		f().frameX = 1;
	}

	public Mod NestedRenames(ModPlayer player) {
		return player.player.inventory[0].modItem.item.modItem.mod;
	}

#if COMPILE_ERROR
	public void NoChangeCompileErrors()
	{
		projectile.FieldA = 1;
		mod.FieldA = 1;
		player.FieldA = 1;
		item.FieldA = 1;
	}
#endif

	public void NoChangeOtherIdentifiers()
	{
		int item, mod, player, projectile;
		item = 1;
		mod = 2;
		player = 3;
		projectile = 4;
	}

	public void NoChangeTypeMismatch()
	{
		Dummy item = new();
		item.FieldA = 0;
	}
}