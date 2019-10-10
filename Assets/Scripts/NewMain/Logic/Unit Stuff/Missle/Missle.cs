using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missle : MonoBehaviour
{
    public Tile destinationTile { get; private set; }
    public float speedPerFrame { get; private set; }
    public float maxHeight { get; private set; }
    public float distanceTraveled { get; private set; }
    public float distanceToTravel { get; private set; }
    public float travelAngle { get; private set; }


    //---------------------------------------------------------------------------------------------------------|
    //this variable is not necessary, I added it cause I have no idea how to change GameObject height in game  |
    public float currentHeight { get; private set; }                                                         //|
    //---------------------------------------------------------------------------------------------------------|

    // add new struct Position which has float x,y
    // this class can also be reused in Tile
    // this one should differ from Position defined in Tile, it has to contain float values
    // perhaps we wont need it, needs discussion

    //public Position position { get; private set; }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		// MoveTowardsDestinationTile();
        // if (position == destinationTile.position)
        // {
        //      Destroy(this);
        //      SendEventThatMissleHasCompletedItsTask();
        // }
	}

    private void MoveTowardsDestinationTile()
    {
        // update position based on angle, current position and speed
        //-----------------------------------------------------------
        distanceTraveled += speedPerFrame;
        // 4 * (MaxWysokość/MaxDystans) * (MaxDystans - PozostałyDystans) * (PozostałyDystans/MaxDystans)
        currentHeight = 4.0f * (maxHeight / distanceToTravel) * (distanceToTravel - distanceTraveled) * (distanceTraveled / distanceToTravel);
    }
}
