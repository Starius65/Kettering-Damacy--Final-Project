using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour {

    public MeshRenderer FadeOut;
    bool startFade;
    public Transform shroud;
    public Canvas ui;
    public GameObject[] select;
    public Text textbox;
    public AudioSource music;
    bool selection;
    bool isDown;
    bool timerStart;
    float timer;

    static bool isStart = true;

    float a;

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
        if(!isStart)
        {
            select[0].SetActive(false);
            select[1].SetActive(false);
        }
        a = 0;
        startFade = false;
        selection = true;
        isDown = false;
        timerStart = false;
    }

    private void Start()
    {
        OnSceneLoaded(new Scene(), LoadSceneMode.Additive);
    }

    // Update is called once per frame
    void Update () {
        if (ui.isActiveAndEnabled)
        {
            if (Input.GetAxis("Submit") > 0 && selection)
            {
                startFade = true;
                textbox.text = "";
                select[0].SetActive(false);
                select[1].SetActive(false);
                isStart = false;
                music.Stop();
            }

            if (Input.GetAxis("Vertical") != 0)
            {
                if (!isDown)
                {
                    isDown = true;
                    selection = !selection;
                    select[0].SetActive(selection);
                    select[1].SetActive(!selection);
                }
            }
            else
                isDown = false;



            if (startFade)
            {
                bool inPosition = false;
                if (!timerStart)
                    inPosition = true;
                if (shroud.localScale.x < 1.43f)
                {
                    shroud.localScale = Vector3.Lerp(shroud.localScale, shroud.localScale * 5f, Time.deltaTime);
                    shroud.localPosition = Vector3.Lerp(shroud.localPosition, new Vector3(
                        shroud.localPosition.x,
                        shroud.localPosition.y - shroud.localScale.y * 2,
                        shroud.localPosition.z), Time.deltaTime);
                    inPosition = false;
                }
                FadeOut.material.color = new Color(0, 0, 0, a);
                if (a < 1)
                    a += 2f * Time.deltaTime;
                if(transform.position.y > 150)
                {
                    transform.position = Vector3.Lerp(transform.position, new Vector3(0, 1, 0), Time.deltaTime);
                    inPosition = false;
                }
                if (inPosition)
                {
                    textbox.text = "Sending things to earth...";
                    timerStart = true;
                    timer = Time.time;
                    inPosition = false;
                }
                if (timerStart & ((Time.time - timer) > 2.0f))
                {
                    Debug.Log("DONE");
                    SceneManager.LoadScene("Kettering");
                }
                Debug.Log((Time.time - timer));

            }
        }

	}
}
