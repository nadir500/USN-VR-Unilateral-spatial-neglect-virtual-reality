using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SQLITEConnect : MonoBehaviour {

DataService sqliteDS;

	void Start()
    {
        sqliteDS=new DataService("USN_Simulation.db");
       // sqliteDS.CreateGameplay();
    }
}
