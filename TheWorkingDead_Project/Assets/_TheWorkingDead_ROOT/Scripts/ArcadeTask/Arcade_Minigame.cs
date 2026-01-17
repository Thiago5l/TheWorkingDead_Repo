using UnityEngine;
using TMPro;

public class Arcade_Minigame : TaskBase
{
    public static Arcade_Minigame instance;


    public int enemiesToKill = 3;
    private int enemiesKilled = 0;
    
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
    }
    void UpdateCounter()
    {
        if (EnemyCount != null)
        { EnemyCount.text = enemiesKilled + "/" + enemiesToKill; }
        if (enemiesKilled >= enemiesToKill)
        {Win();}
    }


}
