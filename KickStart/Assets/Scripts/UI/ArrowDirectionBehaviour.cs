using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowDirectionBehaviour : MonoBehaviour {

    private GameObject targetBird;
    private float speed = 10f;
    
    // Update is called once per frame
    void Update()
    {
        if (!targetBird)
        {
            targetBird = SpawnManager.Instance.GetLastBird;
        }
        else
        {
            Vector3 _direction = Camera.main.ScreenToWorldPoint(targetBird.transform.position) - transform.position;
            float _angle = Mathf.Atan2(_direction.y,_direction.x) * Mathf.Rad2Deg;
            Quaternion _quat = Quaternion.AngleAxis(_angle, Vector3.down);
            transform.rotation = Quaternion.Slerp(transform.rotation, _quat, Time.deltaTime * speed);
        }
    }
}
