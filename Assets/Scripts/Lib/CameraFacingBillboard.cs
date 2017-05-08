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

        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,
            Camera.main.transform.rotation * Vector3.up);
    }
}