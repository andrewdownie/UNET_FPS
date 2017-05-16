using UnityEngine;
using System.Collections;

public class FloatingItem : MonoBehaviour {
    [SerializeField]
    private float degreesPerSecond = 180f;
    [SerializeField]
    private float floatFrequency = 1;

    private float currentHeightLerp;


    Vector3 minHeight, maxHeight;
    bool goingDown;

    void Start()
    {
        minHeight = new Vector3(transform.position.x, transform.position.y - 0.3f, transform.position.z);
        maxHeight = new Vector3(transform.position.x, transform.position.y + 0.3f, transform.position.z);

        currentHeightLerp = floatFrequency / 2;

        goingDown = false;
    }

	void Update () {
        float yRot = transform.position.y + (degreesPerSecond * Time.deltaTime);
        transform.Rotate(transform.rotation.x, yRot, transform.rotation.z);


        transform.position = Vector3.Lerp(minHeight, maxHeight, currentHeightLerp);


        if (goingDown)
        {
            currentHeightLerp -= Time.deltaTime;

            if(currentHeightLerp <= minHeight.y)
            {
                goingDown = false;
            }
        }
        else
        {
            currentHeightLerp += Time.deltaTime;

            if(currentHeightLerp >= maxHeight.y)
            {
                goingDown = true;
            }
        }



    }
}
