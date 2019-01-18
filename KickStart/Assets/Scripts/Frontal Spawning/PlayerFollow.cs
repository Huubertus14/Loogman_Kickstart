using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VrFox;

public class PlayerFollow : MonoBehaviour {

    public GameObject Player;
    private void Start()
    {
        Player = GameManager.Instance.Player.gameObject;
    }

    // Update is called once per frame
    void Update () {
        transform.position = Player.transform.position;
        transform.rotation = Quaternion.Euler(0, Player.transform.rotation.eulerAngles.y, 0);
	}
}
