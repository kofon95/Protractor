using UnityEngine;

public class LineRotator : MonoBehaviour {
    private const int Rounding = 15;
    public Transform rotatingLine;
    public TextMesh alphaOutput;
    public TextMesh sinOutput;
    public TextMesh cosOutput;
    public Material lineRendererMaterial;
    public Transform container;
    public Transform circle;
    float lineAngle;
    private LineRenderer projectionX;
    private LineRenderer projectionY;
    private float circleRadius;
    private LineRenderer spiral;

    private void Start()
    {
        lineAngle = rotatingLine.rotation.eulerAngles.z - rotatingLine.parent.rotation.eulerAngles.z;

        var parentPos = circle.position;
        var half = circle.lossyScale / 2;
        CreateLineRenderer("horizontal", new[] { new Vector3(-half.x, 0), new Vector3(half.x, 0) });
        CreateLineRenderer("vertical", new[] { new Vector3(0, -half.y), new Vector3(0, half.y) });
        var empty = new Vector3[0];
        projectionX = CreateLineRenderer("x", empty);
        projectionY = CreateLineRenderer("y", empty);

        circleRadius = half.x;
        
        spiral = CreateLineRenderer("some_spiral", new[] { parentPos });
    }

    LineRenderer CreateLineRenderer(string name, Vector3[] positions)
    {
        var gameObject = new GameObject(name);
        gameObject.transform.parent = container;
        gameObject.transform.localPosition = Vector3.zero;
        var r = gameObject.AddComponent<LineRenderer>();
        r.useWorldSpace = false;
        r.material = lineRendererMaterial;
        r.startWidth = 0.1f;
        r.endWidth = 0.1f;
        r.SetPositions(positions);
        return r;
    }

    private void OnMouseDrag()
    {
        DragLine(Input.mousePosition, 1);
        DrawSpiral(lineAngle / Mathf.Rad2Deg);
    }
    private void OnMouseUp()
    {
        // draw considering rounding
        DragLine(Input.mousePosition, Rounding);
        DrawSpiral(lineAngle / Mathf.Rad2Deg);
    }
    private void DragLine(Vector3 mousePosition, float rounding)
    {
        Vector3 parent = circle.position;
        var pos = Camera.main.ScreenToWorldPoint(mousePosition) - parent;
        var angle = Mathf.Atan(pos.y / pos.x);
        if (pos.x < 0)
            angle += Mathf.PI;
        else if (pos.y < 0)
            angle += 2 * Mathf.PI;
        pos.z = 0;

        float lastRelativeAngle = Mod(lineAngle, 360);
        float angleInDegree = angle * Mathf.Rad2Deg;

        // rounding
        var angleNeedToMove = (Mathf.Round(angleInDegree / rounding) * rounding - angleInDegree) / Mathf.Rad2Deg;
        var sin = Mathf.Sin(angleNeedToMove);
        var cos = Mathf.Cos(angleNeedToMove);
        pos = new Vector3(pos.x*cos - pos.y*sin, pos.x*sin + pos.y*cos);
        angle += angleNeedToMove;
        bool isSimilarAngle = Mathf.Abs(angle * Mathf.Rad2Deg - lastRelativeAngle) < 0.01f;
        if (isSimilarAngle)
            return;

        float offset = angle * Mathf.Rad2Deg - lastRelativeAngle;
        float diff = Mathf.Abs(offset);


        if (diff <= 180)
        {
            lineAngle += offset;
        }
        else
        {
            if (offset > 0)
                lineAngle -= 360 - diff;
            else
                lineAngle += 360 - diff;
        }

        pos.z = 0;
        pos.Normalize();
        pos.x *= circle.lossyScale.x/2;
        pos.y *= circle.lossyScale.y/2;

        projectionX.SetPositions(new[] { pos, new Vector3(0, pos.y) });
        projectionY.SetPositions(new[] { pos, new Vector3(pos.x, 0) });

        rotatingLine.eulerAngles = new Vector3(rotatingLine.eulerAngles.x, rotatingLine.eulerAngles.y, lineAngle);
        alphaOutput.text = Mathf.RoundToInt(lineAngle).ToString();
        var radius = Mathf.Sqrt(pos.x * pos.x + pos.y * pos.y);
        sinOutput.text = (pos.y / radius).ToString("0.##");
        cosOutput.text = (pos.x / radius).ToString("0.##");
    }

    void DrawSpiral(float angle)
    {
        float stepDegree = Mathf.PI / 180f * Mathf.Sign(angle);
        int stepsCount = Mathf.RoundToInt(angle / stepDegree);

        var positions = new Vector3[stepsCount];
        float a = 0;
        float rCoef = circleRadius / 16f * Mathf.Sign(angle);
        for (int i = 0; i < positions.Length; i++)
        {
            a += stepDegree;
            float x0 = a * rCoef;
            float y0 = 0;

            float cos = Mathf.Cos(a);
            float sin = Mathf.Sin(a);

            var x = x0 * cos - y0 * sin;
            var y = x0 * sin + y0 * cos;

            positions[i] = new Vector3(x, y, 0);
        }
        spiral.positionCount = positions.Length;
        spiral.SetPositions(positions);
    }

    // Python's behaviour for modulo operator (%) (for negative num)
    // for example, -90 % 360 = 270
    private static float Mod(float num, int divisor)
    {
        return (num + divisor * (1 + Mathf.Abs((int)num) / divisor)) % divisor;
    }
}
