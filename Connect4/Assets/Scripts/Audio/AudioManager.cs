using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace C4Audio
{
    public class AudioManager : MonoBehaviour
    {

        public Sound[] sounds;

        public static AudioManager instance;

        public bool dontDestroyOnLoad;

        // Awake is called before the Start
        void Awake()
        {

            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            if (dontDestroyOnLoad)
            {
                DontDestroyOnLoad(gameObject);
            }
            
            // Creates audio source for each sound
            foreach (Sound s in this.sounds)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;

                s.source.volume = s.volume;
                s.source.pitch = s.pitch;
                s.source.loop = s.loop;

            }
        }

        public void Play(string name)
        {
            //Debug.Log($"Playing {name}");
            // Finds the sound
            Sound s = Array.Find(this.sounds, sound => sound.name == name);

            // Checks if the sound was sound
            if (s == null)
            {
                // Logs warning
                Debug.LogWarning("Sound: " + name + " not found!");
            }
            else
            {
                // plays the sound
                s.source.Play();
            }
        }

        public void Stop(string name)
        {
            // Finds the sound
            Sound s = Array.Find(this.sounds, sound => sound.name == name);

            // Checks if the sound was sound
            if (s == null)
            {
                // Logs warning
                Debug.LogWarning("Sound: " + name + " not found!");
            }
            else
            {
                // plays the sound
                s.source.Stop();
            }
        }
    }

}
