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


    //making path bezier
    public void Swerving()
    {

    }

    public void Bouncing()
    {

    }

    private readonly int numPoints = 50;
    private Vector3[] positions;// = new Vector3[50];
    private Vector3 begin, end;

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

    public void BezierQuadratic(Vector3 _begin, Vector3 _end)
    {
        begin = _begin;
        end = _end;
        positions = new Vector3[numPoints];
        MakeQuadraticCurve();
        Nodes.Clear();
        for (int i = 0; i < positions.Length; i++)
        {
            Nodes.Add(new PathNode(positions[i]));
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

    private void MakeQuadraticCurve()
    {

    }

    private Vector3 CalculateQuadraticCurve(float t, Vector2 p0, Vector2 p1, Vector3 p2)
    {

        return Vector3.zero;
    }
}
