
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Adjusts a an object that is always between the Player and the mouse, then the camera follows it by cinemachine
/// </summary>
public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera myCamera = null;  //main camera
    [SerializeField] private GameObject followPointer = null;   //the helping object that is always between the mouse and the player nad the cinemachine camera follows it
    [SerializeField] private GameObject unitGameObject = null; //the Main object it is following, in this case the player

    private Vector3 mousePosition = new Vector3();
    private Vector3 followPointerPosition = new Vector3();

    // Update is called once per frame
    void Update()
    {
        mousePosition = myCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        followPointerPosition = (mousePosition  + unitGameObject.transform.position) / 2;
        followPointerPosition = (followPointerPosition +unitGameObject.transform.position) / 2;
        followPointer.transform.position = followPointerPosition;
    }
}
