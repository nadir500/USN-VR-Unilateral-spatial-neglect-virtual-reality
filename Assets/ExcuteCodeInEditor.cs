using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    [ExecuteInEditMode]

public class ExcuteCodeInEditor : MonoBehaviour
{

    // Use this for initialization

    void Awake()
    {
        Debug.Log("Editor causes this Awake");
        #if UNITY_EDITOR 
	    foreach (var component in GetComponents<BoxCollider>())
        {
			DestroyImmediate(component);
        }
		  #endif

    }

    // Update is called once per frame
    void Update()
    {

    }
}
