using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideGameObject : MonoBehaviour {
    private Renderer[] renderersToDisable;
    private Collider[] collidersToDisable; 


	 void Start(){
        renderersToDisable = GetComponentsInChildren<Renderer>();        
        collidersToDisable = GetComponents<Collider>();
    }


	public void Hide(){
		foreach(Collider c in collidersToDisable){
			c.enabled = false;
		}

		foreach(Renderer r in renderersToDisable){
			r.enabled = false;
		}
	}

}
