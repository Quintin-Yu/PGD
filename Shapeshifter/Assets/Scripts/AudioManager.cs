using UnityEngine.Audio;
using System;
using UnityEngine;

/**
 * This class is used to manage the music and sound effects in the game
 * 
 * Author: Quintin Yu
 * Source: Brackeys
 */

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager instance;

    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(gameObject);                              //This line makes sure that the object isn't destroyed after you swithc between scenes

        if (instance == null)                                       //If there isn't an instance of this object within the game than it's created, other wise no instance is made
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    void Start()
    {
        Play("Game Music");
    }

    public void Play(string name)                                   //Call this method when you want a sound played, use the parameter to call a specific sound
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            return;
        }

        s.source.Play();
    }
}
