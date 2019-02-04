using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundPlate : MonoBehaviour
{
    public static float groundMass;
    public static Vector3 groundCenter;
    public static GameObject groundPlate;

    //attached in scene
    public Rigidbody rigidBody;

    void Start()
    {
        groundPlate = gameObject;
        groundCenter = transform.position;
        groundMass = rigidBody.mass;

        PlateController.massCenter = groundCenter;
        PlateController.totalMass = groundMass;
        PlateController.activeTop = gameObject;
    }

}
