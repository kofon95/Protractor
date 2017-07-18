using UnityEngine;
using UnityEngine.UI;

public class TouchRotater : MonoBehaviour {
	private Text output;

	// Use this for initialization
	void Start () {
		output = GameObject.Find("OutputText").GetComponent<Text>();
	}

	// Pivot between two touches
	Vector2 dragPivot;
	// Angle between two touches
	private float deltaAngle;
	bool secondTouchDown;
	// Delta for dragging with one touch
	private Vector2 dragDelta;

	void OnMouseDown(){
        dragDelta = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
	}
	void OnMouseUp(){
		secondTouchDown = false;
	}
	void OnMouseDrag()
	{
		var touches = Input.touches;
		Vector2 pos0 = Camera.main.ScreenToWorldPoint(GetTouchPosition(0));

		if (Input.touchCount >= 2){
			Vector2 pos1 = Camera.main.ScreenToWorldPoint(touches[1].position);
			Vector2 avg = (pos1 - pos0) / 2 + pos0;
			var angle = AngleTo(pos0, pos1);
			if (!secondTouchDown){
				secondTouchDown = true;
				deltaAngle = angle - transform.rotation.eulerAngles.z;
				dragPivot = avg - (Vector2)transform.position;
				return;
			}
			transform.position = new Vector3(avg.x-dragPivot.x, avg.y-dragPivot.y, transform.position.z);
			var rot = transform.rotation.eulerAngles;
			transform.rotation = Quaternion.Euler(rot.x, rot.y, angle-deltaAngle);
		} else {
			if (secondTouchDown){
				dragDelta = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
				secondTouchDown = false;
			}
			transform.position = pos0 - dragDelta;
		}
	}

	void log(System.Object text, bool clear = false){ log(text == null ? "[Null]" : text.ToString(), clear); }
	void log(string text, bool clear = false){
		if (clear){
			output.text = text;
			return;
		}
		if (output.text.Length >= 200)
			output.text = "";
		output.text += text + "\n";
	}

	static Vector2 GetTouchPosition(int touchIndex)
	{
#if MOBILE_INPUT
		return Input.GetTouch(touchIndex).position;
#else
		return Input.mousePosition;
#endif
	}

	private static float AngleTo(Vector2 pos, Vector2 target)
	{
		Vector2 diference = target - pos;
		float angle = Vector2.Angle(Vector2.right, diference);
		return target.y > pos.y ? angle : -angle;
	}
}
