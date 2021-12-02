using System.Xml.Schema;
using UnityEngine;

public class AudioManager : SINGLETON<AudioManager>
{
    [SerializeField] private AudioSource mono;
    [SerializeField] private AudioSource engines;
    [SerializeField] private AudioSource lasers;
    
    public static void PlaySound(SoundClip clip)
    { 
        Self.mono.PlayOneShot(clip.clip, clip.volume);
    }
}