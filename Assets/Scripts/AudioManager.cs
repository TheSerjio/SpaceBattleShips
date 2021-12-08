using UnityEngine;

public class AudioManager : SINGLETON<AudioManager>
{
    [SerializeField] private AudioSource mono;
    [SerializeField] private AudioSource engines;
    [SerializeField] private AudioSource lasers;
    [SerializeField] private AudioSource music;
    private float dontPlayUntil;

    private SoundClip currentMusic;

    public static float SoundLevel { get; set; }

    public static float MusicLevel
    {
        get => _music_level_;
        set
        {
            _music_level_ = value;
            if (Self)
                Self.music.volume = Self.currentMusic.volume * value;
        }
    }

    private static float _music_level_;


    public Ship Player { get; private set; }

    public static void PlaySound(SoundClip clip, bool coolDown)
    {
        if (coolDown)
        {
            if (Time.time > Self.dontPlayUntil)
            {
                Self.dontPlayUntil = Time.time + 0.33f;
                Self.mono.PlayOneShot(clip.clip, clip.volume * SoundLevel);
            }
        }
        else
            Self.mono.PlayOneShot(clip.clip, clip.volume * SoundLevel);

    }

    public void SetPlayer(Ship what)
    {
        Player = what;
        if (what)
        {
            engines.clip = what.EngineSound.clip;
            engines.pitch = what.EngineSound.Pitch;
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
            engines.volume = Player.EngineQ * Player.EngineSound.volume * SoundLevel;
        else
            lasers.volume = 0;
        if (!music.isPlaying)
        {
            var all = DataBase.Get().Music;
            var obj = all[Random.Range(0, all.Length)];
            currentMusic = obj;
            music.clip = obj.clip;
            music.volume = obj.volume * MusicLevel;
            music.Play();
        }
    }

    public void SetLaserSound(AudioClip sound, float volume)
    {
        lasers.clip = sound;
        lasers.volume = volume * SoundLevel;
        if (!lasers.isPlaying)
            lasers.Play();
    }
}