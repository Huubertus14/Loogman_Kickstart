using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VrFox;


public class CursorBehaviour : MonoBehaviour
{
    /// <summary>
    /// The reticle (this object) mesh renderer
    /// </summary>
    public RectTransform reticle;
    public RawImage img;

    private float rectDistance;

    /// <summary>
    /// Runs at initialization right after the Awake method
    /// </summary>
    void Start()
    {
        // Take the objects
        reticle = GetComponent<RectTransform>();
        img = GetComponent<RawImage>();
        img.color = Color.red;

        // Set the cursor reference
        GameManager.Instance.Reticle = gameObject;

    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        if (!GameManager.Instance.GameStarted)
        {
            img.enabled = false;
            return;
        }
        img.enabled = true;
        
        // Do a raycast into the world based on the user's head position and orientation.
        Vector3 headPosition = Camera.main.transform.position;
        Vector3 gazeDirection = Camera.main.transform.forward;

        RaycastHit hit;
        if (Physics.Raycast(headPosition, gazeDirection, out hit))
        {
            // If the raycast hit a hologram, display the cursor mesh.
            // Move the cursor to the point where the raycast hit.
            //  transform.position = gazeHitInfo.point;

            if (hit.collider)
            {
                reticle.transform.position = Camera.main.WorldToScreenPoint(hit.point);
            }
        }
        else
        {
            // If the raycast did not hit a hologram, hide the cursor mesh.
            rectDistance = 20f;
        }

        //reticle.localPosition = new Vector3(0, 0, rectDistance);
    }
}
