using System.Collections.Generic;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    public Transform[] spawnPoints;

    // NPC fijo (primer spawn)
    public GameObject fixedNPC;

    // NPCs disponibles para spawns aleatorios (no deben incluir el fijo)
    public GameObject[] randomNPCs;

    // Numero total de NPCs a spawnear (incluye el fijo)
    public int npcCount = 3;
    private void Awake()
    {
        SpawnNPCs();
    }

    void SpawnNPCs()
    {
        if (spawnPoints.Length == 0 || npcCount <= 0)
            return;

        List<Transform> availablePoints = new List<Transform>(spawnPoints);
        List<GameObject> availableNPCs = new List<GameObject>(randomNPCs);

        int spawned = 0;

        // First spawn: fixed NPC
        if (fixedNPC != null && availablePoints.Count > 0 && spawned < npcCount)
        {
            Transform firstPoint = availablePoints[0];
            Instantiate(fixedNPC, firstPoint.position, firstPoint.rotation);
            availablePoints.RemoveAt(0);
            spawned++;
        }

        // Random NPCs without repetition
        while (spawned < npcCount &&
               availablePoints.Count > 0 &&
               availableNPCs.Count > 0)
        {
            int pointIndex = Random.Range(0, availablePoints.Count);
            Transform point = availablePoints[pointIndex];
            availablePoints.RemoveAt(pointIndex);

            int npcIndex = Random.Range(0, availableNPCs.Count);
            GameObject npc = availableNPCs[npcIndex];
            availableNPCs.RemoveAt(npcIndex);

            Instantiate(npc, point.position, point.rotation);

            spawned++;
        }
    }
}
