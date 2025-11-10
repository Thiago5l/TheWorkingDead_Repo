using UnityEngine;

public class AddOutline : MonoBehaviour
{
    public Material outlineMaterial;

    void Start()
    {
        var mf = GetComponent<MeshFilter>();
        var mr = GetComponent<MeshRenderer>();

        GameObject outline = new GameObject("Outline");
        outline.transform.SetParent(transform, false);

        outline.AddComponent<MeshFilter>().sharedMesh = mf.sharedMesh;
        outline.AddComponent<MeshRenderer>().sharedMaterial = outlineMaterial;
    }
}
