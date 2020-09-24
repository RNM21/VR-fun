using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class ContinuousMovement : MonoBehaviour
{
    public float speed = 2;
    public float jumpSpeed = 0.25f;
    public float gravity = -9.81f;
    public LayerMask groundLayer;
    public float extraHeight = 0.1f;
    public bool toggleSprint = false;

    private float fallSpeed;
    
    private bool jumping = false;
    private float movementSpeed;//so I can switch between sprining and not sprinting easier
    public XRNode inputSourceLeft;
    public XRNode inputSourceRight;
    private Vector2 inputAxis;
    private CharacterController character;
    private XRRig rig;

    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<CharacterController>();
        rig = GetComponent<XRRig>();
    }

    // Update is called once per frame
    void Update()
    {
        InputDevice deviceLeft = InputDevices.GetDeviceAtXRNode(inputSourceLeft);
        InputDevice deviceRight = InputDevices.GetDeviceAtXRNode(inputSourceRight);
       
        deviceLeft.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxis);

        //sprint mechanic
        #region
        deviceLeft.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out bool primary2DAxisClickValue);
        if(toggleSprint)//toggle sprint on or off using thinb stick click
        {
            //this method mostly works but does not take in account for holding the thumb stick but works well if the user just clicks it
            if (primary2DAxisClickValue)//sprint is pressed
            {
                if(movementSpeed > speed)
                {
                    movementSpeed = speed;
                }
                else
                {
                    movementSpeed = 2 * speed;
                }
            }
        }
        else//hold to sprint
        {
            if (primary2DAxisClickValue)//sprint is pressed
            {
                movementSpeed = speed * 2;
            }
            else
            {

                movementSpeed = speed;
            }
        }
        #endregion//sprint mechanic

        //jump mechanic
        #region

        //deviceRight.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValue);
        //if (primaryButtonValue)
        //{
        //    if(checkIfGrounded())
        //    {
        //        jumping = true;
        //    }
        //}

        
        if (checkIfGrounded())
        {
            deviceRight.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValue);
            if (primaryButtonValue)
            {
                jumping = true;
            }
        }
        else
        {
            jumping = false;
        }

        if (jumping)
        {
            character.Move(Vector3.up * jumpSpeed);
        }

        #endregion

    }

    private void FixedUpdate()
    {
        FollowHeadset();

        Quaternion headYaw = Quaternion.Euler(0, rig.cameraGameObject.transform.eulerAngles.y, 0);
        Vector3 direction = headYaw * new Vector3(inputAxis.x, 0, inputAxis.y);
        character.Move(direction * Time.fixedDeltaTime * movementSpeed);

        //gravity
        bool grounded = checkIfGrounded();
        
        if(!grounded)
        {
            fallSpeed += gravity * Time.fixedDeltaTime;
        }
        else
        {
            fallSpeed = 0;
        }

        character.Move(Vector3.up * fallSpeed * Time.fixedDeltaTime);
    }

    //tells whether gravity is on or not
    //returns true if grounded
    bool checkIfGrounded()
    {
        Vector3 rayStart = transform.TransformPoint(character.center);
        float rayLength = character.center.y + 0.01f;
        bool hit = Physics.SphereCast(rayStart, character.radius, Vector3.down, out RaycastHit hitInfo, rayLength, groundLayer);
        return hit;
    }

    //makews sure the collider we have for the player moves with the headset and not the play area
    void FollowHeadset()
    {
        character.height = rig.cameraInRigSpaceHeight + extraHeight;//extra height if needed
        Vector3 capsuleCenter = transform.InverseTransformPoint(rig.cameraGameObject.transform.position);
        character.center = new Vector3(capsuleCenter.x, character.height/2 + character.skinWidth, capsuleCenter.z);
    }
}
