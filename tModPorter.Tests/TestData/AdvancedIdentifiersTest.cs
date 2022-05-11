using Terraria;
using Terraria.ModLoader;

public class AdvancedIdentifiersTest : ModItem
{
    public void MethodA()
    {
        item.type = 1;
        Main.tile[0, 0].type = 0;
    }
}