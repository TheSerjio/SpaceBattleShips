using System.Xml.Schema;
using UnityEngine;

public class AudioManager : SINGLETON<AudioManager>
{
    [SerializeField] private AudioSource mono;
    [SerializeField] private AudioSource engines;
    [SerializeField] private AudioSource lasers;
    private float dontPlayUntil;

    public static void PlaySound(SoundClip clip, bool coolDown)
    {
        if (coolDown)
        {
            if (Time.time > Self.dontPlayUntil)
            {
                Self.dontPlayUntil = Time.time + 0.2f;
                Self.mono.PlayOneShot(clip.clip, clip.volume);
            }
        }
        else
            Self.mono.PlayOneShot(clip.clip, clip.volume);
    }
}