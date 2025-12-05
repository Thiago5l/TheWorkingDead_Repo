using UnityEngine;

public class RotateCanvas : MonoBehaviour
{

    [SerializeField] public GameObject camara;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        camara = GameObject.FindGameObjectWithTag("MainCamera");
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.rotation = Quaternion.LookRotation(camara.transform.position - transform.position);
    }
}
