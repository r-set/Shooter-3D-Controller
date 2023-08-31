using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]

public class PlayerController : MonoBehaviour
{
    private Vector2 _moveInput;

    public Vector2 MoveInput
    {   
        set
        {
            _moveInput.x = value.x;
            _moveInput.y = value.y;
        }
    }

    [HideInInspector] public bool IsJump;
    [HideInInspector] public bool IsSprint;
    //[HideInInspector] public bool IsCrouch;

    private CharacterController _characterController;
    private Transform _cameraMainTransform;

    private bool _isGround;

    #region Player Movement
    public float _playerSpeed = 5f;
    public float _playerSprint;
    //public float _crouchSpeed = 2f;
    public float _jumpHeight = 2f;
    private float _rotationSpeed = 2.5f;
    #endregion

    private float _gravityValue = -9.81f;
    private Vector3 _playerVelocity;
    private Vector3 _move;

    public Animator _animator;
    private int _moveXAnimatorId;
    private int _moveZAnimatorId;
    private int _jumpAnimator;
    private Vector2 _currentBlendAnim;
    private Vector2 _animVelocity;
    [SerializeField] private float _animSmoothTime = 0.1f;
    [SerializeField] private float _animationPlayTransition = 0.15f;

    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _gunTransform;
    [SerializeField] private float _bulletHitMaxMiss = 25f;
    [SerializeField] private Transform _bulletParent;

    void Awake()
    {
        _cameraMainTransform = Camera.main.transform;
        _animator = GetComponent<Animator>();

        _moveXAnimatorId = Animator.StringToHash("MoveX");
        _moveZAnimatorId = Animator.StringToHash("MoveY");
        _jumpAnimator = Animator.StringToHash("Jump");
    }

    void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        GroundCheck();
        MoveCharacter();
        JumpCharacter();
        RotateDirection();
    }

    private void GroundCheck()
    {
        _isGround = _characterController.isGrounded;
        if (_isGround && _playerVelocity.y < 0) _playerVelocity.y = 0f;
    }

    private void MoveCharacter()
    {
        _currentBlendAnim = Vector2.SmoothDamp(_currentBlendAnim, _moveInput, ref _animVelocity, _animSmoothTime);
        _move = new Vector3(_currentBlendAnim.x, 0, _currentBlendAnim.y);

        _move = _cameraMainTransform.forward * _moveInput.y + _cameraMainTransform.right * _moveInput.x;
        _move.y = 0;
        _characterController.Move(_move * Time.deltaTime * _playerSpeed);

        _animator.SetFloat(_moveXAnimatorId, _currentBlendAnim.x);
        _animator.SetFloat(_moveZAnimatorId, _currentBlendAnim.y);
    }

    private void JumpCharacter()
    {
        if (_isGround && IsJump)
        {
            _playerVelocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravityValue);
            _animator.CrossFade(_jumpAnimator, _animationPlayTransition);
            IsJump = false;
        }

        _animator.SetBool(_jumpAnimator, true);
        _playerVelocity.y += _gravityValue * Time.deltaTime;
        _characterController.Move(_playerVelocity * Time.deltaTime);
    }

    private void RotateDirection()
    {
        if (_moveInput != Vector2.zero)
        {
            //float targetAngle = Mathf.Atan2(_moveInput.x, _moveInput.z) * Mathf.Rad2Deg + _cameraMainTransform.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0f, _cameraMainTransform.eulerAngles.y, 0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * _rotationSpeed);
        }
    }

    public void ShootGun()
    {
        //GameObject bullet = Instantiate(_bulletPrefab, _gunTransform.position, Quaternion.identity, _bulletParent);

        GameObject bullet = ObjectPool.SharedInstance.GetPoolObject();
        if (bullet !=  null)
        {
            bullet.transform.parent = _bulletParent;
            bullet.transform.position = _gunTransform.position;
            bullet.transform.rotation = _gunTransform.rotation;
            bullet.SetActive(true);
        }

        BulletController bulletController = bullet.GetComponent<BulletController>();

        RaycastHit hit;
        if (Physics.Raycast(_cameraMainTransform.position, _cameraMainTransform.forward, out hit, Mathf.Infinity))
        {
            bulletController.Target = hit.point;
            bulletController.Hit = true;
        }
        else
        {
            bulletController.Target = _cameraMainTransform.position + _cameraMainTransform.forward * _bulletHitMaxMiss;
            bulletController.Hit = false;
        }

      }
}
