using UnityEngine.Audio;
using UnityEngine;

/**
 * This class is used to change the volume and pitch for every sound
 * 
 * @author Quintin Yu
 * Source: Brackeys
 */

[System.Serializable]
public class Sound
{
    public string name;

    public AudioClip clip;

    [Range(0f, 2f)]
    public float volume;
    [Range(.1f, 3f)]
    public float pitch;

    public bool loop;

    [HideInInspector]
    public AudioSource source;
}
