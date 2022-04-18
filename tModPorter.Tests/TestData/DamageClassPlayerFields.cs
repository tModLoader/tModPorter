﻿using Terraria;
using Terraria.ModLoader;

namespace tModPorter.Tests.TestData;

public class DamageClassPlayerFields : Mod {
	public void MethodA() {
		player.meleeSpeed = 1;
		player.minionKB = 1;
		player.armorPenetration += 1;
		player.whipUseTimeMultiplier = 2;
	}

	public void MethodB(Player player) {
		player.meleeSpeed = 1;
		A.meleeSpeed = 2;
	} 
}

class A {
	public static int meleeSpeed = 0;
}