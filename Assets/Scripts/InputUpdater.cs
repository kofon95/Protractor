using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputUpdater : MonoBehaviour {
    public InputField a;
    public InputField c;
    public InputField alpha;
    public InputField sin;
    private TriangleLettersDrawer drawer;

    // Use this for initialization
    void Start () {
        drawer = GetComponent<TriangleLettersDrawer>();
	}
	
	// Update is called once per frame
	void OnMouseDrag() {
        a.text = drawer.GetSideLength(0).ToString("0.##");
        c.text = drawer.GetSideLength(1).ToString("0.##");
        var alpha0 = (int)drawer.GetAngle(2);
        alpha.text = alpha0 + "";
        sin.text = Mathf.Sin(alpha0).ToString("0.##");
	}
}
