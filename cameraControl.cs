using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class cameraControl : MonoBehaviour {
    // The target we are following
    public Transform look;
    public Transform target = null;
    public int viewType = 1;
    // The distance in the x-z plane to the target
    public float length = 10f;
    public float maxLength = 40f;
    public float minLength = 5f;
    float distance = 10.0f;
    // the height we want the camera to be above the target
    float height = 5.0f;
    // How much we 
    public float damping = 7f;
    public float zoomDamping = 7f;

    public float mouseSensitivity = 100.0f;
    public float clampAngle = 80.0f;

    private float rotY = 0.0f; // rotation around the up/y axis
    private float rotX = 0.0f; // rotation around the right/x axis

    float x = 0.0f;
    float y = 0.0f;


    private Vector3 origPos;
    private Vector3 shoulder;

    public GameObject crosshair;

    private LayerMask layerMask;



    void Start()
    {
        height = (length * length) / 10f;
        distance = length;

        Vector3 rot = transform.localRotation.eulerAngles;
        rotY = rot.y;
        rotX = rot.x;

        crosshair = GameObject.Find("crosshair");
        layerMask = ~(1 << LayerMask.NameToLayer("player"));
        Debug.Log(LayerMask.NameToLayer("player"));

    }

    void FixedUpdate()
    {
        if (target == null)
        {
            foreach (GameObject plyr in GameObject.FindGameObjectsWithTag("player"))
            {
                if (plyr.GetComponent<InputController>().isLocalPlayer)
                {
                    target = plyr.transform;
                }
            }
        }
        else
        {

            switch (viewType)
            {
                case 1:
                    NormalView();
                    break;
                case 2:
                    ZoomView();
                    break;
                case 3:
                    FreeLook();
                    break;
                case 4:
                    LookAt(look);
                    break;
            }

            


            //experemental
            /*
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray targetRay = new Ray(transform.position, transform.forward);
                if(Physics.Raycast(targetRay, out hit)){
                    if (hit.transform.gameObject.GetComponent<movement>())
                    {
                        hit.transform.gameObject.GetComponent<movement>().enabled = true;
                        target.gameObject.GetComponent<movement>().enabled = false;

                        target = hit.transform;

                        Vector3 rot = target.transform.localRotation.eulerAngles;
                        rotY = rot.y;
                        rotX = rot.x;
                    }
                }
            }
            */
        }

    }



    void FreeLook()
    {
        Cursor.lockState = CursorLockMode.Locked;
        crosshair.GetComponent<Text>().enabled = false;
        

        if (Input.GetAxis("Mouse ScrollWheel") != 0f)
        {
            length = Mathf.Clamp(length - Input.GetAxis("Mouse ScrollWheel"), minLength, maxLength);
            height = ((length * length) / 10f);
            distance = length;
        }

        Vector3 negDistance = new Vector3(0, height, -distance);
        Quaternion rotation = Quaternion.Euler(rotX, rotY, 0);
        Vector3 position = rotation * negDistance + target.position;

        
        
        transform.position = position;
        transform.rotation = rotation;

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = -Input.GetAxis("Mouse Y");

        rotY += mouseX * mouseSensitivity * Time.deltaTime;
        rotX += (mouseY * mouseSensitivity * Time.deltaTime);

        rotX = Mathf.Clamp(rotX, -25f, clampAngle);
    }



    void LookAt(Transform look)
    {
        Cursor.lockState = CursorLockMode.None;
        crosshair.GetComponent<Text>().enabled = false;
        GetComponent<Camera>().fieldOfView = 40;

        Vector3 pos = look.position - Camera.main.transform.position;
        Quaternion newRot = Quaternion.LookRotation(pos, Vector3.up);
        Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, newRot, Time.deltaTime * 10);

        Vector3 loc = look.position + (look.forward * 5)+(-look.right)+(look.up*1.5f);
        transform.position = Vector3.Lerp(transform.position, loc, Time.deltaTime * 10);
    }

    void ZoomView()
    {
        Cursor.lockState = CursorLockMode.Locked;
        crosshair.GetComponent<Text>().enabled = true;
        GetComponent<Camera>().fieldOfView = 50;
        Vector3 shoulder = (target.position + target.transform.up*0.7f+target.transform.right*0.5f-target.transform.forward * 1f);
        transform.position = Vector3.Lerp(transform.position, shoulder, Time.deltaTime * zoomDamping);
        transform.rotation = Quaternion.Euler(rotX, rotY, 0.0f);



        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = -Input.GetAxis("Mouse Y");

        rotY += mouseX * mouseSensitivity * Time.deltaTime;
        rotX += mouseY * mouseSensitivity * Time.deltaTime;

        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);
        

    }

    void NormalView()
    {
        origPos = target.position + (-target.forward * distance) + (target.up * height);

        RaycastHit hit;
        if (Physics.Raycast(target.transform.position, origPos - target.transform.position, out hit, Vector3.Distance(target.transform.position, origPos), layerMask))
        {

            //transform.position = hit.point;
            //transform.position = Vector3.MoveTowards(transform.position, target.transform.position, 1f);
            origPos = target.position + (-target.forward * Vector3.Distance(target.transform.position, hit.point) + (hit.normal * 0.2f)) + (target.up * height);

        }

        Cursor.lockState = CursorLockMode.None;
        crosshair.GetComponent<Text>().enabled = false;
        GetComponent<Camera>().fieldOfView = 60;

        transform.position = Vector3.Lerp(transform.position, origPos, Time.deltaTime * damping);

       




        //adjust distance
        if (Input.GetAxis("Mouse ScrollWheel") != 0f)
        {
            length = Mathf.Clamp(length-Input.GetAxis("Mouse ScrollWheel"), minLength, maxLength);
            height = ((length * length) / 10f)+(1/length);
            distance = length;
        }


        // Always look at the target
        transform.LookAt(target);
        transform.rotation *= Quaternion.AngleAxis(-15, Vector3.right);

        //test
        //origPos = target.position + (-target.forward * distance) + (target.up * height);


        rotX = transform.root.eulerAngles.x-5f;
        rotY = transform.root.eulerAngles.y;

    }


    
}
