using UnityEngine;

public class ActiveBuzz : MonoBehaviour
{
    public bool playinteract = false;
    public bool playstart = true;
    public GameObject objectToActivate;  // Objeto que se activará
    public BuzzTutorial buzzTutorial;    // Referencia al script BuzzTutorial

    public PlayerController PlayerController;

    private void Update()
    {
        if (PlayerController.playerCerca)
        {
            playstart=false;
            playinteract = true;
        }
    }

    public void ActivateObject()
    {
        if (objectToActivate != null)
            objectToActivate.SetActive(true);
        if (playinteract)
        {
            if (buzzTutorial != null)
            {
                buzzTutorial.PlayInteractDialogue();
                playinteract = false;
            }
        }
        else if (playstart)
        {
            if (buzzTutorial != null)
            {
                buzzTutorial.PlayStartDialogue();
                playstart = false;
            }
        }
    }

}
