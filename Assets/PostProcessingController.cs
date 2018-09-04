using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class PostProcessingController : MonoBehaviour
{


    public PostProcessingProfile postProcessingProfile;
    void Start()
    {
        var colorGrading = postProcessingProfile.colorGrading.settings;
        switch (ExperimentParameters.colorChoice)
        {
            case "Black & White":
                {
                    //make saturation zero 
                    colorGrading.basic.saturation = 0;
                    postProcessingProfile.colorGrading.settings = colorGrading;
                    break;
                }

            case "Colorful":
                {
                    //make saturation 1.54
                    colorGrading.basic.saturation = 1.54f;
                    postProcessingProfile.colorGrading.settings = colorGrading;
                    break;
                }

        }
    }


}
