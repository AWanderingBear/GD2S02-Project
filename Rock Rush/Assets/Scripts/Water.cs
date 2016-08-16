using UnityEngine;
using System.Collections;

public class Water : MonoBehaviour {
    public Vector2 addingForce;
    // Use this for initialization
    void Start () {
        addingForce = new Vector2(10.0f, 10.0f);
    }
	
	// Update is called once per frame
	void Update () {
	
	}


    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == xa.Player) 
        {
            other.gameObject.GetComponent<Rigidbody2D>().AddForce(addingForce);
        }
    }
}
