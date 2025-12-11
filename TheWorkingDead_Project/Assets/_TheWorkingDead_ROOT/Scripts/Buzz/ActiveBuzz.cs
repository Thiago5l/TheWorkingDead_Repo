using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ActiveBuzz : MonoBehaviour
{
    public bool objectActivated = false;
    public bool playinteract = false;
    public bool playstart = true;
    public bool playedinteract = false;
    public bool playedstart = false;
    public GameObject objectToActivate;  // Objeto que se activará
    public BuzzTutorial buzzTutorial;    // Referencia al script BuzzTutorial

    public PlayerController PlayerController;
    public Button button;

    public GameObject Buzz;
    public Animator BuzzAnimator;

    private void Start()
    {
        playstart = true;
        button = GetComponent<Button>();
    }
    private void Update()
    {
        if (PlayerController.playerCerca&&!playedinteract)
        {
            playstart = false;
            playinteract = true;
        }
        if (!objectToActivate.activeSelf)
        {
            if (playinteract||playstart)
                BuzzAnimator.Play("Anim_Buzz_interactable");

        }
        else if (objectToActivate.activeSelf)
        { BuzzAnimator.Play("Idle"); }
        if (playedinteract&&!playstart&&!objectToActivate.activeSelf)
        { Buzz.gameObject.SetActive(false); }
    }

        public void StartDialogue()
    {
        StartCoroutine(ActivateAndPlay());
    }

    private IEnumerator ActivateAndPlay()
    {
        if (objectToActivate == null)
        {
            Debug.LogWarning("objectToActivate es NULL");
            yield break;
        }

        if (!objectActivated)
        {
            if(!playedinteract||!playedstart) 
            objectToActivate.SetActive(true);
            objectActivated = true;

            // 2. Esperar un frame para que Unity active el objeto completamente
            yield return null;

            // 3. Reproducir el diálogo AHORA que el objeto ya está activo
            if (buzzTutorial != null)
            {
                if (playinteract&&!playedinteract)
                {
                    Debug.Log("activeinteract");
                    buzzTutorial.PlayInteractDialogue();
                    playinteract = false;
                    playedinteract = true;
                }
                else if (playstart&&!playedstart)
                {
                    Debug.Log("activestart");
                    buzzTutorial.PlayStartDialogue();
                    playstart = false;
                    playedstart = true;
                }
            }
            else
            {
                Debug.LogWarning("buzzTutorial es NULL");
            }
        }
    }


}
