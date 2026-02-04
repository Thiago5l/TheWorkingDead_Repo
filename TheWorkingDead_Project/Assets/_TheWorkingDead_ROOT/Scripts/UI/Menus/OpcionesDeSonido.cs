using UnityEngine;
using UnityEngine.UI;

public class OpcionesDeSonido : MonoBehaviour
{

    [SerializeField] Slider volumenMusicaSlider;
    [SerializeField] Slider volumenEfectosSlider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        float musicaGuardada = PlayerPrefs.GetFloat("volumenMusica", 1f);
        volumenMusicaSlider.value = musicaGuardada;
        AudioManager.Instance.musicSource.volume = musicaGuardada;

        float efectosGuardados = PlayerPrefs.GetFloat("volumenEfectos", 1f);
        volumenEfectosSlider.value = efectosGuardados;
        AudioManager.Instance.sfxSource.volume = efectosGuardados;
    }

    public void CambiarVolumenMusicaPref()
    {
        PlayerPrefs.SetFloat("volumenMusica", volumenMusicaSlider.value);
        PlayerPrefs.Save();

        AudioManager.Instance.MusicVolume(volumenMusicaSlider.value);
    }
    public void CambiarVolumenEfectosPref()
    {
        PlayerPrefs.SetFloat("volumenEfectos", volumenEfectosSlider.value);
        PlayerPrefs.Save();

        AudioManager.Instance.SoundVolume(volumenEfectosSlider.value);
    }
}


