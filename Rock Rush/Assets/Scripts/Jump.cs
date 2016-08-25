using UnityEngine;
using System.Collections;

public class Jump : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}


    public float speed;
    public Transform target;

    private Vector3 zAxis = new Vector3(0, 0, 1);

    void Update()
    {
        transform.RotateAround(target.position, zAxis, speed);
    }
}
