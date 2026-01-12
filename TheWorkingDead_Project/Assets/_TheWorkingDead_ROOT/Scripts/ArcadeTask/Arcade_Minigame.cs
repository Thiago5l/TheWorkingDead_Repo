using UnityEngine;
using TMPro;

public class Arcade_Minigame : TaskBase
{
    public static Arcade_Minigame instance;


    public int enemiesToKill = 3;
    private int enemiesKilled = 0;
    
    public GameObject taskCanvas;
    public TMP_Text EnemyCount; 
    protected override void IniciarTarea()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        UpdateCounter();
    }
    protected override void CancelarTarea()
    { }

    public void EnemiesKilled()
    {
        enemiesKilled++;
        UpdateCounter();

        if (enemiesKilled >= enemiesToKill)
        {
            taskCanvas.SetActive(false);
          

        }
    }
    void UpdateCounter()
    {
        if (EnemyCount != null)
        { EnemyCount.text = enemiesKilled + "/" + enemiesToKill; }
        if (enemiesKilled >= enemiesToKill)
        {Win();}
    }


}
