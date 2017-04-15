using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class AmmoInventory_Base : MonoBehaviour {
	public abstract int Count(GunType gunType);

	public abstract void Add(GunType gunType, int ammoAmount);	

	public abstract int Request(GunType gunType, int amountRequested);
	
	public abstract void SetCB_AmmoChanged(Action callback);
}
