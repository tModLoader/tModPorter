using Terraria;
using Terraria.ModLoader;

public class AdvancedIdentifiersTest : ModItem
{
	public void MethodA()
	{
		Item.type = 1;
		Main.tile[0, 0].TileType = 0;
	}
}