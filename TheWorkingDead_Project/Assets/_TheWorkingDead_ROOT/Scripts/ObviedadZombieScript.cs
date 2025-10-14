using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ObviedadZombieScript : MonoBehaviour
{

    #region General Variables
    [SerializeField] float Zombiedad = 100;
    [SerializeField] Object ZombiedadBar;
    [SerializeField] Transform PlayerTransform;
    [SerializeField] Transform RespawnPoint;
    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //ZombiedadBar.GetComponent(Slider.);
    }

    private void FixedUpdate()
    {
        while (Zombiedad > 0)
        {
            Zombiedad -= Time.deltaTime;
        }

        if (Zombiedad <= 0)
        {
            PlayerTransform.position = RespawnPoint.position;
        }
    }
}
