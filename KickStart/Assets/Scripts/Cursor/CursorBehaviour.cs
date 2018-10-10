using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VrFox;


public class CursorBehaviour : MonoBehaviour
{
    /// <summary>
    /// The cursor (this object) mesh renderer
    /// </summary>
    private RectTransform rect;
    private Image img;

    private float rectDistance;

    /// <summary>
    /// Runs at initialization right after the Awake method
    /// </summary>
    void Start()
    {
        // Take the objects
        rect = GetComponent<RectTransform>();
        img = GetComponent<Image>();
        img.color = Color.white;

        // Set the cursor reference
        GameManager.Instance.Cursor = gameObject;

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

        RaycastHit gazeHitInfo;
        if (Physics.Raycast(headPosition, gazeDirection, out gazeHitInfo, Mathf.Infinity))
        {
            // If the raycast hit a hologram, display the cursor mesh.

            // Move the cursor to the point where the raycast hit.
            //  transform.position = gazeHitInfo.point;
            if (!gazeHitInfo.transform.gameObject.tag.Contains("Bullet"))
            {
                rectDistance = Vector3.Distance(gazeHitInfo.transform.position, headPosition);
            }
            else
            {
                rectDistance = 150;
            }
        }
        else
        {
            // If the raycast did not hit a hologram, hide the cursor mesh.
            rectDistance = 150;
        }

        rect.localPosition = new Vector3(0, 0, rectDistance);
    }
}
