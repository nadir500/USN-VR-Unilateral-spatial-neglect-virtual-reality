using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRendered : MonoBehaviour
{
    MeshRenderer mr;
    bool visible = false;
    bool laseState = false;
    void Start()
    {
        mr = this.GetComponent<MeshRenderer>();
        Application.targetFrameRate=30;
        QualitySettings.vSyncCount=30;
    }


    void Update()
    {
        if (GetComponent<Renderer>().isVisible)
            MyConsol.log("Visible");
    }
}
