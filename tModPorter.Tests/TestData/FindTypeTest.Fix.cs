using Terraria.ModLoader;

public abstract class FindTypeTest : Mod
{
	void Method(Mod mod) {
		int a = mod.Find<ModBuff>("BuffClass").Type;
		int b = mod.Find<ModDust>("DustClass").Type;
		int c = mod.Find<ModItem>("ItemClass").Type;
		int d = mod.Find<ModMount>("MountClass").Type;
		int e = mod.Find<ModNPC>("NPCClass").Type;
		int f = mod.Find<ModPrefix>("PrefixClass").Type;
		int g = mod.Find<ModProjectile>("ProjectileClass").Type;
		int h = mod.Find<ModTileEntity>("TileEntityClass").Type;
		int i = mod.Find<ModTile>("TileClass").Type;
		int j = mod.Find<ModWall>("WallClass").Type;
	}

	void Method() {
		int a = Find<ModBuff>("BuffClass").Type;
		int b = Find<ModDust>("DustClass").Type;
		int c = Find<ModItem>("ItemClass").Type;
		int d = Find<ModMount>("MountClass").Type;
		int e = Find<ModNPC>("NPCClass").Type;
		int f = Find<ModPrefix>("PrefixClass").Type;
		int g = Find<ModProjectile>("ProjectileClass").Type;
		int h = Find<ModTileEntity>("TileEntityClass").Type;
		int i = Find<ModTile>("TileClass").Type;
		int j = Find<ModWall>("WallClass").Type;
	}
}