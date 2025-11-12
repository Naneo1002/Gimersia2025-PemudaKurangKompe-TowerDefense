using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RangeIndicator : MonoBehaviour
{
    [Header("Circle Settings")]
    [Tooltip("Number of segments used to draw the circle. Higher = smoother.")]
    [SerializeField] private int segments = 64;
    [SerializeField] private float lineWidth = 0.05f;
    [SerializeField] private Color lineColor = new Color(0f, 0.6f, 1f, 0.35f);

    [Header("Behavior")]
    [Tooltip("If true the circle is visible on Start. Otherwise you can enable it from code (eg. when hovering a plot)")]
    [SerializeField] private bool showByDefault = false;

    private LineRenderer lr;
    private IceTurret turret;
    private float lastRadius = -1f;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
        lr.useWorldSpace = false; // keep circle local to turret so it moves naturally
        lr.loop = true;
        lr.positionCount = segments + 1;
        lr.startWidth = lr.endWidth = lineWidth;
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.startColor = lr.endColor = lineColor;

        turret = GetComponent<IceTurret>();

        lr.enabled = showByDefault;
    }

    private void Update()
    {
        if (turret == null) return;

        float radius = turret.Range;
        // Only redraw if radius changed (cheap optimization)
        if (!Mathf.Approximately(radius, lastRadius))
        {
            DrawCircle(radius);
            lastRadius = radius;
        }
    }

    private void DrawCircle(float radius)
    {
        if (lr == null) return;
        int pointCount = segments + 1;
        for (int i = 0; i < pointCount; i++)
        {
            float angle = ((float)i / (float)segments) * Mathf.PI * 2f;
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;
            lr.SetPosition(i, new Vector3(x, y, 0f));
        }
    }

    // Public helpers
    public void Show() => lr.enabled = true;
    public void Hide() => lr.enabled = false;
    public void Toggle() => lr.enabled = !lr.enabled;
}
