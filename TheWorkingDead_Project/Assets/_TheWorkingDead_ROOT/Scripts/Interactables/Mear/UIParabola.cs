using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasRenderer))]
public class UIParabola : Graphic
{
    [Header("Parábola")]
    public RectTransform start;
    public RectTransform end;

    [Range(2, 100)]
    public int segments = 30;

    public float height = 150f;
    public float thickness = 4f;

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        if (!start || !end)
            return;

        Vector2 p0 = WorldToLocal(start.position);
        Vector2 p2 = WorldToLocal(end.position);

        Vector2 control = (p0 + p2) * 0.5f + Vector2.up * height;

        Vector2 prevPoint = Bezier(p0, control, p2, 0f);

        for (int i = 1; i <= segments; i++)
        {
            float t = i / (float)segments;
            Vector2 point = Bezier(p0, control, p2, t);

            AddLine(vh, prevPoint, point);
            prevPoint = point;
        }
    }

    Vector2 Bezier(Vector2 a, Vector2 b, Vector2 c, float t)
    {
        Vector2 ab = Vector2.Lerp(a, b, t);
        Vector2 bc = Vector2.Lerp(b, c, t);
        return Vector2.Lerp(ab, bc, t);
    }

    void AddLine(VertexHelper vh, Vector2 a, Vector2 b)
    {
        Vector2 dir = (b - a).normalized;
        Vector2 normal = new Vector2(-dir.y, dir.x) * thickness * 0.5f;

        int index = vh.currentVertCount;

        vh.AddVert(a - normal, color, Vector2.zero);
        vh.AddVert(a + normal, color, Vector2.zero);
        vh.AddVert(b + normal, color, Vector2.zero);
        vh.AddVert(b - normal, color, Vector2.zero);

        vh.AddTriangle(index, index + 1, index + 2);
        vh.AddTriangle(index, index + 2, index + 3);
    }

    Vector2 WorldToLocal(Vector3 worldPos)
    {
        RectTransform rt = rectTransform;
        Vector2 local;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rt,
            worldPos,
            null,
            out local
        );
        return local;
    }

#if UNITY_EDITOR
    void Update()
    {
        SetVerticesDirty(); // refresca en tiempo real
    }
#endif
}
