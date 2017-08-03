using UnityEngine;
using UnityEngine.UI;

public class InputUpdater : MonoBehaviour {
    public InputField a;
    public InputField c;
    public InputField alpha;
    public InputField sin;
    public TriangleLettersDrawer drawer;
	
	void OnMouseDrag() {
        if (drawer.gameObject.activeInHierarchy)
        {
            float sideA = drawer.GetSideLength(2);
            float sideC = drawer.GetSideLength(1);

            a.text = sideA.ToString("0.##");
            c.text = sideC.ToString("0.##");
            alpha.text = drawer.GetAngle(1).ToString("0.#");
            sin.text = (sideA / sideC).ToString("0.##");
        }
        else
        {
            a.text = "-";
            c.text = "-";
            alpha.text = "90";
            sin.text = "1";
        }
	}
}
