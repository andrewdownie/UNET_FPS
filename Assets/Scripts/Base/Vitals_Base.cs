using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public abstract class Vitals_Base : NetworkBehaviour {
	public abstract void ChangeHealth(float amount);
	public abstract void ChangeStamina(float amount);
	public abstract void ChangeMana(float amount);
		
	/// <summary>
	/// Adds a health pack to the vitals.
	/// </summary>
	public abstract void AddHealthpack();

	
	/// <summary>
	/// Uses a healthpack. Does nothing if there are no health packs to use.
	/// </summary>
	public abstract void UseHealthpack();

	//: Less than capacity
	public abstract bool CanAddHealthpack();
	//: At least one 
	public abstract bool HasHealthpack();

	public abstract bool alive{get;}
	public abstract bool dead{get;}

	public abstract void Revive();
}
