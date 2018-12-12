using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VrFox;

public class PlayerGazeBehaviour : MonoBehaviour {
    
	// Update is called once per frame
	void Update () {
        if (GameManager.Instance.GameState == EnumStates.GameStates.Waiting)
        {
            // Do a raycast into the world based on the user's head position and orientation.
            Vector3 headPosition = Camera.main.transform.position;
            Vector3 gazeDirection = Camera.main.transform.forward;

            RaycastHit hit;
            if (Physics.Raycast(headPosition, gazeDirection, out hit))
            {
                // If the raycast hit a hologram, display the cursor mesh.
                GameManager.Instance.HandleGaze(hit.collider.gameObject);
            }
            else
            {
                GameManager.Instance.StartButton.HoverExit();
            }
        }
    }
}
