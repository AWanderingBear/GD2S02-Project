using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {

    public Transform Player1;
    public Transform Player2;
    public Vector3 center;
  

    void Update()
    {
        center = (Player1.position + (Player2.position - Player1.position) / 2.0f);
        center.z = -10;
        transform.position = center;
        
    }
}
