using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public GameObject cubeA;
	public GameObject cubeB;
	public float speed;

	float rotationSpeed;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
//		rotationSpeed += speed * Time.deltaTime;
//		cubeA.transform.Rotate(Vector3.right, rotationSpeed);  
//		cubeB.transform.Rotate(Vector3.right, rotationSpeed); 
//		Vector3 Vector3X = new Vector3(1,0,0);
//		cubeA.transform.Rotate(Vector3X, rotationSpeed, Space.World);
//		cubeB.transform.Rotate(Vector3X, rotationSpeed, Space.World);

		Vector3 middlePosition = (cubeA.transform.position + cubeB.transform.position) / 2;

		cubeA.transform.RotateAround (middlePosition, Vector3.right, speed);
		cubeB.transform.RotateAround (middlePosition, Vector3.right, speed);
	}
}
