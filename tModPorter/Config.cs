using System.Collections.Generic;
using tModPorter.Rewriters;
using static tModPorter.Rewriters.RenameRewriter;

namespace tModPorter;

public static class Config
{
	public static List<BaseRewriter> CreateRewriters() => new() {
		new RenameRewriter(),
	};

	static Config() {
		RenameInstanceField("Terraria.ModLoader.ModType",		from: "mod",			to: "Mod");
		RenameInstanceField("Terraria.ModLoader.ModItem",		from: "item",			to: "Item");
		RenameInstanceField("Terraria.ModLoader.ModNPC",			from: "npc",			to: "NPC");
		RenameInstanceField("Terraria.ModLoader.ModPlayer",		from: "player",			to: "Player");
		RenameInstanceField("Terraria.ModLoader.ModProjectile",	from: "projectile",		to: "Projectile");
		RenameInstanceField("Terraria.ModLoader.ModMount",		from: "mountData",		to: "MountData");

		RenameInstanceField("Terraria.Item",				from: "modItem",		to: "ModItem");
		RenameInstanceField("Terraria.NPC",				from: "modNPC",			to: "ModNPC");
		RenameInstanceField("Terraria.Projectile",		from: "modProjectile",	to: "ModProjectile");
		RenameInstanceField("Terraria.Mount.MountData",	from: "modMountData",	to: "ModMount");

		RenameType(from: "Terraria.ModLoader.ModMountData", to: "Terraria.ModLoader.ModMount");

		RenameInstanceField("Terraria.ModLoader.TooltipLine",	from: "text",			to: "Text");
		RenameInstanceField("Terraria.ModLoader.TooltipLine",	from: "isModifier",		to: "IsModifier");
		RenameInstanceField("Terraria.ModLoader.TooltipLine",	from: "isModifierBad",	to: "IsModifierBad");
		RenameInstanceField("Terraria.ModLoader.TooltipLine",	from: "overrideColor",	to: "OverrideColor");

		RenameInstanceField("Terraria.Tile", from: "frameX",		to: "TileFrameX");
		RenameInstanceField("Terraria.Tile", from: "frameY",		to: "TileFrameY");
		RenameInstanceField("Terraria.Tile", from: "type",		to: "TileType");
		RenameInstanceField("Terraria.Tile", from: "wall",		to: "WallType");
		RenameInstanceField("Terraria.Tile", from: "wallFrameX",	to: "WallFrameX");
		RenameInstanceField("Terraria.Tile", from: "wallFrameY",	to: "WallFrameY");

		RenameStaticField("Terraria.ID.ItemUseStyleID", from: "HoldingUp",	to: "HoldUp");
		RenameStaticField("Terraria.ID.ItemUseStyleID", from: "HoldingOut",	to: "Shoot");
		RenameStaticField("Terraria.ID.ItemUseStyleID", from: "SwingThrow",	to: "Swing");
		RenameStaticField("Terraria.ID.ItemUseStyleID", from: "EatingUsing", to: "EatFood");
		RenameStaticField("Terraria.ID.ItemUseStyleID", from: "Stabbing",	to: "Thrust");

		RenameMethod("Terraria.ModLoader.ModItem",		from: "NetRecieve",		to: "NetReceive");
		RenameMethod("Terraria.ModLoader.GlobalItem",	from: "NewPreReforge",	to: "PreReforge");
		RenameMethod("Terraria.ModLoader.ModTile",		from: "NewRightClick",	to: "RightClick");
		RenameMethod("Terraria.ModLoader.ModTile",		from: "Dangersense",	to: "IsTileDangerous");
		RenameMethod("Terraria.ModLoader.GlobalTile",	from: "Dangersense",	to: "IsTileDangerous");

		RenameMethod("Terraria.ModLoader.GlobalTile",	from: "SetDefaults", to: "SetStaticDefaults");
		RenameMethod("Terraria.ModLoader.GlobalWall",	from: "SetDefaults", to: "SetStaticDefaults");
		RenameMethod("Terraria.ModLoader.InfoDisplay",	from: "SetDefaults", to: "SetStaticDefaults");
		RenameMethod("Terraria.ModLoader.ModTile",		from: "SetDefaults", to: "SetStaticDefaults");
		RenameMethod("Terraria.ModLoader.ModWall",		from: "SetDefaults", to: "SetStaticDefaults");
		RenameMethod("Terraria.ModLoader.ModBuff",		from: "SetDefaults", to: "SetStaticDefaults");
		RenameMethod("Terraria.ModLoader.ModDust",		from: "SetDefaults", to: "SetStaticDefaults");
		RenameMethod("Terraria.ModLoader.ModMount",		from: "SetDefaults", to: "SetStaticDefaults");
		RenameMethod("Terraria.ModLoader.ModPrefix",	from: "SetDefaults", to: "SetStaticDefaults");
	}
}