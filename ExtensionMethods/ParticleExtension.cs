using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

public static class ParticleExtension
{
    public static IEnumerator FadeOutAllParticles(this ParticleSystem _self, float _duration)
    {
        float fadeTime = 0;

        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[_self.main.maxParticles];
        int numParticlesAlive = _self.GetParticles(particles);

        while (fadeTime <= _duration)
        {
            float alphaChange = Time.deltaTime / _duration;
            for (int i = 0; i < numParticlesAlive; i++)
            {
                Color c = particles[i].startColor;
                c.a -= alphaChange;
                if (c.a < 0) c.a = 0;
                particles[i].startColor = c;
            }

            fadeTime += Time.deltaTime;

            _self.SetParticles(particles);
            yield return null;
        }
        yield break;
    }


}
