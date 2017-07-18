using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointDragging : MonoBehaviour {

    [Tooltip("Index of dragging point.\nIt's no draggable if -1 is setted")]
    public int positionIndex;
    private LineRenderer lineRenderer;
    private TriangleLettersDrawer drawer;

    private Vector2 deltaPosition;
    private SphereCollider sphere;

    void Start () {
        drawer = GetComponent<TriangleLettersDrawer>();
        sphere = GetComponent<SphereCollider>();
        lineRenderer = GetComponent<LineRenderer>();
        if (positionIndex == -1) return;
        sphere.center = lineRenderer.GetPosition(positionIndex);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnMouseDown()
    {
        deltaPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - sphere.center;// - transform.position;
    }
    private void OnMouseDrag()
    {
        if (positionIndex == -1) return;
        var pos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - deltaPosition;
        lineRenderer.SetPosition(positionIndex, pos);
        sphere.center = pos;
        drawer.DrawLetters();
    }
}
