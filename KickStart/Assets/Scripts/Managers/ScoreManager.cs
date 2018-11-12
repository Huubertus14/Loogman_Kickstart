using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{

    public static ScoreManager Instance;
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
    }

    [Header("Prefabs")]
    public GameObject ScoreTemplate;

    [Header("All Other Scores:")]
    //public List<int> AllOtherScores = new List<int>();

    public List<int> AllScores = new List<int>();

    private void Start()
    {
        //Add dummy score
        for (int i = 0; i < 10; i++)
        {
            AllScores.Add(Random.Range(2, 35));
        }
        AllScores.Sort(SortByScore);
    }
    
    static int SortByScore(int p1, int p2)
    {
        return p2.CompareTo(p1);
    }

    public void CreateAllScores(int _playerScore)
    {
        // AllScores = AllOtherScores;
        AllScores.Add(_playerScore);
        AllScores.Sort(SortByScore);
    }

    public GameObject CreateScoreBox(int _score)
    {
        GameObject _object = Instantiate(ScoreTemplate, transform.position, Quaternion.identity);
        _object.GetComponentInChildren<Text>().text = _score.ToString();
        return _object;
    }

    /// <summary>
    /// Returns an index of where the player is in the ranking
    /// </summary>
    /// <param the current player score="_score"></param>
    /// <returns>the place in the index, 0 is on top</returns>
    public int GetPositionInTable(int _score)
    {
        //Always the first found index if for the player
        int _scoreIndex = 0;
        int _positionInTable = 0;

        int _siblingFound = 0;

        for (int i = 0; i < AllScores.Count; i++)
        {
            if (AllScores[i] == _score)
            {
                //i is the index of my score
                _scoreIndex = i;
                break;
            }
        }
        //Debug.Log(_scoreIndex);
        //need to find 5 siblings

        //first check maximum 3 above the current score
        if (_scoreIndex < 3)
        {
            if (_scoreIndex < 2)
            {
                if (_scoreIndex < 1)
                {
                    //Player is first
                    _positionInTable = 0;
                    return _positionInTable;
                }
                //player is second
                _positionInTable = 1;
                return _positionInTable;
            }
            //player is third
            _positionInTable = 2;
            return _positionInTable;
        }
        else//rthere are 3 or more scores above
        {
            if (_scoreIndex >= (AllScores.Count - 1) - 2)
            {
                if (_scoreIndex >= (AllScores.Count - 1) - 1)
                {
                    if (_scoreIndex >= (AllScores.Count - 1))
                    {
                        //player is last
                        _positionInTable = 5;
                        return _positionInTable;
                    }
                    //player is 2th last
                    _positionInTable = 4;
                    return _positionInTable;
                }
                //player is third last
                _positionInTable = 5;
                return _positionInTable;
            }
            else // player is in middle
            {
                _positionInTable = 3;
                return _positionInTable;
            }
        }
    }

    public int GetPositionInAllScores(int _score)
    {
        for (int i = 0; i < AllScores.Count; i++)
        {
            if (AllScores[i] == _score)
            {
                //i is the index of my score
                return i;
            }
        }
        return 0;
    }

}
