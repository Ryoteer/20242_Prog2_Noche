using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [Header("<color=#6A89A7>Animation</color>")]
    [SerializeField] private string _onAtkName = "onAttack";
    [SerializeField] private string _onJumpName = "onJump";
    [SerializeField] private string _onSpearName = "onSpearAtk";
    [SerializeField] private string _isMovName = "isMoving";
    [SerializeField] private string _isGroundName = "isGrounded";
    [SerializeField] private string _xName = "xAxis";
    [SerializeField] private string _zName = "zAxis";

    [Header("<color=#6A89A7>Behaviours</color>")]
    [SerializeField] private int _dmg = 25;

    [Header("<color=#6A89A7>Camera</color>")]
    [SerializeField] private Transform _camTarget;

    public Transform GetCamTarget { get { return _camTarget; } }

    [Header("<color=#6A89A7>Inputs</color>")]
    [SerializeField] private KeyCode _atkKey = KeyCode.Mouse0;
    [SerializeField] private KeyCode _secondAtkKey = KeyCode.Mouse1;
    [SerializeField] private KeyCode _jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode _menuKey = KeyCode.Escape;

    [Header("<color=#6A89A7>Movement</color>")]
    [SerializeField] private float _jumpForce = 5f;
    [SerializeField] private float _movSpeed = 3.5f;

    [Header("<color=#6A89A7>Physics</color>")]
    [SerializeField] private Transform _atkOrigin;
    [SerializeField] private float _atkRayDist = 1.25f;
    [SerializeField] private float _spearAtkRayDist = 10.0f;
    [SerializeField] private LayerMask _atkRayMask;
    [SerializeField] private float _groundRayDist = 0.5f;
    [SerializeField] private LayerMask _groundRayMask;

    private float _xAxis = 0f, _zAxis = 0f;
    private Vector3 _camForwardFix = new(), _camRightFix = new(), _dir = new(), _groundOffset = new();
    private Vector3 _dirFix = new();

    private Animator _anim;
    private Rigidbody _rb;
    private Transform _camTransform;

    private Ray _groundRay, _atkRay, _spearRay;
    private RaycastHit _atkHit;

    private void Awake()
    {
        GameManager.Instance.Player = this;

        _rb = GetComponent<Rigidbody>();
        //_rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        //_rb.angularDrag = 1f;
        _rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void Start()
    {
        _camTransform = Camera.main.transform;

        _anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        _dir.x = Input.GetAxis("Horizontal");
        _dir.z = Input.GetAxis("Vertical");

        _anim.SetFloat(_xName, _dir.x);
        _anim.SetFloat(_zName, _dir.z);
        _anim.SetBool(_isMovName, _dir.sqrMagnitude != 0.0f);
        _anim.SetBool(_isGroundName, IsGrounded());

        if (Input.GetKeyDown(_atkKey))
        {
            _anim.SetTrigger(_onAtkName);
        }
        else if (Input.GetKeyDown(_secondAtkKey))
        {
            _anim.SetTrigger(_onSpearName);
        }

        if (Input.GetKeyDown(_jumpKey) && IsGrounded())
        {
            _anim.SetTrigger(_onJumpName);
            Jump();
        }

        if (Input.GetKeyDown(_menuKey))
        {
            LoadingManager.Instance.LoadSceneAsync("MainMenu");
        }
    }

    private void FixedUpdate()
    {
        if(_dir.sqrMagnitude != 0.0f)
        {
            Movement(_dir);
        }
    }

    public void Attack()
    {
        _atkRay = new Ray(_atkOrigin.position, transform.forward);

        if (Physics.Raycast(_atkRay, out _atkHit, _atkRayDist, _atkRayMask))
        {
            if (_atkHit.collider.TryGetComponent<Enemy>(out Enemy enemy))
            {
                enemy.TakeDamage(_dmg);
            }
        }
    }

    private void Movement(Vector3 dir)
    {
        _camForwardFix = _camTransform.forward;
        _camRightFix = _camTransform.right;

        _camForwardFix.y = 0.0f;
        _camRightFix.y = 0.0f;

        Rotate(_camForwardFix);

        _dirFix = (_camRightFix * dir.x + _camForwardFix * dir.z).normalized;

        _rb.MovePosition(transform.position + _dirFix * _movSpeed * Time.fixedDeltaTime);
    }

    private void Jump()
    {
        _rb.AddForce(transform.up * _jumpForce, ForceMode.Impulse);
    }

    private void Rotate(Vector3 dir)
    {
        transform.forward = dir;
    }    

    public void SpearAttack()
    {
        _spearRay = new Ray(_atkOrigin.position, transform.forward);

        RaycastHit[] hitObjs = Physics.RaycastAll(_spearRay, _spearAtkRayDist, _atkRayMask);

        foreach(RaycastHit hitObj in hitObjs)
        {
            if(hitObj.collider.TryGetComponent<Enemy>(out Enemy enemy))
            {
                enemy.TakeDamage(_dmg * 4);
            }
        }
    }

    private bool IsGrounded()
    {
        _groundOffset = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);

        _groundRay = new Ray(_groundOffset, -transform.up);

        return Physics.Raycast(_groundRay, _groundRayDist, _groundRayMask);
    }
}
