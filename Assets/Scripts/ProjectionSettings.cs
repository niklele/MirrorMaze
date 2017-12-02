using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectionSettings : MonoBehaviour {

    public enum Type { Wall=0, Mirror, Lens, Portal, Prism, Target }

    public Type type;

    public Vector3 scaling;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
