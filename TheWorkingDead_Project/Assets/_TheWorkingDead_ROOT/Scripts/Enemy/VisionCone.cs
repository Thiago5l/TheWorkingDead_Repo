using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class VisionCone : MonoBehaviour
{
    [Header("Field of View")]
    public float viewDistance;
    [Range(0, 360)]
    public float viewAngle = 120f;
    public int segmentCount = 30;
    public float chasingViewDistance = 10f;
    public float fillingViewDistance = 15f;
    [Header("Appearance")]
    public Material coneMaterial;

    private Mesh coneMesh;
    // Definir colores en la clase
    public Color colorPatrolling;  // verde semi-transparente
    public Color colorChasing;     // amarillo semi-transparente
    public Color colorFilling;     // rojo semi-transparente

    public EnemyAiBase enemyAiBase;
    MeshRenderer rend;

    void Awake()
    {
        chasingViewDistance=enemyAiBase.sightRange;
        fillingViewDistance=enemyAiBase.attackRange;
        rend = GetComponent<MeshRenderer>();
        MeshFilter mf = GetComponent<MeshFilter>();
        coneMesh = new Mesh();
        mf.mesh = coneMesh;

        if (coneMaterial != null)
            GetComponent<MeshRenderer>().material = coneMaterial;
        else
        {
            // Crear material transparente por defecto
            Material mat = new Material(Shader.Find("Standard"));
            mat.color = new Color(1f, 1f, 0f, 0.2f);
            mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            mat.SetInt("_ZWrite", 0);
            mat.DisableKeyword("_ALPHATEST_ON");
            mat.EnableKeyword("_ALPHABLEND_ON");
            mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            mat.renderQueue = 3000;
            GetComponent<MeshRenderer>().material = mat;
        }
    }

    void Update()
    {
        GenerateConeMesh();

        Color coneColor;

        if (enemyAiBase.isFillingBar)
        {
            viewDistance = fillingViewDistance;
            coneColor = colorFilling;
        }
        else if (enemyAiBase.targetInSightRange && !enemyAiBase.targetInAttackRange)
        {
            viewDistance = chasingViewDistance;
            coneColor = colorChasing;
        }
        else
        {
            viewDistance = chasingViewDistance;
            coneColor = colorPatrolling;
        }


        // Asignar color al material
        rend.material.SetColor("_BaseColor", coneColor);

    }

    void GenerateConeMesh()
    {
        coneMesh.Clear();

        Vector3 origin = new Vector3(0, 0.05f, 0); // ahora el cono se genera en local space
        Vector3[] vertices = new Vector3[segmentCount + 2];
        int[] triangles = new int[segmentCount * 3];

        vertices[0] = origin;

        float halfAngle = viewAngle / 2f;

        for (int i = 0; i <= segmentCount; i++)
        {
            float angle = -halfAngle + (viewAngle / segmentCount) * i;
            Vector3 dir = Quaternion.Euler(0, angle, 0) * Vector3.forward; // usa forward local
            vertices[i + 1] = origin + dir * viewDistance;
        }

        for (int i = 0; i < segmentCount; i++)
        {
            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = i + 2;
        }

        coneMesh.vertices = vertices;
        coneMesh.triangles = triangles;
        coneMesh.RecalculateNormals();
    }

}
