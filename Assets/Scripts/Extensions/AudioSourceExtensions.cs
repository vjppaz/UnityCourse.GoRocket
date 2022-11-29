using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AudioSourceExtensions
{
    public static void StopWhenPlaying(this AudioSource source)
    {
        if (source.isPlaying)
        {
            source.Stop();
        }
    }

    public static void PlayWhenNotPlaying(this AudioSource source)
    {
        if (!source.isPlaying)
        {
            source.Play();
        }
    }
}
