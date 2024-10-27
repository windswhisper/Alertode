using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class ChipEjection : BaseChip
{
	public override void Init(ChipData data){
		base.Init(data);
	}

	public override void OnEquiped(BaseBuild build){
		base.OnEquiped(build);

        var weapon = ObjectFactory.CreateWeapon("NavalBomberWeaponAG");
        weapon.Equiped(build);
        build.weapons.Add(weapon);
	}
}
