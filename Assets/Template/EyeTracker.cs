using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeTracker : MonoBehaviour {

    [SerializeField]
    private Transform m_Camera;
    [SerializeField]
    private LayerMask m_ExclusionLayers;           // Layers to exclude from the raycast.
    [SerializeField]
    private bool m_ShowDebugRay;                   // Optionally show the debug ray.
    [SerializeField]
    private float m_DebugRayLength = 5f;           // Debug ray length.
    [SerializeField]
    private float m_DebugRayDuration = 1f;         // How long the Debug ray will remain visible.
    [SerializeField]
    private float m_RayLength = 500f;              // How far into the scene the ray is cast.
    private GameObject lastHitted;
    private void Update() { EyeRaycast(); }


    private void EyeRaycast()
    {
        // Show the debug ray if required
        if (m_ShowDebugRay)
        {
            Debug.DrawRay(m_Camera.position, m_Camera.forward * m_DebugRayLength, Color.blue, m_DebugRayDuration);
        }

        Ray ray = new Ray(m_Camera.position, m_Camera.forward);
        RaycastHit hit;

        // Do the raycast forweards to see if we hit an interactive item
        if (Physics.Raycast(ray, out hit, m_RayLength, m_ExclusionLayers))
        {

            GameObject interactible = hit.collider.gameObject; //attempt to get the VRInteractiveItem on the hit object
            lastHitted = interactible;


            OnRaycasthit(interactible);
        }
        else
        {
            if(lastHitted != null)
            DeactiveLastInteractible();

        }
    }

    void OnRaycasthit(GameObject hit)
    {
        MyConsol.log(hit.name + " hitted");
    }
    void DeactiveLastInteractible()
    {

        MyConsol.log(lastHitted.name + "De hitted");
    }

}
