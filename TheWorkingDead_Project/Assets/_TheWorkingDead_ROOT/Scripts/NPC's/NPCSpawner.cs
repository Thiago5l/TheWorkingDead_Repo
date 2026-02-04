using System.Collections.Generic;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    [Header("Spawn Points")]
    public Transform[] spawnPoints;
    public Transform[] spawnPointsSit;

    [Header("NPCs")]
    public GameObject[] randomNPCs;
    public GameObject[] randomNPCsSit;

    [Header("Cantidad")]
    public int npcStandingCount = 3;
    public int npcSittingCount = 2;

    private void Start()
    {
        SpawnNPCs(spawnPoints, randomNPCs, npcStandingCount);
        SpawnNPCs(spawnPointsSit, randomNPCsSit, npcSittingCount);
    }

    void SpawnNPCs(Transform[] points, GameObject[] npcs, int count)
    {
        if (points.Length == 0 || npcs.Length == 0 || count <= 0)
            return;

        List<Transform> availablePoints = new List<Transform>(points);
        List<GameObject> availableNPCs = new List<GameObject>(npcs);

        int spawned = 0;

        while (spawned < count &&
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
