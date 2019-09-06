using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CameraControls : MonoBehaviour {

    public Camera enviroCam;
    private float baseSpeed = 1.0f;
    private float speed;
    
    private float scrollSpeed = 2.5f;

    private Camera mainCam;
    private float zoom = 12;
    private float zoomMin = 5;
    private float zoomMax = 20;
    private Vector3 CameraOnPlayer;
    private float ResetCameraSpeed = 2.5f;
    
    private Plane[] planes;
    private Collider playerCollider;
    Collider upperBounds;
    Collider lowerBounds;
    Collider rightBounds;
    Collider leftBounds;

    // Use this for initialization
    void Start () {
        mainCam = Camera.main;
        transform.position = GetCameraPosFromPlayer();
    }

    void Update()
    {
        //CheckRelativePoistion();
        MoveCam();
        Zoom();
    }

    public Vector3 GetCameraPosFromPlayer()
    {
        return new Vector3(GameManager.PlayerPos.x - transform.position.y*Mathf.Sqrt(2)/2f, 
                           transform.position.y, 
                           GameManager.PlayerPos.z - transform.position.y*Mathf.Sqrt(2)/2f);
    }
    void MoveCam()
    {
        // if (Input.GetKey(Controls.GetControl("camera_recenter")))
        
        if (Input.GetKey(Controls.GetControl("camera_up")) || Input.GetKey(Controls.GetControl("camera_down")) || 
            Input.GetKey(Controls.GetControl("camera_right")) || Input.GetKey(Controls.GetControl("camera_left")))
        {
            if (!Input.GetKey(Controls.GetControl("character_up"))&& !Input.GetKey(Controls.GetControl("character_down")) &&
             !Input.GetKey(Controls.GetControl("character_left")) && !Input.GetKey(Controls.GetControl("character_right")))
            {
                Peek();
            }
            else
            {
                RecenterCamera();
            }
        }
        else
        {
            RecenterCamera();
        }
        
    }
    void Peek()
    { 
        float vertical = 0;
        float horizontal = 0;
        planes = GeometryUtility.CalculateFrustumPlanes(mainCam);
        upperBounds = GameObject.Find("UpperBounds").GetComponent<Collider>();
        lowerBounds = GameObject.Find("LowerBounds").GetComponent<Collider>();
        leftBounds = GameObject.Find("LeftBounds").GetComponent<Collider>();
        rightBounds = GameObject.Find("RightBounds").GetComponent<Collider>();
        if (Input.GetKey(Controls.GetControl("camera_up"))&& GeometryUtility.TestPlanesAABB(planes, upperBounds.bounds))
        {
            vertical++;
            horizontal++;
        }
        if (Input.GetKey(Controls.GetControl("camera_down"))&& GeometryUtility.TestPlanesAABB(planes, lowerBounds.bounds))
        {
            vertical--;
            horizontal--;
        }

        if (Input.GetKey(Controls.GetControl("camera_right"))&& GeometryUtility.TestPlanesAABB(planes, leftBounds.bounds))
        {
            vertical--;
            horizontal++;
        }
        if (Input.GetKey(Controls.GetControl("camera_left"))&& GeometryUtility.TestPlanesAABB(planes, rightBounds.bounds))
        {
            vertical++;
            horizontal--;
        }
        
        Vector3 translation = new Vector3(horizontal * baseSpeed * zoom, 0, vertical * baseSpeed * zoom);            
        translation *= Time.unscaledDeltaTime;
        transform.Translate(translation);

        
        //RecenterPeek();
    }

    void RecenterPeek()
    { 
        float vertical = 0;
        float horizontal = 0;
        planes = GeometryUtility.CalculateFrustumPlanes(mainCam);
        upperBounds = GameObject.Find("UpperBounds").GetComponent<Collider>();
        lowerBounds = GameObject.Find("LowerBounds").GetComponent<Collider>();
        leftBounds = GameObject.Find("LeftBounds").GetComponent<Collider>();
        rightBounds = GameObject.Find("RightBounds").GetComponent<Collider>();
        if (!Input.GetKey(Controls.GetControl("camera_up"))&& GeometryUtility.TestPlanesAABB(planes, upperBounds.bounds))
        {
            vertical--;
            horizontal--;
        }
        if (!Input.GetKey(Controls.GetControl("camera_down")) && GeometryUtility.TestPlanesAABB(planes, lowerBounds.bounds))
        {
            vertical++;
            horizontal++;
        }

        if (!Input.GetKey(Controls.GetControl("camera_right")) && GeometryUtility.TestPlanesAABB(planes, leftBounds.bounds))
        {
            vertical++;
            horizontal--;
        }
        if (!Input.GetKey(Controls.GetControl("camera_left"))&& GeometryUtility.TestPlanesAABB(planes, rightBounds.bounds))
        {
            vertical--;
            horizontal++;
        }
        Vector3 translation = new Vector3(horizontal * baseSpeed * zoom, 0, vertical * baseSpeed * zoom);            
        translation *= Time.unscaledDeltaTime*Time.unscaledDeltaTime;
        transform.Translate(translation);
    }

    void RecenterCamera()
    { 
        CameraOnPlayer = GetCameraPosFromPlayer();
        //Help.print(transform.position, CameraOnPlayer);
        // planes = GeometryUtility.CalculateFrustumPlanes(mainCam);
        // Help.print(planes);
            if (Vector3.Distance(transform.position, CameraOnPlayer) > 1)
            {
                float vertical = 0;
                float horizontal = 0;

                vertical += (CameraOnPlayer.z-transform.position.z) * ResetCameraSpeed;

                horizontal += (CameraOnPlayer.x-transform.position.x) * ResetCameraSpeed;
                
                //this will scale with players godmode speed
                if (GameManager.GodMode && GameManager.GodSpeed)
                {
                    speed = baseSpeed * GameManager.GodSpeedMultiplier;
                }
                else
                {
                    speed = baseSpeed;
                }


                Vector3 translation = new Vector3(horizontal * speed, 0, vertical * speed);
                translation *= Time.unscaledDeltaTime;
                transform.Translate(translation);
            }
    }
    void Zoom()
    { 
        if (GameManager.cameraZoomOn)
        {
            zoom -= Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;//1.5f;
            if (zoom < zoomMin)
            {
                zoom = zoomMin;
            }
            else if (zoom > zoomMax)
            {
                zoom = zoomMax;
            }
            mainCam.orthographicSize = zoom;
            if (enviroCam != null)
            {
                enviroCam.orthographicSize = zoom;
            }
        }
    }
}