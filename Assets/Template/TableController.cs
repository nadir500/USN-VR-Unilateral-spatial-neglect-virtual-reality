using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TableController : MonoBehaviour {
    private Object[] tablePrefabs;
    private Transform points;
    private Vector3[][][][][] pointsPositions;
    // Use this for initialization
    void Start() {
        tablePrefabs = Resources.LoadAll("Prefabs/TableObjects");
        points = this.transform.FindChild("Points");
        pointsPositions = new Vector3[3][][][][];

        {
            int i = 0;
            foreach (Transform level in points)         //lvl1 lvl2 lvl3
            {
                pointsPositions[i] = new Vector3[level.childCount][][][];
                int j = 0;
                foreach (Transform direction in level)        //right and left
                {
                    pointsPositions[i][j] = new Vector3[direction.childCount][][];
                    int k = 0;
                    foreach (Transform group in direction)
                    {
                        pointsPositions[i][j][k] = new Vector3[group.childCount][];
                        int l = 0;
                        foreach(Transform dist in group)
                        {
                            pointsPositions[i][j][k][l] = new Vector3[dist.childCount];
                            int p = 0;
                            foreach(Transform point in dist)
                            {

                                pointsPositions[i][j][k][l][p] = point.position;

                            }
                            p++;
                        }
                        k++;
                    }
                    j++;
                }
                i++;
            }
        }
        int[] indeces = new int[tablePrefabs.Length];
        Vector3[] usedPositions = new Vector3[6];

        for (int i = 0; i < indeces.Length; indeces[i] = i++) ;
        System.Random rnd = new System.Random();
        int[] shuffeledNumbers = Enumerable.Range(0, indeces.Length-1).OrderBy(r => rnd.Next()).ToArray();
        for (int i = 0; i < 3; i++)
        {
            for(int j = 0; j < 2; j++)
            {
                Vector3 pos = pointsPositions[i][j][Random.Range(0, pointsPositions[i][j].Length)];
                Instantiate(tablePrefabs[shuffeledNumbers[j + (i*2)]], pos, Quaternion.identity);
                usedPositions[j + (i * 2)] = pos;
            }
        }

        for(int i = 1; i < 3; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
