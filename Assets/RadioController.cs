using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RadioController : MonoBehaviour {


    public AudioSource Radio;
    public AudioClip[] RadioSongs;
    public AudioClip Static;

    private int selectedSong;

    private bool isStatic;
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
        selectedSong = (int)Mathf.Round(Random.Range(0.0f, (RadioSongs.Length - 1)));
        Radio.clip = RadioSongs[selectedSong];
        Radio.loop = true;
        Radio.Play();
        Debug.Log(RadioSongs[selectedSong].name + " " + Radio.isPlaying);
        isStatic = false;
        //Static = (AudioClip)Resources.Load("Assets/Main Assets/Sounds/static.wav"); 
    }

    private void Start()
    {
        OnSceneLoaded(new Scene(), LoadSceneMode.Additive);
    }

    // Update is called once per frame
    void Update () {
        float trackSelect = Input.GetAxis("Tune");
        if (isStatic)
        {
            if (!Radio.isPlaying)
            {
                Debug.Log(selectedSong +" "+ Time.frameCount);
                float songLength = RadioSongs[selectedSong].length;
                songLength = Time.time % songLength;
                Radio.loop = true;
                Radio.clip = RadioSongs[selectedSong];
                isStatic = false;
                Radio.Play();
                Radio.time = songLength;
            }
        }
        if (trackSelect != 0 & !isStatic)
        {
            Radio.time = 0;
            Radio.Stop();
            selectedSong += (int)Mathf.Round(trackSelect);
            if (selectedSong > RadioSongs.Length-1)
                selectedSong = 0;
            if (selectedSong < 0)
                selectedSong = RadioSongs.Length-1;
            Radio.loop = false;
            Radio.clip = Static;
            isStatic = true;
            Radio.Play();
        }
    }
}
