using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CubeController : MonoBehaviour {

	public GameObject rubiksCube;

	Dictionary<string, Vector3> middlePositions;
	int clockwise;
	int layerMask;
	bool readyToInput;

	GameObject[] cubes;
	List<GameObject> cubeToChange;
	Vector3 rotateCenter;
	Vector3 rotateDirection;

	float targetAngle = 0;
	const float targetAmount = 1.5f;

	Vector3 touchPosition;
	Vector3 positionA;
	Vector3 positionB;
	Vector3 hitCubePosition;
	bool hitOnBackgound ;

	void Awake(){
		cubes = GameObject.FindGameObjectsWithTag ("Cube");
		cubeToChange = new List<GameObject> ();
	}

	void Start(){
		readyToInput = true;
		clockwise = 1;
		layerMask = 1 << 8;
		positionA = new Vector3 (0, 0, 0);
		hitOnBackgound = false;
	}

	void Update(){
		//keyboard
//		if (readyToInput) {
//			readyToInput = false;
//			if (Input.GetKey (KeyCode.LeftShift)) {
//				clockwise = -1;
//				GotKey ();
//			} else {
//				clockwise = 1;
//				GotKey ();
//			}
//		}
		//touch


		if(readyToInput){
			readyToInput = false;
			//first touch

			if (Input.touchCount == 1 && Input.GetTouch (0).phase == TouchPhase.Began || Input.GetMouseButtonDown (0)) {
				if (Input.GetMouseButtonDown (0)) {
					touchPosition = Input.mousePosition;
				} else {
					touchPosition = Input.GetTouch (0).position;
				}
				Ray ray = Camera.main.ScreenPointToRay (touchPosition);
				layerMask = 1 << 0;
				RaycastHit hit;
				//is hit the cube
				bool isHitted = Physics.Raycast (ray, out hit, Mathf.Infinity, layerMask);
				if (isHitted == true && hit.collider.tag == "Cube") {
					hitCubePosition = hit.transform.localPosition;
					positionA = rubiksCube.transform.InverseTransformPoint (hit.point);
					hitOnBackgound = false;
					Debug.Log ("cubeP:"+hitCubePosition);
					Debug.Log ("Pa"+positionA);
				} else {
					//is hit the background
					layerMask = 1 << 8;
					if (Physics.Raycast (ray, out hit, Mathf.Infinity, layerMask)) {
						if (hit.collider.tag == "Background") {
							positionA = hit.point;
							hitOnBackgound = true;
						}
					}
				}
			}
			if (Input.touchCount == 1 && Input.GetTouch (0).phase == TouchPhase.Moved || Input.GetMouseButton(0) ) {
				if (Input.GetMouseButton (0)) {
					touchPosition = Input.mousePosition;
				} else {
					touchPosition = Input.GetTouch (0).position;
				}
				Ray ray = Camera.main.ScreenPointToRay (touchPosition);
				RaycastHit hit;
				if (Physics.Raycast (ray, out hit, Mathf.Infinity, layerMask)) {
					if (hitOnBackgound) {
						positionB = hit.point;	
						rubiksCube.transform.Rotate (Vector3.Cross (positionA.normalized, -positionB.normalized) * 3f, Space.World);
					} else if (Vector3.Distance (positionA, rubiksCube.transform.InverseTransformPoint(hit.point)) > 0.1) {
						positionB = rubiksCube.transform.InverseTransformPoint (hit.point);
						Debug.Log ("Pb" + positionB);
						FindCubesAndPointWithTouch (hitCubePosition, positionA, positionB);
					}
				}
			}
			if (Input.touchCount == 1 && (Input.GetTouch (0).phase == TouchPhase.Ended || Input.GetTouch (0).phase == TouchPhase.Canceled) || Input.GetMouseButtonUp(0) ) {
				
				positionA = new Vector3(0,0,0);
				positionB = positionA;
			}	
		}

		if (targetAngle != 0) {
			Rotate (rotateDirection);
		} else {
			if (!hitOnBackgound) {
				Arrange ();
			}
			readyToInput = true;
		}
	}

	void FixedUpdate(){
		
	}
	void GotTouch(Vector3 ObjPosition){
		
	}

	void GotKey (){
		if(Input.GetKeyDown(KeyCode.Q)){
			FindCubesAndPoint ("x", -1);
		}else if(Input.GetKeyDown(KeyCode.W)){
			FindCubesAndPoint ("x", 0);
		}else if(Input.GetKeyDown(KeyCode.E)){
			FindCubesAndPoint ("x", 1);
		}else if(Input.GetKeyDown(KeyCode.A)){
			FindCubesAndPoint ("y", -1);
		}else if(Input.GetKeyDown(KeyCode.S)){
			FindCubesAndPoint ("y", 0);
		}else if(Input.GetKeyDown(KeyCode.D)){
			FindCubesAndPoint ("y", 1);
		}else if(Input.GetKeyDown(KeyCode.Z)){
			FindCubesAndPoint ("z", -1);
		}else if(Input.GetKeyDown(KeyCode.X)){
			FindCubesAndPoint ("z", 0);
		}else if(Input.GetKeyDown(KeyCode.C)){
			FindCubesAndPoint ("z", 1);
		}

	}

	void FindCubesAndPoint(string xyz, int rowNumber){
		cubeToChange.Clear ();
		targetAngle = 90.0f;
		for (int i = 0; i < cubes.Length; i++) {
			Vector3 thePosition = cubes [i].transform.localPosition;
			float theDirection;
			if (xyz == "x")
				theDirection = thePosition.x;
			else if (xyz == "y")
				theDirection = thePosition.y;
			else
				theDirection = thePosition.z;
			if (Mathf.Approximately (theDirection, rowNumber)) {
				cubeToChange.Add (cubes [i]);
			}
		}
		rotateCenter = rubiksCube.transform.localPosition;

		if (xyz == "x") {
			rotateDirection = rubiksCube.transform.right;
			if (rowNumber == -1) {
				rotateCenter -= rubiksCube.transform.right;
			} else if (rowNumber == 1) {
				rotateCenter += rubiksCube.transform.right;
			}
		}else if (xyz == "y") {
			rotateDirection = rubiksCube.transform.up;
			if (rowNumber == -1) {
				rotateCenter -= rubiksCube.transform.up;
			} else if (rowNumber == 1) {
				rotateCenter += rubiksCube.transform.up;
			}
		}else if (xyz == "z") {
			rotateDirection = rubiksCube.transform.forward;
			if (rowNumber == -1) {
				rotateCenter -= rubiksCube.transform.forward;
			} else if (rowNumber == 1) {
				rotateCenter += rubiksCube.transform.forward;
			}
		}
	}

	void FindCubesAndPointWithTouch (Vector3 center, Vector3 positionA, Vector3 positionB){
		Vector3 mainFaceDirection = findMainDirection (positionA);
		Debug.Log ("mainFaceDirection:" + mainFaceDirection);
		Vector3 mainDirection = findMainDirection (positionB - positionA);
		Debug.Log ("mainDirection:" + mainDirection);
		Debug.Log ("center:"+Vector3.Cross(mainDirection,mainFaceDirection));
		if(((Mathf.Abs(mainDirection.x) == 1)&&(Mathf.Abs(mainFaceDirection.y) == 1))||((Mathf.Abs(mainDirection.y) == 1)&&(Mathf.Abs(mainFaceDirection.x) == 1))){
			FindCubesAndPoint ("z", Mathf.RoundToInt(center.z));
			clockwise = -Mathf.RoundToInt(Vector3.Cross (mainDirection, mainFaceDirection).z);
		}else if(((Mathf.Abs(mainDirection.x) == 1)&&(Mathf.Abs(mainFaceDirection.z) == 1))||((Mathf.Abs(mainDirection.z) == 1)&&(Mathf.Abs(mainFaceDirection.x) == 1))){
			FindCubesAndPoint ("y", Mathf.RoundToInt(center.y));
			clockwise = -Mathf.RoundToInt(Vector3.Cross (mainDirection, mainFaceDirection).y);
		}else if(((Mathf.Abs(mainDirection.z) == 1)&&(Mathf.Abs(mainFaceDirection.y) == 1))||((Mathf.Abs(mainDirection.y) == 1)&&(Mathf.Abs(mainFaceDirection.z) == 1))){
			FindCubesAndPoint ("x", Mathf.RoundToInt(center.x));
			clockwise = -Mathf.RoundToInt(Vector3.Cross (mainDirection, mainFaceDirection).x);
		}

	}
	Vector3 findMainDirection (Vector3 direction){
		float xabs = Mathf.Abs (direction.x);
		float yabs = Mathf.Abs (direction.y);
		float zabs = Mathf.Abs (direction.z);
		float maxNumber = Mathf.Max (xabs, yabs, zabs);
		if (xabs == maxNumber) {
			return new Vector3 (Mathf.Round (direction.x / xabs), 0, 0);
		} else if (yabs == maxNumber) {
			return new Vector3 (0, Mathf.Round (direction.y / yabs), 0);
		} else {
			return new Vector3 (0, 0, Mathf.Round (direction.z / zabs));
		}
	}

	void Rotate(Vector3 rotateDirection){
		for (int i = 0; i < cubeToChange.Count; i++) {
			cubeToChange [i].transform.RotateAround (rotateCenter, rotateDirection, clockwise * targetAmount);
		}
		targetAngle -= targetAmount;

	}

	void Arrange(){
		for (int i = 0; i < cubes.Length; i++) {
			float arrangementX = Mathf.Round (cubes [i].transform.localPosition.x);
			float arrangementY = Mathf.Round (cubes [i].transform.localPosition.y);
			float arrangementZ = Mathf.Round (cubes [i].transform.localPosition.z);
			Vector3 arragementPosition = new Vector3 (arrangementX, arrangementY, arrangementZ);
			cubes [i].transform.localPosition = arragementPosition;
//			arrangementX = Mathf.Round (cubes [i].transform.lo.x);
//			arrangementY = Mathf.Round (cubes [i].transform.localPosition.y);
//			arrangementZ = Mathf.Round (cubes [i].transform.localPosition.z);

		}
	}
}
