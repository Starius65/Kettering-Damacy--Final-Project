using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Klonamari;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour {

    public GameObject speech;
    public Image FadeIn;
    public Image RoyalRainbow;
    public GameObject star;
    public MeshRenderer screen;
    public GameObject floatingKing;
    public GameObject UI;
    public AudioClip[] blah;
    public AudioClip rainbow;
    public AudioClip musicStar;
    public AudioClip musicMake;
    static public GameObject katamari;
    public Camera cam;
    public GameObject setKata;
    public AudioSource musicsource;
    public Text size;
    public Text Title;

    public float timer;
    float startTime;
    Renderer background;
    int currentLine;
    public Image Doggo;
    int blahNum;
    int specialIndex;
    int dialogueSelect;

    Vector3 TextStarPos;
    Vector3 DogStarPos;

    float speechTimer;
    float floatTimer;
    float a;

    bool shrink;
    bool staractive;
    bool fadeup;
    bool textFlag;

    enum LevelState { Default, Intro, BeginDialogue, Gameplay, Special, Check, Finish, TimeUp, LevelComplete, StarCreate, End }

    static LevelState status;

    static List<List<string>> Dialogue;

    private void Awake()
    {
        Dialogue = new List<List<string>>();
        string path = "Assets/Dialogue.txt";
        StreamReader reader = new StreamReader(path);
        while (!reader.EndOfStream)
        {
            List<string> diaLine = new List<string>();
            string line = reader.ReadLine();
            string[] phrase = line.Split('|');
            foreach (string l in phrase)
            {
                diaLine.Add(l);
            }
            Dialogue.Add(diaLine);
        }
        Start();
    }
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
    currentLine = 0;
        Debug.Log(scene.name + " . " + SceneManager.GetActiveScene().name +" > "+ SceneManager.GetSceneByBuildIndex(0).name);
        if (status == LevelState.Default && SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(1))
        {
            status = LevelState.Intro;
            katamari = GameObject.Find("Katamari");
            shrink = false;
            blahNum = 0;
            TextStarPos = speech.GetComponent<RectTransform>().localPosition;
            DogStarPos = Doggo.rectTransform.localPosition;
            specialIndex = 0;
        }
        if (status == LevelState.TimeUp)
        {
            status = LevelState.LevelComplete;
            katamari.gameObject.SetActive(true);
            float scaleSize = katamari.GetComponent<Katamari>().SizeFloat()*2;
            scaleSize = (-4.5f * Mathf.Atan(.5f * scaleSize - 5) + 10);
            katamari.transform.localScale = new Vector3(scaleSize, scaleSize, 0.1f);
            katamari.transform.localRotation = new Quaternion(0,0,0,0);
            katamari.transform.position = setKata.transform.position;
            setKata.SetActive(false);
            speech.GetComponent<RectTransform>().localPosition =
                new Vector3(0, 170, 0);
            cam.transform.position = new Vector3(
                -2000,
                cam.transform.position.y,
                cam.transform.position.z);
            dialogueSelect = 2;
            speech = speech.transform.GetChild(0).gameObject;
            musicsource.clip = musicStar;
            speech.GetComponentInChildren<Text>().alignment = TextAnchor.MiddleCenter;
            size.text = katamari.GetComponent<Katamari>().SizeFormatted();
            musicsource.Play();
            staractive = false;
            textFlag = false;
            fadeup = true;
            a = 0;
            Title.text = "Kettering Star";
        }
    }


    private void Start()
    {
        OnSceneLoaded(new Scene(), LoadSceneMode.Additive);
    }

    // Update is called once per frame
    void Update() {
        switch (status)
        {
            case LevelState.Intro:
                if (Time.time < timer)
                {
                    float mult = (timer - Time.time) / timer;
                    FadeIn.color = new Color(
                        FadeIn.color.r,
                        FadeIn.color.g,
                        FadeIn.color.b,
                        mult);
                }
                else
                {
                    status = LevelState.BeginDialogue;
                    FadeIn.gameObject.SetActive(false);
                }
                break;
            case LevelState.BeginDialogue:
                if(blahNum < 2)
                {
                    if (!GetComponent<AudioSource>().isPlaying)
                    {
                        blahNum++;
                        NewRecordScratch();
                    }
                }
                if (currentLine < Dialogue[0].Count)
                {
                    speech.GetComponentInChildren<Text>().text = Dialogue[0][currentLine];
                }
                else
                {
                    status = LevelState.Gameplay;
                    currentLine = 0;
                    UI.SetActive(true);
                    speech.GetComponent<RectTransform>().localPosition =
                        new Vector3(115, 180, 0);
                    speech.gameObject.SetActive(false);
                }
                if (Input.GetButtonDown("Jump"))
                {
                    blahNum = 0;
                    currentLine++;
                    NewRecordScratch();
                    blahNum++;
                }
                break;
            case LevelState.Gameplay:
                if (!shrink)
                {
                    if (Doggo.rectTransform.localScale.magnitude > .02f)
                        Doggo.rectTransform.localScale = Vector3.Lerp(
                            Doggo.rectTransform.localScale,
                            new Vector3(0.0f, 0.0f, 0.0f),
                            Time.deltaTime*3);
                    else
                    {
                        katamari.GetComponent<Katamari>().ControlSwitch(true);
                        shrink = true;
                        Doggo.rectTransform.localScale = Vector3.one;
                        Doggo.rectTransform.localPosition = new Vector3(
                            -362, 157, 0);
                        Doggo.gameObject.SetActive(false);
                        
                    }
                }
                break;
            case LevelState.Special:
                if(Time.time - speechTimer > 4.0f)
                {
                    Dialogue[specialIndex].Clear();
                    Doggo.gameObject.SetActive(false);
                    speech.gameObject.SetActive(false);

                    status = LevelState.Gameplay;
                }
                break;
            case LevelState.TimeUp:
                if (blahNum < 2)
                {
                    if (!GetComponent<AudioSource>().isPlaying)
                    {
                        blahNum++;
                        NewRecordScratch();
                    }
                }
                /*if (shrink)
                {
                    Debug.Log(Doggo.rectTransform.localScale);
                    if (Doggo.rectTransform.localScale.magnitude != 1)
                        Doggo.rectTransform.localScale = Vector3.Lerp(
                            Doggo.rectTransform.localScale,
                            new Vector3(1.0f, 1.0f, 1.0f),
                            1);
                    else
                    {
                        shrink = false;
                        speech.gameObject.SetActive(true);
                    }
                    Debug.Log(Doggo.rectTransform.localScale);
                }*/
                //else if (!shrink)
                //{
                if (currentLine < Dialogue[1].Count-1)
                {
                    speech.GetComponentInChildren<Text>().text = Dialogue[1][currentLine];
                }
                else
                {
                    speech.GetComponentInChildren<Text>().text = Dialogue[1][currentLine];
                    RoyalRainbow.gameObject.SetActive(true);
                }
                if (Input.GetButtonDown("Jump"))
                {
                    if(currentLine < Dialogue[1].Count - 1)
                        currentLine++;
                    if(currentLine == Dialogue[1].Count-1)
                    {
                        GetComponent<AudioSource>().clip = rainbow;
                        GetComponent<AudioSource>().Play();
                    }
                    else
                    {
                        blahNum = 0;
                        NewRecordScratch();
                        blahNum++;

                    }
                }
                //}

                break;
            case LevelState.LevelComplete:
                if (blahNum < 2)
                {
                    if (!GetComponent<AudioSource>().isPlaying)
                    {
                        blahNum++;
                        NewRecordScratch();
                    }
                }
                if (currentLine < Dialogue[dialogueSelect].Count)
                {
                    if (Dialogue[dialogueSelect][currentLine].Contains("[Size]"))
                        Dialogue[dialogueSelect][currentLine] = Dialogue[dialogueSelect][currentLine].Replace("[Size]", 
                            katamari.GetComponent<Katamari>().SizeFormatted());
                    speech.GetComponentInChildren<Text>().text = Dialogue[dialogueSelect][currentLine];
                }
                else
                {
                    if (dialogueSelect < 3)
                    {
                        dialogueSelect++;
                        currentLine = 0;
                        if (dialogueSelect == 3 && katamari.GetComponent<Katamari>().SizeFloat()*2 < 1)
                        {
                            dialogueSelect++;
                        }
                    }
                    else
                    {
                        status = LevelState.StarCreate;
                        speech.transform.parent.gameObject.SetActive(false);
                        floatTimer = Time.time;
                        musicsource.loop = false;
                        musicsource.Stop();
                        musicsource.clip = musicMake;
                        musicsource.Play();
                    }
                }
                if (Input.GetButtonDown("Jump"))
                {
                    blahNum = 0;
                    currentLine++;
                    NewRecordScratch();
                    blahNum++;
                }
                break;
            case LevelState.StarCreate:
                floatingKing.transform.localPosition = Vector3.Lerp(floatingKing.transform.localPosition, new Vector3(0, -1.5f, 0), Time.deltaTime*0.2f);
                if (Time.time - floatTimer > 4.75f)
                    staractive = true;
                Debug.Log(Time.time - floatTimer);
                if (staractive)
                {
                    if (fadeup)
                    {
                        screen.material.color = new Color(1, 1, 1, a);
                        a += 3 * Time.deltaTime;
                        if (a > 1.0f)
                        {
                            fadeup = false;
                            star.SetActive(true);
                            katamari.SetActive(false);
                        }
                    }
                    if (!fadeup)
                    {
                        a -= 3 * Time.deltaTime;
                        if (a <= 0.0f)
                        {
                            staractive = false;
                            status = LevelState.End;
                            floatTimer = Time.time;
                        }
                    }
                    screen.material.color = new Color(1, 1, 1, a);
                }
                break;
            case LevelState.End:
                if(Time.time - floatTimer > 6f)
                {
                    Title.text = "Press Enter To Play Again";
                }
                break;


            default: break;
        }
    }

    void NewRecordScratch()
    {
        GetComponent<AudioSource>().clip = blah[Random.Range(0, blah.Length)];
        GetComponent<AudioSource>().Play();
    }

    public void SpecialItem(int index)
    {
        specialIndex = index;
        if (specialIndex == 0)
            return;
        specialIndex += 4;
        if (Dialogue[specialIndex].Count > 0)
        {
            speech.GetComponentInChildren<Text>().text = Dialogue[specialIndex][0];
            status = LevelState.Special;
            speechTimer = Time.time;
            Doggo.gameObject.SetActive(true);
            transform.GetChild(2).gameObject.SetActive(true);
        }
    }
    public void TimeUp()
    {
        katamari.GetComponent<Katamari>().ControlSwitch(false);
        status = LevelState.TimeUp;
        Doggo.rectTransform.localPosition = DogStarPos;
        Doggo.rectTransform.localScale = Vector3.one;
        Doggo.gameObject.SetActive(true);
        speech.gameObject.SetActive(true);
        speech.GetComponent<RectTransform>().localPosition = TextStarPos;
        shrink = true;
    }
}
