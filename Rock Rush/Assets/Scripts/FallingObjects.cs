using UnityEngine;
using System.Collections;

public class FallingObjects : MonoBehaviour {
    public float delay = 10.0f;
    public GameObject Meteor;
    public GameObject Diamond;
    public Transform T_Meteor;

	// Use this for initialization
	void Start () {
        InvokeRepeating("SpawnMeteors", 5.0f, delay);
    }

    void Update()
    {
        
    }
	
	void SpawnMeteors()
    {
        GameObject clone = (GameObject)Instantiate(Meteor, new Vector3(Random.Range(-15, 15), 10, 0), Quaternion.identity); // random x coordinate
        Destroy(clone, 10.0f); // destroy object after x seconds after spawning
    }
}
