using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptLookingForIGun : MonoBehaviour {

	void Start () {
		IGun gun = gameObject.GetComponent<IGun>();

		gun.Shoot();	
	}

}
