using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBehaviour : MonoBehaviour {

    [Header("Prefabs:")]
    public GameObject BulletPrefab;


    [Header("Refs:")]
    public TextFlashing ScoreTextFlash;
    public TextFlashing GarbageTextFlash;
    //public GameObject MudPrefab;
    //public Sprite[] MudSprites;

  //  private List<GameObject> mudObjects = new List<GameObject>();
    
 

    //Player Shoots a bullet
    public void Shoot()
    {
        AudioManager.Instance.PlayAudio(AudioSampleManager.Instance.GetShootSound(), 1);
        GameObject _bullet = Instantiate(BulletPrefab, transform.position, Quaternion.identity);
        _bullet.GetComponent<BulletBehaviour>().ShootBullet();
    }


    public void GetMud()
    {
        //for (int i = 0; i < Random.Range(3,5); i++)
        //{
        //    GameObject _mud = Instantiate(MudPrefab, Vector3.zero, Quaternion.identity, GameManager.Instance.PlayerCanvas.transform);
        //    _mud.GetComponent<Image>().sprite = MudSprites[Random.Range(0, MudSprites.Length)];
        //    _mud.GetComponent<Image>().color = Color.green;
        //    mudObjects.Add(_mud);
        //    _mud.transform.localPosition = new Vector3(Random.Range(-30,30), Random.Range(-25,25),0);
        //}
    }

    public void ScoreFlash()
    {
        ScoreTextFlash.StartEffect();
    }

    public void GarbageFlash()
    {
        GarbageTextFlash.StartEffect();
    }

}
