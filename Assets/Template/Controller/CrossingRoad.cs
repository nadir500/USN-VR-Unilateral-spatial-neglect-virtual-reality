using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossingRoad : MonoBehaviour
{

    Rigidbody rb;
    GameObject parentCar;
    GameObject playerGB, carColliderGB;
    Vector3 playerPos, carColliderPos;
    CarMove carMoveController;
    string carDirection;
    bool stopCar;
    LayerMask uiMask = (1 << 5);
    public delegate void HitByCar();
    public HitByCar WhenHitByCar;
    private float Timeleft;
    private CarParentOnRoad carParentOnRoadController;

    void OnTriggerEnter(Collider hitBox)
    {
        if (hitBox.tag.Equals(value: "Car") && !RoadController.fadeout_after_crossing)
        {

            Timeleft = 0.5f;
            parentCar = hitBox.transform.parent.gameObject; //bringing car game object collided with the player 
            rb = parentCar.GetComponent<Rigidbody>();

            carMoveController = parentCar.gameObject.GetComponent<CarMove>();
            carDirection = carMoveController.carDirection;
            stopCar = carMoveController.hasToStop;
            playerGB = this.gameObject; //we'll put it in an apropriate place in the hierarchy 
            carColliderGB = hitBox.gameObject;
            rb.drag = 40;
            WhenHitByCar();
            parentCar.GetComponent<CarMove>().onBrake();
            carParentOnRoadController = parentCar.transform.parent.GetComponent<CarParentOnRoad>();
            carParentOnRoadController.StopAllCarsAfterAccident(parentCar.transform.GetSiblingIndex());

        }
    }

    void Update()
    {
        if (rb != null && RoadController.fadeout_after_crossing == false)
        {
            Timeleft -= Time.deltaTime;
            if (Timeleft < 0.0f)
            {

                StopCar();
                parentCar.GetComponent<CarMove>().RemoveBrakeSound();
                // CrashSound();
                CarHornSound();
                Camera.main.GetComponent<CameraShake>().shakeDuration = 0.5f;
                rb = null;
            }
        }
    }
    IEnumerator BrakeCarSound()
    {
        yield return new WaitForSeconds(1.5f);
        parentCar.GetComponent<CarMove>().onBrake();
    }

    void StopCar()
    {
        rb.isKinematic = true;
    }
    void CrashSound()
    {
       // parentCar.GetComponent<CarMove>().CrashSound();

    }
    void CarHornSound()
    {
        parentCar.GetComponent<CarMove>().CarHorn();
    }
}
