using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerValues
{
    private int score;
    private string name;

    public PlayerValues(string _name, int _score)
    {
        name = _name;
        score = _score;
    }

    public int GetScore
    {
        get { return score; }
    }
    public string GetName
    {
        get { return name; }
    }
}
