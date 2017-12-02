using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Creates the tree of image projections starting from the key
 */
public class ManageProjections : MonoBehaviour {

    private Quaternion currRotation = Quaternion.identity;

    public GameObject key;
    public float maxDistance;

    public GameObject imageObject;
    public bool drawImage;

    private List<GameObject> imageObjects = new List<GameObject>();

    // each node has:
    // - input image
    // - scaling factors (lenses)
    // - transposition factors (portals)
    // - output ray (reflections)
    // - brightness factors?
    // get them from project settings of the obj
    // use that to calculate the output ray

    class ObjRay {
        public GameObject inputObj;
        public Vector3 inputPos;
        public Vector3 outputRay;

        public ObjRay(GameObject inputObj, Vector3 inputPos, Vector3 outputRay)
        {
            this.inputObj = inputObj;
            this.inputPos = inputPos;
            this.outputRay = outputRay;
        }

        // transformations to inputObj are inside the projection settings of the object
        // output ray is calculated based on those transformations

    }

    void CastRays(ObjRay start) {

        Queue<ObjRay> rayQueue = new Queue<ObjRay>();
        rayQueue.Enqueue(start);

        while (rayQueue.Count > 0) {

            ObjRay curr = rayQueue.Dequeue();
            RaycastHit hit;

            if (Physics.Raycast(origin: curr.inputPos, direction: curr.outputRay, maxDistance: maxDistance, hitInfo: out hit))
            {
                if (drawImage) {
                    imageObject = Instantiate(imageObject) as GameObject;
                    imageObject.transform.position = hit.point;
                    imageObject.transform.rotation = curr.inputObj.transform.rotation;
                    imageObjects.Add(imageObject);
                }

                // look at hitinfo
                //print(hit.point);
                //print(hit.collider.gameObject.name);
            
                GameObject hitObj = hit.collider.gameObject;
                var type = hitObj.GetComponent<ProjectionSettings>().type;
                switch(type) {
                    case ProjectionSettings.Type.Wall: break;
                    case ProjectionSettings.Type.Mirror:
                        Vector3 nextOutputRay = Vector3.Reflect(curr.outputRay, hit.normal).normalized;
                        rayQueue.Enqueue(new ObjRay(curr.inputObj, hitObj.transform.position, nextOutputRay));
                        break;
                    case ProjectionSettings.Type.Lens: break;
                    case ProjectionSettings.Type.Portal: break;
                    case ProjectionSettings.Type.Prism: break;
                }
                
            }
        }
    }

    void HandleMirror() {
        // Vector3 reflection = Vector3.reflect(incoming, hit.normal)
    }

	// Use this for initialization
	void Start () {
        currRotation = key.transform.rotation;

        // add key as start node
        CastRays(new ObjRay(key, key.transform.position, Vector3.forward));
	}
	
	void FixedUpdate () {
		if (key.transform.rotation != currRotation)
        {
            // delete old image objects
            imageObjects.RemoveAll(delegate (GameObject g)
            {
                Destroy(g);
                return true;
            });

            // update and cast rays again
            currRotation = gameObject.transform.rotation;
            CastRays(new ObjRay(key, key.transform.position, Vector3.forward));
        }
	}
}
