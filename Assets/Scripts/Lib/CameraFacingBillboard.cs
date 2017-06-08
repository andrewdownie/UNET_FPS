//SOURCE: http://wiki.unity3d.com/index.php?title=CameraFacingBillboard
using UnityEngine;
using System.Collections;
 
public class CameraFacingBillboard : MonoBehaviour
{
    public Camera m_Camera;

    void Start(){
        m_Camera = Camera.main;
    }
 
    void Update()
    {
        if(Camera.main == null){
            return;
        }


        float x, y;
        Camera c = Camera.main;
        x = c.transform.rotation.x;
        y = c.transform.rotation.y;

        Quaternion camRot = Quaternion.Euler(x, y, 0);


        



        
        transform.LookAt(transform.position + c.transform.rotation * Vector3.forward,
            camRot * Vector3.up);
    }
}