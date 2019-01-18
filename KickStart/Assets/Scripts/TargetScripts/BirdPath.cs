using System.Collections.Generic;
using UnityEngine;
using VrFox;

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

    /// <summary>
    /// Makes sure that no nodes are closer than .85 units away from player
    /// </summary>
    public void FixPath()
    {
        Vector3 _playerPos = GameManager.Instance.Player.transform.position;
        for (int i = 0; i < Nodes.Count; i++)
        {
            if (Vector3.Distance(Nodes[i].GetPosition, _playerPos) < 1.2f)
            {
                //replace node 0.5f units awat from player
                Vector3 _mutateValue = Nodes[i].GetPosition - _playerPos;
                _mutateValue.Normalize();
                _mutateValue *= 1.1f;
                if (_mutateValue.y < 0)
                {
                    _mutateValue.y = -_mutateValue.y;
                }
                Nodes[i].Mutate(_mutateValue);
            }
        }        
    }

    public void Swerving()
    {
        bool _swerve = (Random.Range(0, 2) == 1);
        Vector3 _swirlValue = new Vector3(0.5f, 0, 0.5f);

        for (int i = 1; i < Nodes.Count - 1; i += 2)
        {
            if (_swerve)
            {
                _swerve = !_swerve;
                Nodes[i].Mutate(_swirlValue);
            }
            else
            {
                _swerve = !_swerve;
                Nodes[i].Mutate(-_swirlValue);
            }
        }
    }

    public void Bouncing()
    {
        bool _up = (Random.Range(0, 2) == 1);
        Vector3 _swirlValue = new Vector3(0, 0.5f, 0);

        for (int i = 1; i < Nodes.Count - 1; i++)
        {
            if (_up)
            {
                _up = !_up;
                Nodes[i].Mutate(_swirlValue);
            }
            else
            {
                _up = !_up;
                Nodes[i].Mutate(-_swirlValue);
            }
        }
    }

}
