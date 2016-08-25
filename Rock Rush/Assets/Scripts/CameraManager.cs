using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {

    public Transform Player1;
    public Transform Player2;
    public Vector3 center;
    public Vector3 Player1Vec;
    public Vector3 Player2Vec;
    public Vector3 respawnPos = new Vector3(0.0f, 7.0f, 0.0f);

    void Update()
    {
        if (GameObject.Find("Player1") != null && GameObject.Find("Player2") != null)
        {
            center = (Player1.position + (Player2.position - Player1.position) / 2.0f);
            center.z = -10;
            transform.position = center;
        }
        if (GameObject.Find("Player1") == null)
        {
            Player2Vec = Player2.position;
            Player2Vec.z = -10;
            transform.position = Player2Vec;
        }

        if (GameObject.Find("Player2") == null)
        {
            Player1Vec = Player1.position;
            Player1Vec.z = -10;
            transform.position = Player1Vec;
        }

        if (GameObject.Find("Player1") == null && GameObject.Find("Player2") == null)
        {
            transform.position = respawnPos;
        }
    }
}
