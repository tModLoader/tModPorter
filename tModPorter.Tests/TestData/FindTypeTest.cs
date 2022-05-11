using Terraria.ModLoader;

namespace tModPorter.Tests.TestData.FindTypeRewriterTest
{
    public abstract class FindTypeTest : ModType
    {
        void Method()
        {
            int a = mod.BuffType("BuffClass");
            int b = mod.DustType("DustClass");
            int c = mod.ItemType("ItemClass");
            //int d = mod.MountType("MountClass");
            int e = mod.NPCType("NPCClass");
            int f = mod.PrefixType("PrefixClass");
            int g = mod.ProjectileType("ProjectileClass");
            int h = mod.TileEntityType("TileEntityClass");
            int i = mod.TileType("TileClass");
            int j = mod.WallType("WallClass");
        }
    }
}