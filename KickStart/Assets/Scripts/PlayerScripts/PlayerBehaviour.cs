using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour {

    [Header("Prefabs:")]
    public GameObject BulletPrefab;

    //Player Shoots a bullet
    public void Shoot()
    {
        GameObject _bullet = Instantiate(BulletPrefab, transform.position, Quaternion.identity);
        _bullet.GetComponent<BulletBehaviour>().ShootBullet();
    }

}
