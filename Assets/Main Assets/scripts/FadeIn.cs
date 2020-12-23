using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeIn : MonoBehaviour {

    public float timer;
    public GameObject can;
    float startTime;
    Renderer background;
    bool isComplete;

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        background = GetComponentInChildren<Renderer>();
        Debug.Log(background.material.color);
        background.material.color = Color.black;
        Debug.Log(background.material.color);
        isComplete = false;
    }

    private void Start()
    {
        OnSceneLoaded(new Scene(), LoadSceneMode.Additive);
    }

    // Update is called once per frame
    void Update () {
        if (!isComplete)
        {
            if (Time.time < timer)
            {
                float mult = Time.time / timer;
                background.material.color = new Color(mult, mult, mult);
            }
            else
            {
                isComplete = true;
                can.SetActive(true);
            }
        }
	}
}
