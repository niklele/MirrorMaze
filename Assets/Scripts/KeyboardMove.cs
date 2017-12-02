using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardMove : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
    
    public float rotationSpeed = 100.0F;
    void FixedUpdate()
    {
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed;
        rotation *= Time.deltaTime;
        transform.Rotate(0, rotation, 0);
    }
}
