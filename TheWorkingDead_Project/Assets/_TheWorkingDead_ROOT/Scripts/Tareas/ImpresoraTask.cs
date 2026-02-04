using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ImpresoraTask : TaskBase
{
    [Header("Progress Bar")]
    [SerializeField] private float valorInicial = 25f;
    [SerializeField] private float suma = 10f;
    [SerializeField] private float resta = 5f;
    [SerializeField] private float tiempo = 0.5f;
    [SerializeField] private Image fillTaskBar;

    [Header("Feedback")]
    [SerializeField] private Image spacebarSprite;
    [SerializeField] private float flashTime = 0.15f;

    private float valor;


    [Header("Audio")]
    //[SerializeField] AudioManager audioManager;
    [SerializeField] string teclaSoundName = "Teclear";
    protected override void Start()
    {
       
        base.Start();
        valor = valorInicial;
        ActualizarBarra();
    }
    private void Update()
    {
        if (EstaEnListaDeTareas() == false)
        {
            particles.gameObject.SetActive(false);
        }
    }

    protected override void IniciarTarea()
    {
        valor = valorInicial;
        ActualizarBarra();
        StartCoroutine(DrainRoutine());
    }

    protected override void CancelarTarea()
    {
        CancelarBase();
        valor = valorInicial;
        ActualizarBarra();
    }

    private IEnumerator DrainRoutine()
    {
        while (interactuando && valor > 0f && valor < 100f)
        {
            yield return new WaitForSeconds(tiempo);
            valor -= resta;
            ActualizarBarra();
        }

        if (valor <= 0f)
        {
            AudioManager.Instance.sfxSource.pitch = 1f;
            Loose();
        }
        else if (valor >= 100f)
        {
            AudioManager.Instance.sfxSource.pitch = 1f;
            Win();
        }
    }

    public void Pulsar()
    {
        if (!interactuando || tareaAcabada) return;

        valor += suma;
        ActualizarBarra();
        StartCoroutine(FlashRoutine());

        AudioManager.Instance.sfxSource.pitch = Random.Range(0.1f, 2);
        AudioManager.Instance.PlaySFX(teclaSoundName);


        if (valor >= 100f)
        {
            AudioManager.Instance.sfxSource.pitch = 1f;
            Win();
        }
    }

    private void ActualizarBarra()
    {
        fillTaskBar.fillAmount = valor / 100f;
    }

    private IEnumerator FlashRoutine()
    {
        spacebarSprite.fillAmount = 1f;
        yield return new WaitForSeconds(flashTime);
        spacebarSprite.fillAmount = 0f;
    }
}
