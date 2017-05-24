using UnityEngine.Networking;

public abstract class Vitals_Base : NetworkBehaviour {


    /// 
    /// 
    ///                                     Getters 
    /// 
	/// 

	/// <summary>
	/// Returns true if the player is alive.
	/// </summary>
	public abstract bool alive{get;}

	/// <summary>
	/// Returns true if the player is dead.
	/// </summary>
	public abstract bool dead{get;}





    /// 
    /// 
    ///                                     Getters 
    /// 
	/// 

	/// <summary>
	/// Changes (adds/subtracts) the amount of stamina the vitals have.
	/// </summary>
	/// <param name="amount">The amount of stamina to add/subtract.</param>
	public abstract void ChangeStamina(float amount);

	/// <summary>
	/// Changes (adds/subtracts) the amount of mana the vitals have.
	/// </summary>
	/// <param name="amount"></param>
	public abstract void ChangeMana(float amount);

	/// <summary>
	/// Changes (adds/subtracts) the amount of health the vitals have.
	/// </summary>
	/// <param name="amount"></param>
	public abstract void ChangeHealth(float amount);

	/// <summary>
	/// Adds a healthpack to the vitals.
	/// </summary>
	public abstract void AddHealthpack();

	/// <summary>
	/// Uses a healthpack from the vitals.
	/// </summary>
	public abstract void UseHealthpack();

	/// <summary>
	/// Returns true if there is room to add another health pack to the vitals.
	/// </summary>
	public abstract bool CanAddHealthpack();

	/// <summary>
	/// Returns true if the vitals have atleast one health pack.
	/// </summary>
	public abstract bool HasHealthpack();

	/// <summary>
	/// Bring the player out from being dead, and give them a little health.
	/// </summary>
	public abstract void Revive();
}
