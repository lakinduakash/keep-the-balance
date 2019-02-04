using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlateController : MonoBehaviour
{
    public static List<GameObject> plateList;

    public static int collisionCount =0;
    public static int plateCount = 0;

    public static Vector3 massCenter;
    public static float totalMass;

    public static GameObject activeTop;

    public float increaseFactor = 4f;
    public float heightOffset = 0.5f;
    [SerializeField]public float force = 10f;
    public Vector3 nextInitialPosition;
    public float downForce=300f;
    public bool startUpdate;

    bool isClicked = false;
    bool onceCollide = false;

    
    

    Rigidbody rb;
    bool atRight = true;

    void Start()
    {
        plateCount++;
        if (plateCount == 1)
            startUpdate = true;

        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.constraints = RigidbodyConstraints.FreezePositionY;

        rb.isKinematic = true;
        rb.isKinematic = false;

        if (massCenter==null)
        {
            Debug.LogError("MassCenter not initialised");
        }

        if(activeTop==null)
        {
            Debug.LogError("Active top not initialised as GroundPlate");
        }


    }

    void Update()
    {
        if (transform.position.x >= 0 )
        {
            atRight = true;
        }

        else
        {
            atRight = false;
        }

        if (Input.GetMouseButtonDown(0))
        {
            isClicked = true;
        }
    }

    void FixedUpdate()
    {
        if (startUpdate)
        {
            if (atRight && !isClicked)
            {
                rb.AddForce(new Vector3(-force, 0f, 0f));
            }
            else if (!atRight && !isClicked)
            {
                rb.AddForce(new Vector3(force, 0f, 0f));
            }

            if (isClicked)
            {
                rb.AddForce(Vector3.zero);
                rb.velocity = Vector3.zero;
                rb.constraints = RigidbodyConstraints.None;
                rb.constraints = RigidbodyConstraints.FreezeRotation;
                rb.AddForce(new Vector3(0f, -downForce, 0f));
            }
        }
        
    }

   void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.collider.tag);

        if (collision.collider.tag.Equals("Player") && isClicked && !onceCollide && GameObject.ReferenceEquals(activeTop, collision.collider.gameObject))
        {
            rb.isKinematic = true;
            collisionCount += 1;
            onceCollide = true;
            isClicked = false;

            activeTop = gameObject;
            SetCenterOfMass();
            CreateNewPlate();
            SetCamTarget();


        }
        else if(isClicked && !onceCollide && !GameObject.ReferenceEquals(activeTop, collision.collider.gameObject))
        {
            rb.isKinematic = true;
            collisionCount += 1;
            onceCollide = true;
            isClicked = false;

            Debug.Log("Not active top");
        }
      
    }


    void SetCamTarget()
    {
        CamTarget.camTarget.transform.position = gameObject.transform.position;
    }

    void SetCenterOfMass()
    {
        Vector3 v=CenterOfMass.getCenterOfMass(totalMass, massCenter, rb.mass, transform.position);
        totalMass += rb.mass;
        massCenter = v;

        Debug.Log("Mass: "+totalMass+" center: "+massCenter);

        Debug.DrawRay(v, Vector3.down, Color.white, 5f);
    }



    GameObject CreateNewPlate()
    {
        GameObject prefab = Resources.Load("PlatePlayer", typeof(GameObject)) as GameObject;
        GameObject newPlate = Instantiate(prefab);

        Rigidbody newRb = newPlate.GetComponent<Rigidbody>();
        PlateController newCont = newPlate.GetComponent<PlateController>();
        newRb.mass = rb.mass * increaseFactor;
        newCont.force = force * increaseFactor;
        newCont.downForce = downForce * increaseFactor;
        newRb.isKinematic = true;
        newPlate.transform.SetParent(transform.parent);
        int[] leftOrRight = { -1, 1, -1, 1, -1, 1, -1, 1, -1, 1, -1, 1 };
        int randLeftOrRight = Random.Range(0, 11);
        Debug.Log(randLeftOrRight);
        float randPos = Random.Range(1f,3f);
        newPlate.transform.localPosition = new Vector3(randPos*leftOrRight[randLeftOrRight], newPlate.transform.localPosition.y, 0f);

        newPlate.transform.localPosition += new Vector3(0f, transform.localPosition.y, 0f);
        newRb.isKinematic = false;
        newCont.startUpdate = true;
        return newPlate;
    }

}
