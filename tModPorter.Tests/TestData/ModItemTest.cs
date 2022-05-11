using System.IO;
using Terraria;
using Terraria.ModLoader;

public class ModItemTest : ModItem
{
    public override bool UseItem(Player player) { return true; /* comment */ }
    
    public override void NetRecieve(BinaryReader reader) { /* Empty */ }
}