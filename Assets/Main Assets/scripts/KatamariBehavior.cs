using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KatamariBehavior : MonoBehaviour {

    public AudioClip[] pops;
    public Text DebugMass;
    public Image Circle;
    private AudioSource sounds;
    private LinkedList<PickupObject> allObjects;
    private LinkedList<PickupObject> outliers;
    private float averageOffset;
    private float size;

	// Use this for initialization
	void Start () {
        Physics.IgnoreLayerCollision(9, 10);
        //pops = new AudioSource[2] { GetComponent<AudioSource>(), GetComponent<AudioSource>() };
        sounds = GetComponent<AudioSource>();
        allObjects = new LinkedList<PickupObject>();
        outliers = new LinkedList<PickupObject>();
        averageOffset = 0.0f;
    }
	
	// Update is called once per frame
	void Update () {

        size = Circle.GetComponent<Transform>().localScale.x;

        //DEBUG CRAP
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            GetComponent<Rigidbody>().mass += 1;
            GetComponent<Transform>().localScale += Vector3.one/10;
            DebugMass.text = "Mass: " + GetComponent<Rigidbody>().mass;
            Circle.GetComponent<Transform>().localScale = Vector3.one * (GetComponent<Rigidbody>().mass * size);
        }
        
        if (Input.GetKeyDown(KeyCode.DownArrow) & GetComponent<Rigidbody>().mass > 1)
        {
            GetComponent<Rigidbody>().mass -= 1;
            GetComponent<Transform>().localScale -= Vector3.one/10;
            DebugMass.text = "Mass: " + GetComponent<Rigidbody>().mass;
            Circle.GetComponent<Transform>().localScale = Vector3.one * (GetComponent<Rigidbody>().mass * size);
        }
    }

    public void OnAddObject(GameObject obj, float offset)
    {

        //Calculating mass (obscelete?)
        GetComponent<Rigidbody>().mass += obj.GetComponent<Rigidbody>().mass / 10;
        //GetComponentInParent<PlayerController>().forceMod += GetComponent<Rigidbody>().mass / 100;
        //DebugMass.text = "Mass: " + GetComponent<Rigidbody>().mass;
        Circle.GetComponent<Transform>().localScale = Vector3.one * (GetComponent<Rigidbody>().mass * size);

        //Sound effects
        int randomClip = (int)Random.Range(0f, pops.Length);
        sounds.clip = pops[randomClip];
        sounds.Play();

        //Offset calculation
        if ((int)(offset * 100f) > 0)   
            offset = 0;
        PickupObject current = new PickupObject(offset, obj);
        allObjects.AddLast(current);
        averageOffset = ((averageOffset * (allObjects.Count - 1)) + offset) / allObjects.Count;
        Debug.Log(averageOffset + " " + offset);
        if (offset > averageOffset)
        {
            outliers.AddLast(current);
            Debug.Log("added. " + outliers.Count);
        }
    }
}

public class PickupObject {
    
    private float Offset;
    private GameObject Object;

    public PickupObject(float o, GameObject Object)
    {
        Offset = o;
        this.Object = Object;
    }
}
