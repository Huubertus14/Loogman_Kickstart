using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VrFox;
public class EventTrigger : MonoBehaviour {

    [Header("Prefabs:")]
    public GameObject EventPrefab;

    [Header("Refs:")]
    public GameObject CreatePosition;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerBehaviour>())
        {
            CreateEvent();
        }
    }


    private void CreateEvent()
    {
        GameObject _event = Instantiate(EventPrefab, CreatePosition.transform.position, Quaternion.identity);
        EventBehaviour _bahviour = _event.GetComponent<EventBehaviour>();

        _bahviour.StartEvent(new EventData(12, 5, true));
    }
}
