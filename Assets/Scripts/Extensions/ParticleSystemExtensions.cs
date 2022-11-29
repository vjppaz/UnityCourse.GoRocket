using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ParticleSystemExtensions
{
    public static void StopWhenPlaying(this ParticleSystem paticles)
    {
        if (paticles.isPlaying)
        {
            paticles.Stop();
        }
    }

    public static void PlayWhenNotPlaying(this ParticleSystem paticles)
    {
        if (!paticles.isPlaying)
        {
            paticles.Play();
        }
    }
}
