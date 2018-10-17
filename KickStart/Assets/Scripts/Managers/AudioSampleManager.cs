using UnityEngine;

public class AudioSampleManager : MonoBehaviour
{

    public static AudioSampleManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public AudioClip[] BirdSpawnSounds;
    public AudioClip[] BirdFlapSounds;

    public AudioClip getBirdSpawnSounds()
    {
        int x = Random.Range(0, BirdSpawnSounds.Length);
        return BirdSpawnSounds[x];
    }

    public AudioClip getBirdFlapSounds()
    {
        int x = Random.Range(0, BirdFlapSounds.Length);
        return BirdFlapSounds[x];
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
    [Space]
    public AudioClip TestVoice;
}
