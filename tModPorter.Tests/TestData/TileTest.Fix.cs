using Terraria;
using Terraria.ModLoader; 

public class TileTest {
	void UseTileMembers() {
		Tile tile = new Tile();

		if (tile.HasTile) {
			tile.TileType = 0;
			tile.WallType = 0;
		}

		if (tile.HasUnactuatedTile) {
			tile.TileFrameX = 0;
			tile.TileFrameY = 0;
			tile.WallFrameX = 0;
			tile.WallFrameY = 0;
		}

		Main.tile[0, 0].TileType = 0;
	}
}