using UnityEngine;

public class AudioManager : SINGLETON<AudioManager>
{
    [SerializeField] private AudioSource mono;
    [SerializeField] private AudioSource engines;
    [SerializeField] private AudioSource lasers;
    [SerializeField] private AudioSource music;
    private float dontPlayUntil;

    public Ship Player { get; private set; }

    public static void PlaySound(SoundClip clip, bool coolDown)
    {
        if (coolDown)
        {
            if (Time.time > Self.dontPlayUntil)
            {
                Self.dontPlayUntil = Time.time + 0.33f;
                Self.mono.PlayOneShot(clip.clip, clip.volume);
            }
        }
        else
            Self.mono.PlayOneShot(clip.clip, clip.volume);

    }

    public void SetPlayer(Ship what)
    {
        Player = what;
        if (what)
        {
            engines.clip = what.EngineSound.clip;
            engines.Play();
        }
        else
        {
            engines.volume = 0;
            lasers.volume = 0;
        }
    }

    public void Update()
    {
        if (Player)
            engines.volume = Player.EngineQ * Player.EngineSound.volume;
        else
            lasers.volume = 0;
        if (!music.isPlaying)
        {
            var all = DataBase.Get().Music;
            var obj = all[Random.Range(0, all.Length)];
            music.PlayOneShot(obj.clip, obj.volume);
        }
    }

    public void SetLaserSound(AudioClip sound, float volume)
    {
        lasers.clip = sound;
        lasers.volume = volume;
        if (!lasers.isPlaying)
            lasers.Play();
    }
}