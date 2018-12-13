using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PathNode {

    public Vector3 position;
    public PathNode(Vector3 _position)
    {
        position = _position;
    }

    public Vector3 GetPosition
    {
        get { return position; }
    }
	
}
