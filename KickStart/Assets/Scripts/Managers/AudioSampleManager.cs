using UnityEngine;

public class AudioSampleManager : MonoBehaviour
{

    public static AudioSampleManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    [Header("Audio Clips:")]
    public AudioClip[] BirdSpawnSounds;
    public AudioClip[] BirdFlapSounds;
    public AudioClip[] BirdHitSounds;
    public AudioClip getBirdSpawnSounds()
    {
        int x = Random.Range(0, BirdSpawnSounds.Length);
        return BirdSpawnSounds[x];
    }
    public AudioClip getFatBirdExplosionSound()
    {
        int x = 6;
        return BirdSpawnSounds[x];
    }

    public AudioClip getBirdFlapSounds()
    {
        int x = Random.Range(0, BirdFlapSounds.Length);
        return BirdFlapSounds[x];
    }

    public AudioClip getBirdHitSounds()
    {
        int x = Random.Range(0, BirdHitSounds.Length);
        return BirdHitSounds[x];
    }

    public AudioClip[] ShootSounds;
    public AudioClip GetShootSound()
    {
        int x = Random.Range(0, ShootSounds.Length);
        return ShootSounds[x];
    }

    public AudioClip[] ExplosionSounds;
    public AudioClip GetExplosionSound()
    {
        int x = Random.Range(0, ExplosionSounds.Length);
        return ExplosionSounds[x];
    }


    public AudioClip[] DustyTalkSound;
    public AudioClip GetDustyTalkSound()
    {
        int x = Random.Range(0, DustyTalkSound.Length);
        return DustyTalkSound[x];
    }

    [Header("DustyText")]
    public AudioClip[] DustyVoorWas;
    public AudioClip[] DustyRonde01;
    public AudioClip[] DustyRonde02;
    public AudioClip[] DustyRonde03;
    public AudioClip[] DustyTimeReminder;
    public AudioClip[] DustyMisc;

    [Space]
    public AudioClip DustyHitSound;

}
