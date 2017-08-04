using UnityEngine;

public class LineRotator : MonoBehaviour {
    private const int Rounding = 5;
    public Transform rotatingLine;
    public TextMesh alphaOutput;
    public TextMesh sinOutput;
    public TextMesh cosOutput;
    public Material lineRendererMaterial;
    public Transform container;
    public Transform cicle;
    float lineAngle;
    private LineRenderer projectionX;
    private LineRenderer projectionY;

    private void Start()
    {
        print(cicle.lossyScale);
        lineAngle = rotatingLine.rotation.eulerAngles.z - rotatingLine.parent.rotation.eulerAngles.z;

        var parentPos = cicle.position;
        var half = cicle.lossyScale / 2;
        CreateLineRenderer("horizontal", new[] { new Vector3(-half.x, 0) + parentPos, new Vector3(half.x, 0) + parentPos });
        CreateLineRenderer("vertical", new[] { new Vector3(0, -half.y) + parentPos, new Vector3(0, half.y) + parentPos });
        var empty = new Vector3[0];
        projectionX = CreateLineRenderer("x", empty);
        projectionY = CreateLineRenderer("y", empty);
    }

    LineRenderer CreateLineRenderer(string name, Vector3[] positions)
    {
        var gameObject = new GameObject(name);
        gameObject.transform.parent = container;
        var r = gameObject.AddComponent<LineRenderer>();
        r.useWorldSpace = false;
        r.material = lineRendererMaterial;
        r.startWidth = 0.1f;
        r.endWidth = 0.1f;
        r.SetPositions(positions);
        return r;
    }

    private void OnMouseDrag() { DragLine(Input.mousePosition); }
    private void DragLine(Vector3 mousePosition)
    {
        Vector3 parent = cicle.position;
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
        var angleNeedToMove = (Mathf.Round(angleInDegree / Rounding) * Rounding - angleInDegree) / Mathf.Rad2Deg;
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
        pos.x *= cicle.lossyScale.x/2;
        pos.y *= cicle.lossyScale.y/2;
        // pos is point on the circle here
        var relativePos = pos + parent;
        projectionX.SetPositions(new[] { relativePos, new Vector3(parent.x, relativePos.y) });
        projectionY.SetPositions(new[] { relativePos, new Vector3(relativePos.x, parent.y) });

        rotatingLine.eulerAngles = new Vector3(rotatingLine.eulerAngles.x, rotatingLine.eulerAngles.y, lineAngle);
        alphaOutput.text = Mathf.RoundToInt(lineAngle).ToString();
        var radius = Mathf.Sqrt(pos.x * pos.x + pos.y * pos.y);
        sinOutput.text = (pos.y / radius).ToString("0.##");
        cosOutput.text = (pos.x / radius).ToString("0.##");
    }

    // Python's behaviour for modulo operator (%) (for negative num)
    // for example, -90 % 360 = 270
    private static float Mod(float num, int divisor)
    {
        return (num + divisor * (1 + Mathf.Abs((int)num) / divisor)) % divisor;
    }
}
