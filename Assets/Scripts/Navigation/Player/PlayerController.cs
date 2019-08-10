
///////////////////////////////////////////////////
// 
//   Main class for moving the player object
//
///////////////////////////////////////////////////


using System;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(vp_FPController))]
public class PlayerController : MonoBehaviour
{
    #region ---------- Variables ------------

    // ----------- UFPS Ref ---------------
    vp_FPPlayerEventHandler playerEvents;
    vp_FPController playerController;
    vp_FPCamera playerCamera;
    vp_FPInput playerInput;


    // ----------- Step Settings-----------
    Pedometer pedometer;
    float strideDurationCap = 2;

    float minStrideDuration = .2f;
    float maxStrideDuration = .5f;


    // ----------- Path Variables ---------
    public Path3D walkPath;

    //float lookDistance = 10;
    Vector3 m_CurrentLookPoint = Vector3.zero;
    Vector3 m_LookVelocity = Vector3.zero;
    float LookDamping = .1f;
    float transitionDistance = 20;                           //Distance to the current waypoint that we should move to looking at the next waypoint
    int currentWaypoint = 1;

    #endregion


    void Awake()
    {
        // ------- Get instance refs ----------
        pedometer = Pedometer.singleton;
        playerInput = GetComponent<vp_FPInput>();
        playerEvents = GetComponent<vp_FPPlayerEventHandler>();
        playerController = GetComponent<vp_FPController>();
        playerCamera = GetComponentInChildren<vp_FPCamera>();
    }

    void Start()
    {
        playerInput.AllowGameplayInput = false;
        ClientNetworkManager.singleton.Step += OnStep;

    }

    void FixedUpdate()
    {
        //Test for forwared movement except when at last waypoint
        if (!(currentWaypoint >= walkPath.waypoints.Length - 1)) { ForwardMovement(); }
        else { playerEvents.InputMoveVector.Set(new Vector2(0, 0)); }

        SnapLookAt(walkPath.waypoints[2].position);
        FollowPath();
    }


    void OnStep(object o, EventArgs args)
    {
    }

    /// <summary>
    /// Takes in the info from the pedometer and converts it to forward movement
    /// </summary>
    void ForwardMovement()
    {
        if (pedometer.lastStepDuration > minStrideDuration && pedometer.lastStepDuration < maxStrideDuration)
        {
            playerEvents.InputMoveVector.Set(new Vector2(0, Mathf.Clamp(strideDurationCap - pedometer.lastStepDuration, 0, strideDurationCap)));
        }
        else if (pedometer.lastStepDuration > maxStrideDuration)
        {
            playerEvents.InputMoveVector.Set(new Vector2(0, Mathf.Clamp((strideDurationCap - pedometer.lastStepDuration) / 2, 0, strideDurationCap)));
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerEvents.InputMoveVector.Set(new Vector2(0, 1));
        }
    }

    /// <summary>
    /// Makes the player follow the path by looking at the waypoints
    /// </summary>
    void FollowPath()
    {
        if (Vector3.Distance(transform.position, walkPath.waypoints[currentWaypoint].position) <= transitionDistance)
        {
            if (!(currentWaypoint >= walkPath.waypoints.Length - 1))
            {
                currentWaypoint++;
            }

        }
        Debug.Log(walkPath.waypoints[currentWaypoint]);
        SmoothLookAt(walkPath.waypoints[currentWaypoint].position);
    }

    /// <summary>
    /// if called every frame, smoothly force rotates the Camera
    /// to look at the given lookpoint. set LookDamping to regulate
    /// transition speed
    /// </summary>
    public void SmoothLookAt(Vector3 lookPoint)
    {
        m_CurrentLookPoint = Vector3.SmoothDamp(m_CurrentLookPoint, lookPoint, ref m_LookVelocity, LookDamping);
        playerCamera.transform.LookAt(m_CurrentLookPoint);
        playerCamera.Angle = new Vector2(playerCamera.transform.eulerAngles.x, playerCamera.transform.eulerAngles.y);
    }


    /// <summary>
    /// if called every frame, fixes the Camera angle at the
    /// given lookpoint
    /// </summary>
    public void SnapLookAt(Vector3 lookPoint)
    {

        m_CurrentLookPoint = lookPoint;
        playerCamera.transform.LookAt(m_CurrentLookPoint);
        playerCamera.Angle = new Vector2(playerCamera.transform.eulerAngles.x, playerCamera.transform.eulerAngles.y);

    }

    /// <summary>
    /// snaps the controller position and Camera angle to a certain
    /// coordinate and yaw/pitch, respectively
    /// </summary>
    public void Teleport(Vector3 pos, Vector2 startAngle)
    {
        playerController.SetPosition(pos);
        playerCamera.SetRotation(startAngle);

    }
}
