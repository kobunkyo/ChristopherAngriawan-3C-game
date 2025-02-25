using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public Action<Vector2> OnMoveInput;
    public Action<bool> OnSprintInput;
    public Action OnJumpInput;
    public Action OnClimbInput;
    public Action OnCancelClimb;

    void Update()
    {
        CheckMovementInput();
        CheckCancelInput();
        CheckChangePOVInput();
        CheckClimbInput();
        CheckCrouchInput();
        CheckGlideInput();
        CheckJumpInput();
        CheckMainMenuInput();
        CheckPunchInput();
        CheckSprintInput();
    }

    private void CheckMovementInput()
    {
        float verticalAxis = Input.GetAxisRaw("Vertical");
        float horizontalAxis = Input.GetAxisRaw("Horizontal");
        Vector2 inputAxis = new Vector2(horizontalAxis, verticalAxis);
        if (OnMoveInput != null)
        {
            OnMoveInput(inputAxis);
        }
    }

    private void CheckSprintInput()
    {
        bool isHoldSprintInput = Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.LeftShift);
        if (isHoldSprintInput)
        {
            if (OnSprintInput != null)
            {
                OnSprintInput(true);
            }
            
        }
        else
        {
            if (OnSprintInput != null)
            {
                OnSprintInput(false);
            }
        }
        
    }

    private void CheckJumpInput()
    {
        bool isPressJumpInput = Input.GetKeyDown(KeyCode.Space);
        if (isPressJumpInput)
        {
            if (OnJumpInput != null)
            {
                OnJumpInput();
            }
        }
    }

    private void CheckCrouchInput()
    {
        bool isPressCrouchInput = Input.GetKeyDown(KeyCode.RightControl) || Input.GetKey(KeyCode.LeftControl);
        if (isPressCrouchInput)
        {
            Debug.Log("Crouch");
        }
    }

    private void CheckChangePOVInput()
    {
        bool isPressChangePOVInput = Input.GetKeyDown(KeyCode.Q);
        if (isPressChangePOVInput)
        {
            Debug.Log("Change POV");
        }
    }

    private void CheckClimbInput()
    {
        bool isPressClimbInput = Input.GetKeyDown(KeyCode.E);
        if (isPressClimbInput)
        {
            if (OnClimbInput != null)
            {
                OnClimbInput();
            }
        }
    }

    private void CheckGlideInput()
    {
        bool isPressGlideInput = Input.GetKeyDown(KeyCode.G);
        if (isPressGlideInput)
        {
            Debug.Log("Glide");
        }
    }

    private void CheckCancelInput()
    {
        bool isPressCancelInput = Input.GetKeyDown(KeyCode.C);
        if (isPressCancelInput)
        {
            if(OnCancelClimb != null)
            {
                OnCancelClimb();
            }
        }
    }

    private void CheckPunchInput()
    {
        bool isPressPunchInput = Input.GetKeyDown(KeyCode.Mouse0);
        if (isPressPunchInput)
        {
            Debug.Log("Punch");
        }
    }

    private void CheckMainMenuInput()
    {
        bool isPressMainMenuInput = Input.GetKeyDown(KeyCode.Escape);
        if (isPressMainMenuInput)
        {
            Debug.Log("MainMenu");
        }
    }
}
