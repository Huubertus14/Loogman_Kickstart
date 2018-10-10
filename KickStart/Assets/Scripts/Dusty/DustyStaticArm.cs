using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VrFox;

public class DustyStaticArm : MonoBehaviour
{

    [Header("Dusty Arm Values:")]
    public float ArmRotationSpeed;

    private Transform targetObject;
    private Quaternion goalRotation;


    private void Update()
    {
        if (targetObject)
        {
            //Get target object
            Vector3 _dir = targetObject.position - transform.position;
            //Determine goal rotation
            goalRotation = Quaternion.LookRotation(_dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, goalRotation, ArmRotationSpeed * Time.deltaTime);
        }
        else
        {
            if (SpawnManager.Instance.GetLastBird != null)
            {
                targetObject = SpawnManager.Instance.GetLastBird.transform;
            }
        }
    }
}
