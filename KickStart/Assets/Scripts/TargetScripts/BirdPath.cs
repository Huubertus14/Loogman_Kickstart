using System.Collections.Generic;
using UnityEngine;
using VrFox;

public class BirdPath : MonoBehaviour
{
    public System.Collections.Generic.List<PathNode> Nodes = new List<PathNode>();

    private Vector3 beginPoint, endPoint;

    /// <summary>
    /// Construct the bird path
    /// </summary>
    public BirdPath(Vector3 _begin, Vector3 _end)
    {
        beginPoint = _begin;
        endPoint = _end;
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

    Vector3 middlePoint, firstMiddle, secondMiddle;

    public void ClasicSwerving()
    {
        SetNodes();
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
        SetNodes();
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

    public void Swerve()
    {
        Vector3 _direction = endPoint - beginPoint;
        middlePoint = beginPoint + (_direction * 0.5f);
        firstMiddle = beginPoint + (_direction * 0.25f);
        secondMiddle = beginPoint + (_direction * 0.75f);

        Vector3 _mutate = new Vector3(0, Random.Range(-10,10), Random.Range(-10, 10));

        firstMiddle += _mutate;
        secondMiddle += -_mutate;

        GameObject o = Instantiate(new GameObject(), beginPoint, Quaternion.identity);
        GameObject e = Instantiate(new GameObject(), middlePoint, Quaternion.identity);
        GameObject w = Instantiate(new GameObject(), endPoint, Quaternion.identity);

        o.name = "begin";
        e.name = "middle";
        w.name = "end" +
            "";
        BezierQuadratic(middlePoint, endPoint, beginPoint);
       // BezierQuadratic(middlePoint, endPoint, secondMiddle);

        //flip temp list

        temp.Reverse();
        Nodes = temp;
    }

    private readonly int numPoints = 50;
    private Vector3[] positions;// = new Vector3[50];
    private Vector3 begin, middle, end;
    System.Collections.Generic.List<PathNode> temp = new List<PathNode>();

    public void BezierLinear(Vector3 _begin, Vector3 _end) //time = 0 = begin, time = 1 = end
    {
        begin = _begin;
        end = _end;
        positions = new Vector3[numPoints];
        MakeLinearCurve();
        Nodes.Clear();
        for (int i = 0; i < positions.Length; i++)
        {
            Nodes.Add(new PathNode(positions[i]));
        }
    }

    public void BezierQuadratic(Vector3 _begin, Vector3 _middle, Vector3 _end)
    {
        positions = new Vector3[numPoints];

        MakeQuadraticCurve(_begin, _middle, _end);

        for (int i = 0; i < positions.Length; i++)
        {
            temp.Add(new PathNode(positions[i]));
        }
    }

    private void MakeLinearCurve()
    {
        for (int i = 1; i < numPoints + 1; i++)
        {
            float t = i / (float)numPoints;
            positions[i - 1] = CalculateLinearBezierPoint(t, begin, end);
        }
    }

    private Vector3 CalculateLinearBezierPoint(float t, Vector3 p0, Vector3 p1)
    {
        return p0 + t * (p1 - p0);
    }

    private void MakeQuadraticCurve(Vector3 _beg, Vector3 _mid, Vector3 _end)
    {
        for (int i = 1; i < numPoints + 1; i++)
        {
            float t = i / (float)numPoints;
            positions[i - 1] = CalculateQuadraticCurve(t, _beg, _mid, _end);
        }
    }

    private Vector3 CalculateQuadraticCurve(float t, Vector2 p0, Vector3 p1, Vector3 p2)
    {
        //B(t) = (1-t)2P0 + 2(1-t)tP1 + t2P2 , 0 < t < 1
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        Vector3 p = uu * p0;
        p += 2 * u * t * p1;
        p += tt * p2;

        return p;
    }
}
