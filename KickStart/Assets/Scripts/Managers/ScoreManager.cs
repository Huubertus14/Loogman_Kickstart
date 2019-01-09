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

    SaveData saveEngine;

    public List<int> AllScores = new List<int>();

    private readonly string safeFileName = "highScore";

    private void Start()
    {
        //Reset all values
        saveEngine = new SaveData();
        AllScores.Clear();

        //Update all scores to the current saved list
        int[] _temp = saveEngine.ReadData(safeFileName);
        for (int i = 0; i < _temp.Length; i++)
        {
            AllScores.Add(_temp[i]);
        }
        //Always add dummy values
        for (int i = 0; i < 6; i++)
        {
            AllScores.Add(0);
        }

        //Do check if there are yo many 0 values
        //Remove 0 values
        if (AllScores.Count > 15)
        {
            int _zeroCount = 0;
            for (int i = 0; i < AllScores.Count; i++)
            {
                if (AllScores[i] == 0)
                {
                    _zeroCount++;
                }
            }

            if ((15 - _zeroCount) > 6)
            {
                for (int i = 0; i < AllScores.Count; i++)
                {
                    if (AllScores[i] == 0)
                    {
                        AllScores.RemoveAt(i);
                    }
                }
            }

        }

        saveEngine.SaveScore(safeFileName, AllScores.ToArray());
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
        saveEngine.SaveScore(safeFileName, AllScores.ToArray());
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

        _scoreIndex = GetPositionInAllScores(_score);

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
