using System.Collections.Generic;
using UnityEngine;

public class BirdPath
{

    public List<PathNode> Nodes = new List<PathNode>();

    private Vector3 beginPoint, endPoint;

    /// <summary>
    /// Construct the bird path
    /// </summary>
    public BirdPath(Vector3 _begin, Vector3 _end)
    {
        beginPoint = _begin;
        endPoint = _end;
        SetNodes();
    }

    public void SetNodes()
    {
        float _dis = Vector3.Distance(beginPoint, endPoint);
        int _nodeLength = (int)(_dis / 0.5f);

        Vector3 _dir = endPoint - beginPoint;
        _dir.Normalize();

        Vector3 _increase = (_dir * _dis) / _nodeLength;

        for (int i = 0; i < _nodeLength; i++)
        {
            Nodes.Add(new PathNode(beginPoint + (_increase * i)));
        }
    }


    public void Swerving()
    {
        bool _swift = (Random.Range(0, 2) == 1);
        Vector3 _swirlValue = new Vector3(0.5f, 0, 0.5f);

        for (int i = 1; i < Nodes.Count - 1; i += 2)
        {
            if (_swift)
            {
                _swift = !_swift;
                Nodes[i].Mutate(_swirlValue);
            }
            else
            {
                _swift = !_swift;
                Nodes[i].Mutate(-_swirlValue);
            }
        }
    }

    public void Bouncing()
    {
        bool _swift = (Random.Range(0, 2) == 1);
        Vector3 _swirlValue = new Vector3(0, 0.5f, 0);

        for (int i = 1; i < Nodes.Count - 1; i++)
        {
            if (_swift)
            {
                _swift = !_swift;
                Nodes[i].Mutate(_swirlValue);
            }
            else
            {
                _swift = !_swift;
                Nodes[i].Mutate(-_swirlValue);
            }
        }
    }

}
