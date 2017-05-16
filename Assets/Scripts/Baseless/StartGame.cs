using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class StartGame : NetworkBehaviour {
	[SerializeField]
	GameObject[] objectsToDestroy;



	void OnTriggerEnter(Collider c){

		if(c.tag == "Player"){
			if(isServer){
				foreach(GameObject go in objectsToDestroy){
					NetworkServer.Destroy(go);
				}


				NetworkServer.Destroy(gameObject);
			}
		}


	}
}
