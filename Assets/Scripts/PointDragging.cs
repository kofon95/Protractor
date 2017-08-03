using UnityEngine;

public class PointDragging : MonoBehaviour {

    [Tooltip("Index of dragging point.\nIt's no draggable if -1 is setted")]
    public int positionIndex;
    public bool freezeX;
    public bool freezeY;
    public float upperBoundY = 1;
    public float lowerBoundY = -1;
    public GameObject lineRendererKeeper;
    public GameObject lineRendererBounder;
    private LineRenderer lineRenderer;
    private TriangleLettersDrawer drawer;

    private Vector2 deltaPosition;
    private SphereCollider sphere;

    void Start () {
        SetActiveRenderer(true);
        lineRenderer = lineRendererKeeper.GetComponent<LineRenderer>();
        drawer = lineRenderer.GetComponent<TriangleLettersDrawer>();
        sphere = GetComponent<SphereCollider>();
        //lineRenderer = GetComponent<LineRenderer>();
        if (positionIndex == -1) return;
        sphere.center = lineRenderer.GetPosition(positionIndex);
	}

    private void OnMouseDown()
    {
        deltaPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - sphere.center;// - transform.position;
    }
    private void OnMouseDrag()
    {
        if (positionIndex == -1) return;
        var pos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - deltaPosition;
        var rendererPoint = lineRenderer.GetPosition(positionIndex);

        if (freezeX)
            pos.x = rendererPoint.x;
        if (freezeY)
            pos.y = rendererPoint.y;
        bool isInBounds = pos.y >= lowerBoundY && pos.y <= upperBoundY;
        if (isInBounds)
        {
            lineRenderer.SetPosition(positionIndex, pos);
            sphere.center = pos;
            drawer.DrawLetters();
        }
        SetActiveRenderer(isInBounds);
    }
    private void OnMouseUp()
    {
        SetActiveRenderer(true);
    }
    private void SetActiveRenderer(bool active)
    {
        lineRendererKeeper.SetActive(active);
        lineRendererBounder.SetActive(!active);
    }
}
