using UnityEngine;

public class SonidoTareaTexto : MonoBehaviour
{
    [Header("Audio")]
    //[SerializeField] AudioManager audioManager;
    [SerializeField] string botonSoundName = "Teclear";

    public void ReproducirSonidoBoton()
    {
        AudioManager.Instance.sfxSource.pitch = Random.Range(1f, 2);
         AudioManager.Instance.PlaySFX(botonSoundName);    
    }

    }
