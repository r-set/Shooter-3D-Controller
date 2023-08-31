using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputSystem : MonoBehaviour
{
    private PlayerController _playerController;

    #region InputSystem
    private PlayerInput _inputController;
    private InputAction _actionMove;
    private InputAction _actionJump;
    private InputAction _actionSprint;
    private InputAction _actionShoot;
    #endregion

    private void Awake()
    {
        _inputController = GetComponent<PlayerInput>();
        _playerController = GetComponent<PlayerController>();

        _actionMove = _inputController.actions["Move"];
        _actionSprint = _inputController.actions["Run"];
        _actionJump = _inputController.actions["Jump"];
        _actionShoot = _inputController.actions["Shoot"];

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnEnable()
    {
        _actionShoot.performed += _ => Shooting();
    }

    private void OnDisable()
    {
        _actionShoot.performed -= _ => Shooting();
    }

    private void Update()
    {
        ActionMove();
        ActionJump();
        ActionSprint();
    }

    private void ActionMove()
    {
        Vector2 inputMoveAction = _actionMove.ReadValue<Vector2>();
        _playerController.MoveInput = inputMoveAction;
    }

    private void ActionJump()
    {
        if ( _actionJump.triggered)
        {
            _playerController.IsJump = true;
        }
        else
        {
            _playerController.IsJump = false;
        }
    }

    private void ActionSprint()
    {
        if ( _actionSprint.inProgress)
        {
            _playerController.IsSprint = true;
        }
        else
        {
            _playerController.IsSprint = false;
        }
    }

    private void Shooting()
    {
        _playerController.ShootGun();
    }
}