using UnityEngine;
using System.Collections;

public class DestroyGameObject : MonoBehaviour {

    [SerializeField]
    private float delay = 1;


	void Start () {
        Destroy(gameObject, delay);
	}


}
