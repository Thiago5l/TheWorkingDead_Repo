using UnityEngine;
using UnityEngine.UI;

public class OpcionesDeSonido : MonoBehaviour
{

    [SerializeField] Slider volumenMusicaSlider;
    [SerializeField] Slider volumenEfectosSlider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerPrefs.GetFloat("volumenMusica");
        AudioManager.Instance.musicSource.volume = volumenMusicaSlider.value;
        PlayerPrefs.GetFloat("volumenEfectos");
        AudioManager.Instance.sfxSource.volume = volumenEfectosSlider.value;
    }

    public void CambiarVolumenMusicaPref()
    {
        PlayerPrefs.SetFloat("volumenMusica", volumenMusicaSlider.value);
        AudioManager.Instance.MusicVolume(volumenMusicaSlider.value);
    }
    public void CambiarVolumenEfectosPref()
    {
        PlayerPrefs.SetFloat("volumenEfectos", volumenEfectosSlider.value);
        AudioManager.Instance.SoundVolume(volumenEfectosSlider.value);
    }
}


