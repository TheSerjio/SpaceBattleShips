using UnityEngine;
using UnityEngine.UI;

public class SoundSettingHandler : MonoBehaviour
{
    public Slider sound;
    public Slider music;

    private static bool init;
    private const string Nsound = "sound";
    private const string Nmusic = "audio";

    public static void CheckInit()
    {
        if (!init)
        {
            AudioManager.SoundLevel = PlayerPrefs.GetFloat(Nsound, 0.5f);
            AudioManager.MusicLevel = PlayerPrefs.GetFloat(Nmusic, 0.5f);
            init = true;
        }
    }
    
    public void Awake()
    {
        sound.value = AudioManager.SoundLevel;
        music.value = AudioManager.MusicLevel;
        sound.onValueChanged.AddListener(Sound);
        music.onValueChanged.AddListener(Music);
        CheckInit();
    }

    private static void Music(float q)
    {
        AudioManager.MusicLevel = q;
        PlayerPrefs.SetFloat(Nmusic, q);
    }

    private static void Sound(float q)
    {
        AudioManager.SoundLevel = q;
        PlayerPrefs.SetFloat(Nsound, q);
    }
}