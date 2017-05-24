using UnityEngine;
using System;

public abstract class AmmoInventory_Base : MonoBehaviour {
    /// 
    /// 
    ///                                    Methods 
    /// 
    /// 

    /// <summary>
    /// Returns the number of bullets of the type provided that this AmmoInventory contains.
    /// </summary>
    /// <param name="gunType">What type of gun to get the ammo count of.</param>
    /// <returns>The ammo count of the given gun type.</returns>
    public abstract int Count(GunType gunType);

    /// <summary>
    /// Adds bullets to this AmmoInventory of the given type and amount.
    /// </summary>
    /// <param name="gunType">The type of bullets to add to this AmmoInventory.</param>
    /// <param name="ammoAmount">The amount of bullets to add to this AmmoInventory.</param>
	public abstract void Add(GunType gunType, int ammoAmount);	

    /// <summary>
    /// Asks this AmmoInventory for the given amount and type of ammo. If the AmmoInventory has enough, the full amount will be returned, if there is not enough, the AmmoInventory will return the most it can. Whatever amount is returned will be subtracted from this AmmoInventory.
    /// </summary>
    /// <param name="gunType">The type of ammo requested.</param>
    /// <param name="amountRequested">The amount of bullets requested.</param>
    /// <returns>The amount of ammo that was available. Either the full amount or less.</returns>
	public abstract int Request(GunType gunType, int amountRequested);
	
    /// <summary>
    /// Calls the Action parameter whenever a change (add or request) in ammo occurs in this AmmoInventory.
    /// </summary>
    /// <param name="callback">The Action that will be called whenever a change in ammo occurs in the AmmoInventory.</param>
	public abstract void SetCB_AmmoChanged(Action callback);
}
