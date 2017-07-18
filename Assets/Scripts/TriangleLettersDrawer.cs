using UnityEngine;
using System.Linq;
using System.Collections;
using System.Diagnostics;

public class TriangleLettersDrawer : MonoBehaviour
{
    private const float OffsetAngleLetter = .8f;
    public TextMesh sideLetter;
    public TextMesh angleLetter;
    public float offsetSideLetter = 0;

    readonly static string[] sideLetters = new[] { "a", "b", "c" };
    readonly static string[] angleLetters = new[] { "γ", "β", "α" };

    private LineRenderer lineRenderer;
    TextMesh[] sideLettersMesh;
    TextMesh[] angleLettersMesh;
    float[] sidesLength;
    float[] angles;

    // Use this for initialization
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        int len = lineRenderer.positionCount;
        sideLettersMesh = new TextMesh[len];
        angleLettersMesh = new TextMesh[len];
        sidesLength = new float[len];
        angles = new float[len];

        for (int i = 0; i < len; i++)
        {
            sideLettersMesh[i] = Instantiate(sideLetter, transform);
            sideLettersMesh[i].text = sideLetters[i];
            sideLettersMesh[i].name = sideLetters[i];

            angleLettersMesh[i] = Instantiate(angleLetter, transform);
            angleLettersMesh[i].text = angleLetters[i];
            angleLettersMesh[i].name = angleLetters[i];
        }
        DrawLetters();
    }
    
    internal void DrawLetters()
    {
        var positions = new Vector3[lineRenderer.positionCount];
        var offset = transform.position;
        lineRenderer.GetPositions(positions);
        int len = positions.Length;

        // vectors https://habrahabr.ru/post/131931/
        // a: 0, 1, 2;  b: 1, 2, 0;  c: 2, 0, 1
        for (int a = 0, b = 1, c = 2; a < len; a++, b++, c++)
        {
            if (b == len) b = 0;
            if (c == len) c = 0;

            var middle = (positions[b] + positions[a]) / 2;
            var ma = positions[a] - middle;
            var maRotated = new Vector3(ma.y, -ma.x, ma.z); // rotate to 90deg
            maRotated = Vector3.Normalize(maRotated)*.5f;

            var ab = positions[b] - positions[a];
            var bc = positions[c] - positions[b];
            var rotate = ab.x*bc.y - ab.y*bc.x;
            if (rotate >= 0)
                maRotated = -maRotated;

            sideLettersMesh[a].transform.position = middle + maRotated + offset;

            sidesLength[a] = Vector3.Distance(positions[a], positions[b]);
            angles[a] = Vector3.Angle(ab, positions[c] - positions[a]);
        }

        // bisector http://tutata.ru/217
        for (int a = 0, b = 1, c = 2; a < len; a++, b++, c++)
        {
            if (b == len) b = 0;
            if (c == len) c = 0;

            var ratio = sidesLength[a] / sidesLength[c];
            float x = (positions[b].x + ratio * positions[c].x) / (1 + ratio);
            float y = (positions[b].y + ratio * positions[c].y) / (1 + ratio);
            var bisector = new Vector3(x - positions[a].x, y - positions[a].y, positions[a].z);

            angleLettersMesh[a].transform.position = OffsetAngleLetter * Vector3.Normalize(bisector) + positions[a] + offset;
        }
    }

    internal float GetSideLength(int index)
    {
        return sidesLength[index];
    }

    internal float GetAngle(int index)
    {
        return angles[index];
    }

    static Vector2 RotateToAngle(Vector2 v, float angle, bool isRadian = false)
    {
        if (!isRadian)
            angle = angle * Mathf.PI / 180;
        var cos = Mathf.Cos(angle);
        var sin = Mathf.Sin(angle);
        return new Vector3(
            v.x * cos - v.y * sin,
            v.x * sin + v.y * cos);
    }
}
