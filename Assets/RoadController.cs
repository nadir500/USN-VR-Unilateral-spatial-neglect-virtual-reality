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
    private Vector3 startPosition = new Vector3(10f, -2.0f, 0.0f);
<<<<<<< HEAD
=======
    public GameObject BuildingsWrapper;
>>>>>>> master

    // Use this for initialization
    void Start () {
        for(int i = 0; i < (numberOfPathsInSingleRoad / 2); i++)
        {
            Instantiate(streatPath, new Vector3(10f + (streatPathWidth * i), -2.0f, 0.0f), Quaternion.identity);
        }
        Instantiate(midWalk, new Vector3(6.25f + streatPathWidth * (numberOfPathsInSingleRoad / 2), -2.0f, 0.0f), Quaternion.identity);
        for (int i = 0; i < (numberOfPathsInSingleRoad / 2); i++)
        {
            Instantiate(streatPath, new Vector3(12.5f + (streatPathWidth * i) + (streatPathWidth * (numberOfPathsInSingleRoad / 2)), -2.0f, 0.0f), Quaternion.identity);
        }
        Instantiate(midWalk, new Vector3(8.75f + streatPathWidth * (numberOfPathsInSingleRoad), -2.0f, 0.0f), Quaternion.identity);
<<<<<<< HEAD
=======
        BuildingsWrapper.transform.position = new Vector3(9.3f + streatPathWidth * (numberOfPathsInSingleRoad), 0, 0);
>>>>>>> master

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ButtonPressed()
    {
        Debug.Log("FUCK YOU FUCKEN BUTTON");
    }
}
