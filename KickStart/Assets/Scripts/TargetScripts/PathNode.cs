using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PathNode {

    private Vector3 position;
    public PathNode(Vector3 _position)
    {
        position = _position;
    }

    public Vector3 GetPosition
    {
        get { return position; }
    }

    public void SetPosition(Vector3 _set)
    {
        position = _set;
    }

    public void Mutate(Vector3 _mutationValue)
    {
        position += _mutationValue;
    }
}
