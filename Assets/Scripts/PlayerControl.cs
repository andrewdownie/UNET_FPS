using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

    [SerializeField]
    private float moveSpeed = 5;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 inputDirection;
        int xDir = 0, zDir = 0;

        if (Input.GetKey(KeyCode.W))
        {
            zDir = 1;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            zDir = -1;
        }


        if (Input.GetKey(KeyCode.A))
        {
            xDir = -1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            xDir = 1;
        }

        inputDirection = new Vector3(xDir, 0, zDir).normalized;


        transform.position = transform.position + (inputDirection * moveSpeed * Time.deltaTime);
	}
}
