using Terraria;
using Terraria.ModLoader; 

public class DangersenseTile : ModTile {
	public override bool Dangersense(int i, int j, Player player) {
		return false;
	}
}

public class DangersenseGlobal : GlobalTile {
	public override bool Dangersense(int i, int j, int type, Player player) {
		return false;
	}
}