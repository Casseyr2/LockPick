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
    bool inTolerance;
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

    //will contain the boundaries of the screen in world space
    //used in converting pixel movement to unit movement of pick
    Vector3 screenBoundaryMin;
    Vector3 screenBoundaryMax;
    float screenHeightY;
    

    // Start is called before the first frame update
    void Start()
    {
        corePosition = new Vector3(0, transform.position.y, 0);
        pickPositionOffset = offsetObject.transform.position - transform.position;

        Camera cam = Camera.main;
        screenBoundaryMin = cam.ScreenToWorldPoint(Vector3.zero);
        screenBoundaryMax = cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth, cam.pixelHeight, 0));

        screenHeightY = screenBoundaryMax.y - screenBoundaryMin.y;

        moveToPin(currentPin);
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

                inTolerance = true;
                positionLastUpdate = touchStartPos;
            }


            //While the pick is active
            if (fingerDown)
            {
                //move Pick up and down in response to finger movement

                //movement calculates the proportion of 1 pixel of movement = how many units in world space?
                float movement = (screenHeightY * (Input.touches[1].position.y - positionLastUpdate.y)) / Camera.main.pixelHeight;
                transform.Translate(new Vector3(0, movement, 0) );

                if (Mathf.Abs(Input.touches[1].position.y - touchStartPos.y) > swipeToleranceY)
                {
                    inTolerance = false;
                }

                //If within toleranceY, check to see if swipe.
                //On swipe, move pick to next location and then disable fingerdown
                if (inTolerance && Input.touches[1].position.x - touchStartPos.x > swipeToleranceX)
                {
                    moveToPin(currentPin + 1);
                }
                if (inTolerance && Input.touches[1].position.x - touchStartPos.x < -swipeToleranceX)
                {
                    moveToPin(currentPin - 1);
                }
                    

            }

            positionLastUpdate = Input.touches[1].position;
        }

        //Finger has been released, deactivate the pick

        else if (fingerDown)
        {
            fingerDown = false;

            moveToPin(currentPin);

            Debug.Log("Pick Deactivated");
        }


    }

    //Moves the pick to a new spot and disables fingerDown to end the touch.
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


            fingerDown = false;
            Debug.Log("Pick deactivated after moving");
            currentPin = pin;
        }
    }
}
