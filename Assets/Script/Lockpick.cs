using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//The lockpick will activate with the SECOND continued touch input.
//The idea is to press with your left thumb to tension the lock, and then take controls
//with your right thumb to manipulate the lock with the pick
public class Lockpick : MonoBehaviour
{
    //Touch Input
    public int swipeToleranceX; //swipe left/right to move
    public int swipeToleranceY; //if outside of this tolerance, swipe is disabled.
    bool fingerDown = false;
    Vector2 touchStartPos;

    //Pin related. Pick will snap to pin locations on swips. Positions[0] is just in front of the lock
    //For each level, place the pick in the scene, and then manually assign the pins to it in order.
    public Transform[] pinPositions;
    int currentPin = 0;
    public int moveToleranceY = 0; //Pin must be within this Y distance of the center of the core to switch left or right

    //Positional Variabes
    //Have this here because unity keeps changing the pivot point to the center of the group of objects despite the children
    public GameObject offsetObject;
    Vector3 pickPositionOffset;
    Vector3 corePosition; //Y position of core, determined by pick starting position
    Vector3 positionLastUpdate;

    // Start is called before the first frame update
    void Start()
    {
        corePosition = new Vector3(0, transform.position.y, 0);
        pickPositionOffset = offsetObject.transform.position - transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 1)
        {
            //Did the second touch happen? Activate the lockpick
            if (!fingerDown && Input.touches[1].phase == TouchPhase.Began)
            {
                fingerDown = true;
                touchStartPos = Input.touches[1].position;
                Debug.Log("Pick Activated");

                positionLastUpdate = touchStartPos;
            }


            //While the pick is active
            if (fingerDown)
            {
                //move Pick up and down in response to finger movement
                
                //If within toleranceY, check to see if swipe.
                    //On swipe, move pick to next location and then disable fingerdown



            }
        }

        //Finger has been released, deactivate the pick

        else if (fingerDown)
        {
            fingerDown = false;

            Debug.Log("Pick Deactivated");
        }


    }

    void moveToPin(int pin)
    {
        //Don't move out of boudns
        if (pin > pinPositions.Length - 1 || pin < 0)
        {
            return;
        }
        else
        {
            //Move to the desired pin
            transform.position =
                pinPositions[pin].position
                - new Vector3(0, corePosition.y, 0)
                - pickPositionOffset;


        }
    }
}
