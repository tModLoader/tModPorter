using Terraria;
using Terraria.ModLoader;

public class NewPreReforge : GlobalItem
{
    public override bool PreReforge(Item item) { return false; /* comment */ }
}