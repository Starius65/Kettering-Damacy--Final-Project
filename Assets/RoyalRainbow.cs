using Klonamari;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoyalRainbow : MonoBehaviour {

    public GameObject katamari;

    bool edge;
    Vector2 start;
    Vector2 max;

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
        edge = false;
        start = GetComponent<RectTransform>().sizeDelta;
        max = new Vector2(1038*1.5f, 509*1.5f);
    }

    private void Start()
    {
        OnSceneLoaded(new Scene(), LoadSceneMode.Additive);
    }

    // Update is called once per frame
    void Update () {
        if (!edge)
            GetComponent<RectTransform>().sizeDelta = Vector2.Lerp(GetComponent<RectTransform>().sizeDelta, max, Time.deltaTime);
        if (edge)
            GetComponent<RectTransform>().sizeDelta = Vector2.Lerp(GetComponent<RectTransform>().sizeDelta, new Vector2(1038 * -.5f, 509 * -.5f), Time.deltaTime);
        if (GetComponent<RectTransform>().sizeDelta.y > 650)
        {
            edge = true;
            katamari.GetComponent<Katamari>().BecomeStar(true);
            katamari.SetActive(false);
        }
        if (GetComponent<RectTransform>().sizeDelta.magnitude < 25 && edge)
            SceneManager.LoadScene("IntroScene");
    }
}
