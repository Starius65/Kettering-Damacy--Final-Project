using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour {

    const float timeUp = 300f;
    float startTime;
    Image wheel;
    public Text num;

    // Use this for initialization
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
        startTime = Time.time;
        wheel = GetComponent<Image>();
	}

    private void Start()
    {
        OnSceneLoaded(new Scene(), LoadSceneMode.Additive);
    }

    // Update is called once per frame
    void Update () {
        float currentTime = Time.time - startTime;
        wheel.fillAmount = (timeUp - currentTime) / timeUp;
        float timeInMinuites = Mathf.Round((timeUp - currentTime) / 60f);
        num.text = timeInMinuites.ToString().Substring(0,1);
        if(currentTime > timeUp)
            GameObject.Find("King").GetComponent<StartGame>().TimeUp();
    }
}
