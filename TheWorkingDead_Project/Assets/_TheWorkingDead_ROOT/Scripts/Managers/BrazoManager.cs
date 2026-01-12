using UnityEngine;

public class BrazoManager : MonoBehaviour
{
    [SerializeField] private GameObject brazoL;
    public GameObject pfBrazoCaido;
    public bool brazoYaCaido;
    public GameObject Feedback;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        brazoYaCaido = Feedback.GetComponent<FadeCanvas>().brazoYaCaido;
    }

    public void BrazoSeCae()
    {
        if (!brazoYaCaido)
        {

            // Asegúrate de que brazoL no sea null
            if (brazoL == null) brazoL = GameObject.FindGameObjectWithTag("BrazoL");

            pfBrazoCaido.transform.position = brazoL.transform.position;
            brazoL.SetActive(false);

            //BrazoCaido script = pfBrazoCaido.GetComponent<BrazoCaido>();


            pfBrazoCaido.SetActive(true);
            brazoYaCaido = true;

            //GameObject nuevoBrazo = Instantiate(pfBrazoCaido, brazoL.transform.position, brazoL.transform.rotation);
            //BrazoCaido script = nuevoBrazo.GetComponent<BrazoCaido>();
            //script.brazoL = brazoL;
            //script.player = GameObject.FindGameObjectWithTag("Player");
            //script.feedBackCanva = this.gameObject;
            //brazoYaCaido = true;
        }
    }

}
