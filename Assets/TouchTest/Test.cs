using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {

//	public GameObject canmera;

	Vector3 positionA;
	Vector3 positionB;

	void OnMouseDown(){
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;		
		if (Physics.Raycast (ray,out hit)) {
			positionA = hit.point;
		}
	}

	void OnMouseDrag ()
	{
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;		
		if (Physics.Raycast (ray,out hit)) {
			positionB = hit.point;
			this.transform.Rotate(Vector3.Cross(positionA,positionB)*3f, Space.World);
		}			

	}

	void OnMouseUp(){
	}
}
