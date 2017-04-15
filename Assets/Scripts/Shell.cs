using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : Shell_Base {

    [SerializeField]
    private Vector3 initialBulletForce = new Vector3(25, 25, 0);

    //[SerializeField]
    //private LayerMask layerMask;

    [SerializeField]
    private float destroyTimer = 4;


    private Rigidbody rigid;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        

        rigid.AddForce( transform.rotation * (initialBulletForce * -1), ForceMode.Impulse);


        Destroy(this, destroyTimer);
    }

    public override void AddVelocity(Vector3 velocity){
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = rb.velocity + velocity;
    }

}
