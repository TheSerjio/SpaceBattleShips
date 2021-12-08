using UnityEngine;
using UnityEngine.UI;

public class SoundSettingHandler : MonoBehaviour
{
    public Slider sound;
    public Slider music;
    
    public void Start()
    {
        sound.onValueChanged.AddListener(Sound);
        music.onValueChanged.AddListener(Music);
        sound.value = AudioManager.SoundLevel;
        music.value = AudioManager.MusicLevel;
    }

    private static void Music(float q)
    {
        AudioManager.MusicLevel = q;
    }
    
    private static void Sound(float q)
    {
        AudioManager.SoundLevel = q;
    }
}