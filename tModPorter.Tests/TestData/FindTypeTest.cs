using Terraria.ModLoader;

public abstract class FindTypeTest : Mod
{
	void Method(Mod mod) {
		int a = mod.BuffType("BuffClass");
		int b = mod.DustType("DustClass");
		int c = mod.ItemType("ItemClass");
		int d = mod.MountType("MountClass");
		int e = mod.NPCType("NPCClass");
		int f = mod.PrefixType("PrefixClass");
		int g = mod.ProjectileType("ProjectileClass");
		int h = mod.TileEntityType("TileEntityClass");
		int i = mod.TileType("TileClass");
		int j = mod.WallType("WallClass");
	}

	void Method() {
		int a = BuffType("BuffClass");
		int b = DustType("DustClass");
		int c = ItemType("ItemClass");
		int d = MountType("MountClass");
		int e = NPCType("NPCClass");
		int f = PrefixType("PrefixClass");
		int g = ProjectileType("ProjectileClass");
		int h = TileEntityType("TileEntityClass");
		int i = TileType("TileClass");
		int j = WallType("WallClass");
	}
}

