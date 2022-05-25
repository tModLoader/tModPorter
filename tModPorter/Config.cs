using System.Collections.Generic;
using tModPorter.Rewriters;

namespace tModPorter;

public static class Config
{
	public static List<BaseRewriter> CreateRewriters() => new() {
		new RenameRewriter()
	};

	static Config() {
		RenameRewriter.RenameInstanceField("Terraria.ModLoader.ModType",		from: "mod",			to: "Mod");
		RenameRewriter.RenameInstanceField("Terraria.ModLoader.ModItem",		from: "item",			to: "Item");
		RenameRewriter.RenameInstanceField("Terraria.ModLoader.ModNPC",			from: "npc",			to: "NPC");
		RenameRewriter.RenameInstanceField("Terraria.ModLoader.ModPlayer",		from: "player",			to: "Player");
		RenameRewriter.RenameInstanceField("Terraria.ModLoader.ModProjectile",	from: "projectile",		to: "Projectile");
		RenameRewriter.RenameInstanceField("Terraria.ModLoader.ModMount",		from: "mountData",		to: "MountData");

		RenameRewriter.RenameInstanceField("Terraria.Item",				from: "modItem",		to: "ModItem");
		RenameRewriter.RenameInstanceField("Terraria.NPC",				from: "modNPC",			to: "ModNPC");
		RenameRewriter.RenameInstanceField("Terraria.Projectile",		from: "modProjectile",	to: "ModProjectile");
		RenameRewriter.RenameInstanceField("Terraria.Mount.MountData",	from: "modMountData",	to: "ModMount");

		RenameRewriter.RenameInstanceField("Terraria.ModLoader.TooltipLine",	from: "text",			to: "Text");
		RenameRewriter.RenameInstanceField("Terraria.ModLoader.TooltipLine",	from: "isModifier",		to: "IsModifier");
		RenameRewriter.RenameInstanceField("Terraria.ModLoader.TooltipLine",	from: "isModifierBad",	to: "IsModifierBad");
		RenameRewriter.RenameInstanceField("Terraria.ModLoader.TooltipLine",	from: "overrideColor",	to: "OverrideColor");

		RenameRewriter.RenameInstanceField("Terraria.Tile", from: "frameX",		to: "TileFrameX");
		RenameRewriter.RenameInstanceField("Terraria.Tile", from: "frameY",		to: "TileFrameY");
		RenameRewriter.RenameInstanceField("Terraria.Tile", from: "type",		to: "TileType");
		RenameRewriter.RenameInstanceField("Terraria.Tile", from: "wall",		to: "WallType");
		RenameRewriter.RenameInstanceField("Terraria.Tile", from: "wallFrameX",	to: "WallFrameX");
		RenameRewriter.RenameInstanceField("Terraria.Tile", from: "wallFrameY",	to: "WallFrameY");

		RenameRewriter.RenameStaticField("Terraria.ID.ItemUseStyleID", from: "HoldingUp",	to: "HoldUp");
		RenameRewriter.RenameStaticField("Terraria.ID.ItemUseStyleID", from: "HoldingOut",	to: "Shoot");
		RenameRewriter.RenameStaticField("Terraria.ID.ItemUseStyleID", from: "SwingThrow",	to: "Swing");
		RenameRewriter.RenameStaticField("Terraria.ID.ItemUseStyleID", from: "EatingUsing", to: "EatFood");
		RenameRewriter.RenameStaticField("Terraria.ID.ItemUseStyleID", from: "Stabbing",	to: "Thrust");

		//RenameRewriter.RenameType(from: "Terraria.ModLoader.ModMountData", to: "Terraria.ModLoader.ModMount");


	}
}