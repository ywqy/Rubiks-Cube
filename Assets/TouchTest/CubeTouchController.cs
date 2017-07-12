using UnityEngine;
using System.Collections;

public class CubeTouchController : MonoBehaviour {

	// Use this for initialization
	public GameObject targetItem;
	public float rotateRate;

	RaycastHit hit;
	bool wasRotate;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.touchCount > 0) {
			Touch theTouch = Input.GetTouch (0);
			Ray ray = Camera.main.ScreenPointToRay (theTouch.position);

			if (Physics.Raycast (ray, out hit)) {
				if (Input.touchCount == 1) {
					if (theTouch.phase == TouchPhase.Began) {
						wasRotate = false;
					}
					if (theTouch.phase == TouchPhase.Moved) {
						targetItem.transform.Rotate (theTouch.deltaPosition.y * rotateRate, -theTouch.deltaPosition.x * rotateRate, 0, Space.World);
						wasRotate = true;
					}
					if (theTouch.phase == TouchPhase.Ended || theTouch.phase == TouchPhase.Canceled) {
						if (wasRotate == true) {
							
						}
					}
				}
			}
		}
	}
}
