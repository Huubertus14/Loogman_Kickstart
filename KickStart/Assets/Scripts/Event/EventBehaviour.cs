using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VrFox;
using EnumStates;

public class EventBehaviour : MonoBehaviour
{

    private float duration;
    private float spawnTimer;
    private EventData eventData;

    public float[] spawnInterval;
    private int intervalIndex;

    private void Start()
    {
        StartEvent(new EventData(10, 5, true));
    }

    public void StartEvent(EventData _data)
    {
        eventData = _data;

        duration = 0;
        intervalIndex = 0;

        spawnInterval = new float[eventData.AmountOfBirds];

        FillIntervalArray();
    }

    private void FillIntervalArray()
    {
        Debug.Log("Start filling array");
        for (int i = 0; i < spawnInterval.Length; i++)
        {
            spawnInterval[i] = 0;
        }

        for (int i = 0; i < eventData.Duration * 10; i++)
        {
            int _x = Random.Range(0, spawnInterval.Length);
            spawnInterval[_x] += 0.1f;
        }

        for (int i = 0; i < spawnInterval.Length; i++)
        {
            if (spawnInterval[i] < 0.7f)
            {
                FillIntervalArray();
                break;
            }
        }
    }

    private void Update()
    {
        spawnTimer += Time.deltaTime;
        duration += Time.deltaTime;

        //Keep index in range of array
        if (intervalIndex < spawnInterval.Length)
        {
            //Check if time has passed
            if (spawnTimer > spawnInterval[intervalIndex])
            {
                intervalIndex++;
                spawnTimer = 0;
                Debug.Log("Spawn");
            }
        }
        //End of the event object
        if (duration > eventData.Duration + 0.1f)
        {
            Debug.Log("destroy");
            Destroy(gameObject);
        }
    }
}

public class EventData
{
    private float duration;
    private int amountOfBirds;
    private bool multipleSpawns;

    public EventData(float _duration, int _amountOfBirds, bool _multipleSpawns)
    {
        duration = _duration;
        amountOfBirds = _amountOfBirds;
        multipleSpawns = _multipleSpawns;
    }

    public float Duration
    {
        get
        {
            return duration;
        }
    }

    public int AmountOfBirds
    {
        get
        {
            return amountOfBirds;
        }
    }

    public bool MultipleSpawns
    {
        get { return multipleSpawns; }
    }
}
