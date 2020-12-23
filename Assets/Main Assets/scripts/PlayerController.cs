//using UnityEngine;
//using UnityEngine.UI;
//using System.Collections;
//using System.Collections.Generic;
//using Klonamari;

//public class PlayerController : MonoBehaviour {

//    public float speed;
//    public float MaxSpeed;
//    public GameObject DebugSpeed;

//    private enum BoostState { Disabled, Running, PowerOff };

//    private Rigidbody rb;
//    private GameObject Katamari;
//    private GameObject Cam;
//    private Vector3 initialOffset;
//    private Vector3 offset;
//    private Vector3 deltaVelocity;
//    private float boostCountdown;
//    private float dir;
//    private float multiplier;
//    private float newOffset;
//    private float speedMod;
//    private BoostState speedState;

//    public float forceMod;

//    private const int dirMag = 100;
//    private const float boostTimer = 1.5f;


//    #region EXTERNAL CODE

//    private const float ONE_THIRD = 1.0f / 3.0f;

//    public float ROLL_UP_MAX_RATIO = 0.25f; //NOTE that this isn't talking about rolling up stairs. the game's lingo uses this for collection.

//    public float TORQUE_MULT = 1500.0f;
//    public float FORCE_MULT = 500.0f;
//    public float AIRBORNE_FORCE_MULT = 250.0f;
//    public float UPWARD_FORCE_MULT = 1000.0f;
//    public float STAIR_CLIMB_RATIO = 2.15f; // you can climb sheer walls STAIR_CLIMB_RATIO * radius above initial contact. if it's taller than that, you're falling down.
//    public float BREAK_OFF_THRESHOLD = 10.0f;

//    public float ROTATION_MULTIPLIER;

//    //private KatamariInput katamariInput;
//    //public CameraBoom follow;
//    public AudioSource audioSource;

//    public Rigidbody rB;
//    public SphereCollider sphere;
//    public float volume { get; private set; }
//    public float density;
//    public float mass { get; private set; }

//    private List<Transform> touchingClimbables = new List<Transform>();

//    public bool isGrounded { get; private set; }
//    public int defaultContacts {
//        get; private set;
//    }

//    public float rotationY {
//        get; private set;
//    }

//    private List<CollectibleObject> collectibles = new List<CollectibleObject>();
//    private List<CollectibleObject> irregularCollectibles = new List<CollectibleObject>();

//    private Vector3 userInput = Vector3.zero;
//    #endregion


//    void Start()
//    {
//        #region NOT MINE
//        //A note here, I'd rather pull all of this stuff up into a Context class and keep all platform-dependent compilation up there.
//        //the Context class could fill out either a Service Locator or set up bindings for DI. This class would just ask for an instance
//        //of KatamariInput from injection or from the locator instead of calling new here.
////#if UNITY_EDITOR || UNITY_STANDALONE
//        SetInput(new KeyboardInput());
////#elif UNITY_XBOX360 || UNITY_XBOXONE
////            SetInput(new KatamariJoystickInput());
////#endif
//        //other input implementations for mobile, joystick, eye tracking, etc. we could also build a way for the user to select them once we have more.
//        #endregion


//        Katamari = transform.gameObject;
//        //GameObject blah = transform.parent.gameObject;
//        //Cam = blah.transform.GetChild(1).gameObject;
//        //Cam = transform.GetComponentInParent<Transform>().GetChild(1).gameObject;
//        rb = Katamari.GetComponent<Rigidbody>();
//        //initialOffset = Cam.transform.position - Katamari.transform.position;
//        //initialOffset = (initialOffset + Vector3.one);
//        deltaVelocity = new Vector3(0, 0, 0);
//        multiplier = 1;
//        dir = 0.0f;
//        newOffset = 0.0f;
//        rb.maxAngularVelocity = MaxSpeed;
//        speedMod = 1f;
//        speedState = BoostState.Disabled;
//        boostCountdown = 0f;
//    }

//    private void SetInput(KatamariInput input)
//    {
//        katamariInput = input;
//    }

//    private void ProcessInput(Vector3 input)
//    {
//        float forwardInputMultiplier = input.z;
//        float lateralInputMultiplier = input.x;
//        float upwardInputMultiplier = 0.0f;

//        //add an upward force if we're in contact with something we can climb.
//        if ((Mathf.Abs(forwardInputMultiplier) > float.Epsilon || Mathf.Abs(lateralInputMultiplier) > float.Epsilon) && defaultContacts > 0)
//        {
//            upwardInputMultiplier += UPWARD_FORCE_MULT * mass;
//        }

//        float adjustedTorqueMultiplier = TORQUE_MULT * mass;
//        float adjustedForceMultiplier = mass * (isGrounded ? FORCE_MULT : AIRBORNE_FORCE_MULT);
//        Vector3 currentForward = new Vector3(0, rotationY, 0);

//        Vector3 torque = new Vector3(forwardInputMultiplier * adjustedTorqueMultiplier, input.y * adjustedTorqueMultiplier, -lateralInputMultiplier * adjustedTorqueMultiplier);
//        Vector3 force = new Vector3(lateralInputMultiplier * adjustedForceMultiplier, upwardInputMultiplier, forwardInputMultiplier * adjustedForceMultiplier);
//        rB.AddTorque(Quaternion.Euler(currentForward) * torque);
//        rB.AddForce(Quaternion.Euler(currentForward) * force);
//    }

//    void FixedUpdate()
//    {
//        ProcessInput(userInput);
//    }

//    /*

//    void Update()
//    {
//        //rb.maxAngularVelocity = MaxSpeed;

//        float moveCamera = Input.GetAxis("Horizontal");
//        float moveForward = Input.GetAxis("Vertical");
//        float mouseHorizontal = Input.GetAxis("Mouse X");

//        if (Input.GetMouseButton(1))
//        {
//            if (moveCamera > 0)
//                moveCamera = Mathf.Max(mouseHorizontal, moveCamera);
//            else
//                moveCamera = mouseHorizontal;
//        }

//        /*if (Input.GetButtonDown("Speed") & moveForward >= 0.5f & speedState == BoostState.Disabled)
//        {
//            rb.constraints = RigidbodyConstraints.FreezePosition;
//            rb.maxAngularVelocity = 50f;
//            speedState = BoostState.Running;
//        }
//        switch(speedState)
//        {
//            case BoostState.Running:
//                if (Input.GetButton("Speed"))
//                {
//                    speedMod+= 1f;
//                    if (speedMod > 200f)
//                        speedMod = 200f;
//                }
//                else if (!Input.GetButton("Speed"))
//                {
//                    rb.constraints = RigidbodyConstraints.None;
//                    speedState = BoostState.PowerOff;
//                    speedMod = 1f;
//                }
//                break;
//            case BoostState.PowerOff:
//                if (boostCountdown < boostTimer)
//                {
//                    boostCountdown += Time.deltaTime;
//                    rb.maxAngularVelocity -= .2f;
//                }
//                else
//                {
//                    rb.maxAngularVelocity = MaxSpeed;
//                    speedState = BoostState.Disabled;
//                    boostCountdown = 0f;
//                    speedMod = 1f;
//                }
//                break;
//            default:
//                break;
//        //}*

//        isGrounded = Physics.Raycast(transform.position, Vector3.down, sphere.radius + 0.01f);

//        userInput = katamariInput.Update(this);
//        rotationY += userInput.y * Time.deltaTime * ROTATION_MULTIPLIER;

//        dir += (moveCamera * Time.deltaTime * dirMag);
//        if (dir >= 360)
//            dir = 0.0f;
//        if (dir < 0.0f)
//            dir = 360;
//        float radDir = (dir+90f) * Mathf.Deg2Rad;
//        //  moveForward *= forceMod;

//        /*Vector3 movement = new Vector3((moveForward*Mathf.Sin(radDir)), 0.0f, (moveForward * Mathf.Cos(radDir))) * speed;

//        //rb.AddForce(movement, ForceMode.Acceleration);
//        rb.AddTorque(movement);
//        //Vector3 roundedVelocity = new Vector3(Mathf.Round(rb.velocity.x), Mathf.Round(rb.velocity.y), Mathf.Round(rb.velocity.z));
//        //if (roundedVelocity == Vector3.zero && moveForward > 0) {
//        //rb.AddForce(movement * multiplier);
//        //multiplier++;
//        //}
//        //else
//        //    multiplier = 1;
//        //if (multiplier > 100)
//        //    multiplier = 100;
//        DebugSpeed.GetComponent<Text>().text = ""+rb.velocity.magnitude;
//        Debug.Log(movement.magnitude +" "+ speedMod +" "+ rb.angularVelocity.magnitude);
//        offset = Quaternion.AngleAxis(dir, Vector3.up) * (initialOffset + new Vector3(0.0f, initialOffset.normalized.y * newOffset, initialOffset.normalized.z * newOffset));   

//        Cam.transform.LookAt(Katamari.transform);
//        Vector3 newPos = rb.position + offset;
//        Cam.transform.position = Vector3.Slerp(Cam.transform.position, newPos, 0.2f);*
//    }
    
//    */


//    void Update()
//    {
//        isGrounded = Physics.Raycast(transform.position, Vector3.down, sphere.radius + 0.01f);

//        userInput = katamariInput.Update(this);
//        rotationY += userInput.y * Time.deltaTime * ROTATION_MULTIPLIER;

//        follow.UpdatePosition(this);
//    }

//    void OnCollisionEnter(Collision collision)
//    {
//        if (collision.gameObject.layer == 0)
//        {
//            return;
//        }

//        bool rolledUp = OnContact(collision);

//        if (!rolledUp)
//        {
//            Collider hit = collision.rigidbody.GetComponent<Collider>();
//            float targetTop = hit.bounds.extents.y + collision.transform.position.y;
//            float sphereBottom = transform.position.y - sphere.radius;
//            if (collision.gameObject.layer == 9 && targetTop > sphereBottom && sphereBottom + STAIR_CLIMB_RATIO * sphere.radius > targetTop)
//            {
//                ++defaultContacts;
//                touchingClimbables.Add(collision.transform);
//            }
//        }
//    }

//    void OnCollisionExit(Collision collision)
//    {
//        if (touchingClimbables.Contains(collision.transform))
//        {
//            --defaultContacts;
//            touchingClimbables.Remove(collision.transform);
//        }
//    }

//    private bool OnContact(Collision collision)
//    {
//        bool rolledUp = false;
//        Transform t = collision.transform;

//        CollectibleObject collectible = t.GetComponent<CollectibleObject>();
//        if (collectible)
//        {
//            if (collectible.mass < mass * ROLL_UP_MAX_RATIO)
//            {
//                if (!collectible.collected)
//                {
//                    AudioClip clip = collectible.GetRandomCollectAudio();
//                    if (clip)
//                    {
//                        audioSource.PlayOneShot(clip);
//                    }
//                    //collect the thing we hit!
//                    collectible.collected = true;
//                    rolledUp = true;
//                    collectible.gameObject.layer = 9;

//                    collectible.Attach(this);
//                    t.parent = transform;

//                    volume += collectible.volume;
//                    mass += collectible.mass;
//                    rB.mass += collectible.mass;
//                    RecalculateRadius();

//                    collectibles.Add(collectible);

//                    Vector3 delta = (collectible.transform.position - transform.position);
//                    float distance = delta.magnitude - sphere.radius;
//                    Vector3 direction = delta.normalized;

//                    collectible.transform.position = collectible.transform.position - direction * distance;

//                    //EventManager.Attach(collectible, sphere.radius * 2);

//                    //large or irregular objects will make our katamari bounce differently until it grows large enough
//                    if (collectible.IsIrregular(sphere.radius))
//                    {
//                        Destroy(collectible.rB);
//                        irregularCollectibles.Add(collectible);
//                    }
//                    else
//                    {

//                        collectible.rB.mass = 0;
//                        collectible.rB.detectCollisions = false;
//                        collectible.rB.isKinematic = true;
//                    }
//                }
//            }
//            else
//            {
//                //decide how many objects to break off, then break them off.
//                float magnitude = collision.relativeVelocity.magnitude;
//                while (magnitude >= BREAK_OFF_THRESHOLD && collectibles.Count > 0)
//                {
//                    CollectibleObject toRemove = collectibles[collectibles.Count - 1];
//                    OnDetach(toRemove);
//                    magnitude -= 4.0f;
//                }
//            }
//        }
//        return rolledUp;
//    }

//    void OnDetach(CollectibleObject detached)
//    {
//        audioSource.PlayOneShot(detached.detachClip);
//        //this could be improved by using a Dictionary and adding some sort of id to collectibles.
//        collectibles.Remove(detached);
//        if (irregularCollectibles.Contains(detached))
//        {
//            irregularCollectibles.Remove(detached);
//        }

//        if (!detached.IsIrregular(sphere.radius))
//        {
//            rB.mass -= detached.mass;
//        }
//        mass -= detached.mass;
//        volume -= detached.volume;
//        RecalculateRadius();

//        detached.Detach(this);

//        //EventManager.Detach(detached, sphere.radius * 2);
//    }

//    private void RecalculateRadius()
//    {
//        sphere.radius = Mathf.Pow((3 * volume) / (4 * Mathf.PI), ONE_THIRD);

//        //check to see if we're big enough for irregular objects to stop making us bounce irregularly
//        int irregulars = irregularCollectibles.Count;
//        for (int i = irregulars - 1; i >= 0; --i)
//        {
//            CollectibleObject collectible = irregularCollectibles[i];
//            if (!collectible.IsIrregular(sphere.radius))
//            {
//                Debug.Log("irregular rolled in");
//                irregularCollectibles.RemoveAt(i);

//                if (collectible.rB == null)
//                {
//                    collectible.rB = collectible.gameObject.AddComponent<Rigidbody>();
//                }
//                collectible.rB.mass = 0;
//                collectible.rB.detectCollisions = false;
//                collectible.rB.isKinematic = true;
//            }
//        }
//    }

//    public void NewMax(float size)
//    {
//        if (newOffset < size)
//        {
//            newOffset = size;
//        }
        
//    }
//}