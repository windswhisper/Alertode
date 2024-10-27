using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BattleModifier
{
	public int enemyHpRate = 100;
	public int enemyAtkRate = 100;
	public int enemyMoveSpeed = 100;
	public int enemyFlySpeed = 100;
	public int buildCostRate = 100;
	public int reviveMaxCount = -1;
	public bool isTDMode = false;
	public bool isPBMode = false;
	public bool noDayRest = false;
	public List<string> nagetiveBuildTag = new List<string>();
}
