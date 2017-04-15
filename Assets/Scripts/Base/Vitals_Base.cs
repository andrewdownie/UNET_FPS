using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Vitals_Base : MonoBehaviour {
	public abstract void ChangeHealth(float amount);
	public abstract void ChangeStamina(float amount);
	public abstract void ChangeMana(float amount);
		
	/// <summary>
	/// Adds a health pack to the vitals.
	/// </summary>
	/// <returns>Returns true if the healthpack was able to be taken.</returns>
	public abstract bool AddHealthpack();

	
	/// <summary>
	/// Uses a healthpack. Does nothing if there are no health packs to use.
	/// </summary>
	public abstract void UseHealthpack();
}
