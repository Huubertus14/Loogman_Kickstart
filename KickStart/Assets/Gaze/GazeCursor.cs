using UnityEngine;
using VrFox;

public class GazeCursor : MonoBehaviour
{
    Vector3 orginScale;
    // Use this for initialization
    void Start()
    {
        orginScale = transform.localScale;
        transform.localScale = orginScale * 0.7f;
        // Set the cursor reference
        GameManager.Instance.Reticle = gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        //var distance = (Camera.main.transform.position - transform.position).magnitude;
        //var size = distance * FixedSize * Camera.main.fieldOfView;
        //transform.localScale = Vector3.one * size;
        //transform.forward = transform.position - Camera.main.transform.position;

        // Do a raycast into the world based on the user's
        // head position and orientation.
        var headPosition = Camera.main.transform.position;
        var gazeDirection = Camera.main.transform.forward;

        RaycastHit hitInfo;
        // If the raycast hit a hologram...
        if (Physics.Raycast(headPosition, gazeDirection, out hitInfo))
        {
            if (!hitInfo.collider.gameObject.tag.Contains("Bullet"))
            {
                // Move the cursor to the point where the raycast hit.
                this.transform.position = hitInfo.point;

                // Rotate the cursor to hug the surface of the hologram.
                this.transform.rotation =
                    Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
            }
            else
            {
                DefaultPosition();
            }
        }
        else
        {
            DefaultPosition();
        }
    }
    private void DefaultPosition()
    {
        // If the raycast did not hit a hologram, display mesh at certain distance
        //transform.position = Camera.main.transform.forward*3.0f;
        transform.position = GameManager.Instance.Player.CursorPlace.transform.position;
        transform.LookAt(Camera.main.transform.position);
        transform.Rotate(-90, 0, 0);
    }
}
