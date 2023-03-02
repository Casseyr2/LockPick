using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurningTool : MonoBehaviour
{
    public Lockpick lockpick;
    public float rotateSpeed = 200f;
    private float startingPosition;

    public float maxZRotation = 20f;
    public float minZRotation = -20f;

    public bool[] pinsArray;

    private Transform localTrans;
    
    // Start is called before the first frame update
    void Start()
    {
        localTrans = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        //First finger touch
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    startingPosition = touch.position.x;
                    Debug.Log("First touch detected.");
                    break;
                case TouchPhase.Moved:
                    /*
                    if (startingPosition > touch.position.x)
                    {
                        transform.Rotate(Vector3.back, -rotateSpeed * Time.deltaTime);
                    }
                    */

                    // Turning tool moves forward on X axis
                    if (startingPosition < touch.position.x)
                    {
                        transform.Rotate(Vector3.back, rotateSpeed * Time.deltaTime);
                    }
                    break;
                case TouchPhase.Ended:
                    Debug.Log("First touch ended.");
                    break;
            }
        }
    }

    private void MaxRotation()
    {
        Vector3 playerEulerAngles = localTrans.rotation.eulerAngles;

        playerEulerAngles.z = (playerEulerAngles.z > 180)? playerEulerAngles.z - 360 : playerEulerAngles.z;
        playerEulerAngles.z = Mathf.Clamp(playerEulerAngles.z, minZRotation, maxZRotation);

        localTrans.rotation = Quaternion.Euler(playerEulerAngles); 
    }
}
