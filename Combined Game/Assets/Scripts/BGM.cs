using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MusicType { 
    MainMenuHouse,
    Overworld,
    Dungeon,
    Boss
}

public class BGM : MonoBehaviour
{
    public static GameObject instance;
    public AudioSource music;
    public MusicType musicType;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        if (instance != null && instance.GetComponent<BGM>().musicType == musicType)
        {
            Destroy(gameObject);
        }
        yield return new WaitUntil(() => instance == null);
        instance = gameObject;
        DontDestroyOnLoad(gameObject);
        music.Play();
    }

    public void Destroy(MusicType expectedMusicType)
    {
        if (expectedMusicType != musicType)
        {
            StartCoroutine(DimMusic());
        }
    }

    IEnumerator DimMusic()
    {
        float volume = music.volume;
        float timer = 0f;
        while (timer < 1f)
        {
            music.volume = volume * (1f - timer);
            yield return new WaitForEndOfFrame();
            timer += Time.unscaledDeltaTime;
        }
        Destroy(gameObject);
    }
}
