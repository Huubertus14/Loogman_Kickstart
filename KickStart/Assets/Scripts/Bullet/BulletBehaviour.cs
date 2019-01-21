using EnumStates;
using UnityEngine;
using VrFox;

public class BulletBehaviour : MonoBehaviour
{
    private Rigidbody rb;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void ShootBullet()
    {
        if (!rb)
        {
            rb = GetComponent<Rigidbody>();
        }
        transform.position = Camera.main.transform.position;
        rb.AddForce(Camera.main.transform.forward * GameManager.Instance.GetBulletForce);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.tag.Contains("Target"))
        {
            //Target HIT!
            //AudioManager.Instance.PlayAudio(AudioSampleManager.Instance.GetExplosionSound(), 1);
            collision.gameObject.GetComponent<TargetBehaviour>().Hit(transform.position);

            GameManager.Instance.SetScoreText();

            Destroy(gameObject);
        }
        if (collision.gameObject.tag.Contains("Dusty"))
        {
            if (DustyManager.Instance.DustyHitTimer > 5)
            {
                if (GameManager.Instance.CurrentRound != Round.Intro)
                {
                    DustyManager.Instance.Messages.Add(new DustyTextFile("Hey! Ik ben geen vogel!", 5f, AudioSampleManager.Instance.DustyHitSound));
                }
                DustyManager.Instance.DustyHitTimer = 0;
                DustyManager.Instance.PlayAnimation("Panic 2");
                Destroy(gameObject);
            }
        }
    }
}
