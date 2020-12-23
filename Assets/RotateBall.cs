using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBall : MonoBehaviour {

    public int RotateScale;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        GetComponent<Transform>().Rotate(new Vector3(0, 0, Mathf.PI*Time.deltaTime*RotateScale));
	}
}
