using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    private AudioSource audioSourceMusic;

    public static GameObject bgmObject;

    void Awake()
    {
        audioSourceMusic = GetComponent<AudioSource>();

        DontDestroyOnLoad(gameObject);

        if (bgmObject)
        {
            Destroy(bgmObject);
            return;
        }

        audioSourceMusic.Play();
        bgmObject = gameObject;
    }
}
