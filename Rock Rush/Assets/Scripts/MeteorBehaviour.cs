using UnityEngine;
using System.Collections;

public class MeteorBehaviour : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Player1")
        {
            var player = other.GetComponent<Player1>();
            player.knockBackCount = player.knockBackLength;

            if (other.transform.position.x < transform.position.x)
            {
                player.knockFromRight = true;
            }
            else
            {
                player.knockFromRight = false;
            }
        }

        if (other.name == "Player2")
        {
            var player = other.GetComponent<Player2>();
            player.knockBackCount = player.knockBackLength;

            if (other.transform.position.x < transform.position.x)
            {
                player.knockFromRight = true;
            }
            else
            {
                player.knockFromRight = false;
            }
        }
    }
}
