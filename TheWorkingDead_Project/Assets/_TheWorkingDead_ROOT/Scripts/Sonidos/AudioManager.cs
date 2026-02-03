using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class AudioManager : MonoBehaviour
{

    [SerializeField] private AudioManager Instance;

    public Sound[] musicSound, sfxSound;
    public AudioSource musicSource, sfxSource;


    private void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        PlayMusic("MusicOffice");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musicSound, x => x.name == name); 
        if (s == null)
        {
            Debug.Log("sonido no encontrado");  
        }
        else
        {
            musicSource.clip = s.clip;
            musicSource.Play();
        }
    }
    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sfxSound, x => x.name == name); 
        if (s == null)
        {
            Debug.Log("sonido no encontrado");
        }
        else
        {
            sfxSource.clip = s.clip;
            sfxSource.Play();
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
}
