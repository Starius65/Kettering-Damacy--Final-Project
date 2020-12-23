using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBehavior : MonoBehaviour {

    public bool collected;
    Rigidbody rB;

    public float volume { get; private set; } //honestly, volume should probably be calculated, depending on the mesh we're using. maybe just collider bounds size.
    public float density;
    public float mass { get; private set; }
    public string displayName;

    // Use this for initialization
    void Start () {
        rB = GetComponent<Rigidbody>();
        Vector3 size = transform.lossyScale;
        volume = size.x * size.y * size.z;
        rB.mass = mass = volume * density;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Katamari" & tag == "PickupObject")
        {
            GameObject katamari = collision.gameObject;
            if (katamari.GetComponent<Rigidbody>().mass > GetComponent<Rigidbody>().mass)
            {
                transform.SetParent(collision.transform, true);
                tag = "Untagged";
                Destroy(GetComponent<Rigidbody>());

                //collision.contacts[0].
                transform.position = collision.contacts[0].point; // TURINIG: STORE THIS POINT APART, AND VISUALIZE IT USING GIZMOS...

                /*Vector3 distance = (transform.position - katamari.transform.position);
                Vector3 angles3D = new Vector3(
                    Mathf.Acos(distance.x / distance.magnitude),
                    Mathf.Acos(distance.y / distance.magnitude),
                    Mathf.Acos(distance.z / distance.magnitude)
                    );
                Vector3 radius = katamari.transform.position - collision.contacts[0].point;
                float newDistance = distance.magnitude - radius.magnitude;
                Vector3 newLocation = new Vector3(
                    newDistance*Mathf.Cos(angles3D.x),
                    newDistance*Mathf.Cos(angles3D.y),
                    newDistance*Mathf.Cos(angles3D.z)                    
                    );
                transform.Translate(-newLocation);
                
                Debug.Log("radius: " + radius + "\tnewDistance: " +
                    newDistance + "\tnewLocation: " + newLocation +
                    "\tdistance: " + distance + "\ntransform.position: " + transform.position +
                    "\tkatamari.transform.position: " + katamari.transform.position + "\tthing: " + name +
                    "\tcollision.contacts[0].point: " + collision.contacts[0].point);*/
                // TODO: Move the colliding object toward the center of the sphere.      Maybe fixed??

                katamari.GetComponent<KatamariBehavior>().OnAddObject(gameObject, collision.contacts[0].separation);
                //katamari.GetComponentInParent<PlayerController>().NewMax(newDistance);
            }
        }
    }
}
