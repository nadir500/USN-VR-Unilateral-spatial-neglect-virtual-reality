using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadController : MonoBehaviour {


    public int numberOfPathsInSingleRoad = 4;
    public GameObject sidewalk;
    public GameObject streatPath;
    public GameObject midWalk;
    const int streatPathWidth = 10;
    const float sidewalkWidth = 2.5f;
    private Vector3 startPosition = new Vector3(10f, 0.1f, 0.0f);

    // Use this for initialization
    void Start () {
        for(int i = 0; i < (numberOfPathsInSingleRoad / 2); i++)
        {
            Instantiate(streatPath, new Vector3(10f + (streatPathWidth * i), 0.1f, 0.0f), Quaternion.identity);
        }
        Instantiate(midWalk, new Vector3(6.25f + streatPathWidth * (numberOfPathsInSingleRoad / 2), 0.1f, 0.0f), Quaternion.identity);
        for (int i = 0; i < (numberOfPathsInSingleRoad / 2); i++)
        {
            Instantiate(streatPath, new Vector3(12.5f + (streatPathWidth * i) + (streatPathWidth * (numberOfPathsInSingleRoad / 2)), 0.1f, 0.0f), Quaternion.identity);
        }
        Instantiate(midWalk, new Vector3(8.75f + streatPathWidth * (numberOfPathsInSingleRoad), 0.1f, 0.0f), Quaternion.identity);

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
