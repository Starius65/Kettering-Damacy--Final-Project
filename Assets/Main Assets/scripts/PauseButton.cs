using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButton : MonoBehaviour {

    private float minimum;
    private float maximum;
    private float threshold;
    public Material ActiveMaterial;
    public Material InactiveMaterial;
    public bool _isPushed;
    private GameObject[] pause;
    private bool paused;
    private AudioSource music;
    private bool IsPushed { get { return _isPushed; } set {
            _isPushed = value;
            if (_isPushed)
            {
                foreach (GameObject o in pause)
                    o.GetComponent<MeshRenderer>().material = ActiveMaterial;
                if (paused)
                    music.UnPause();
                else
                    music.Play();
            }
            else
            {
                foreach (GameObject o in pause)
                    o.GetComponent<MeshRenderer>().material = InactiveMaterial;
                music.Pause();
            }
        } }
    private bool beingPushed;
    private Vector3 basePosition;

    // Use this for initialization
    void Start () {
        Physics.IgnoreLayerCollision(11, 12);
        maximum = transform.localPosition.x;
        minimum = transform.localPosition.x - 0.15f;
        basePosition = transform.position;
        music = GetComponent<AudioSource>();
        pause = new GameObject[] { transform.GetChild(0).gameObject, transform.GetChild(1).gameObject };
        beingPushed = false;
        threshold = (maximum - minimum) * (3f / 4f);
        if (IsPushed)
            foreach (GameObject o in pause)
                o.GetComponent<MeshRenderer>().material = ActiveMaterial;
        if (IsPushed)
        {
            GetComponent<AudioSource>().Play();
            paused = true;
        }
        else
            paused = false;

    }
	
	// Update is called once per frame
	void Update () {
        if (transform.localPosition.x > maximum)
            transform.position = basePosition;
        if (transform.localPosition.x < minimum)
            transform.localPosition = new Vector3(minimum, transform.localPosition.y, transform.localPosition.z);
        if (transform.localPosition.x >= minimum && transform.localPosition.x != maximum)
            transform.position = Vector3.Lerp(transform.position, basePosition, Time.deltaTime*2f);
        if (maximum - transform.localPosition.x <= 0.01f)
            transform.position = basePosition;
        if ((transform.localPosition.x-minimum) < threshold & !beingPushed)
        {
            beingPushed = true;
            IsPushed = !IsPushed;
        }
        if (beingPushed & transform.position == basePosition)
            beingPushed = false;
    }
}
