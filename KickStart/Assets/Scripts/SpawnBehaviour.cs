using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBehaviour : MonoBehaviour {

    public GameObject Enemy;

	// Use this for initialization
	void Start () {
        //spawnEnemy();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            spawnEnemy();
        }
	}

    private void spawnEnemy()
    {
        GameObject tempEnemy = Instantiate(Enemy, new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10)), Quaternion.identity);
    }
}
