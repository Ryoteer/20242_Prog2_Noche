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

    [Header("<color=#6A89A7>Inputs</color>")]
    [SerializeField] private KeyCode _atkKey = KeyCode.Mouse0;
    [SerializeField] private KeyCode _secondAtkKey = KeyCode.Mouse1;
    [SerializeField] private KeyCode _jumpKey = KeyCode.Space;

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
    private Vector3 _dir = new(), _groundOffset = new();

    private Animator _anim;
    private Rigidbody _rb;

    private Ray _groundRay, _atkRay, _spearRay;
    private RaycastHit _atkHit;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        //_rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        //_rb.angularDrag = 1f;
        _rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void Start()
    {
        GameManager.Instance.Player = this;

        _anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        _xAxis = Input.GetAxis("Horizontal");
        _zAxis = Input.GetAxis("Vertical");

        _anim.SetFloat(_xName, _xAxis);
        _anim.SetFloat(_zName, _zAxis);
        _anim.SetBool(_isMovName, _xAxis != 0 || _zAxis != 0);
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
    }

    private void FixedUpdate()
    {
        if(_xAxis != 0 || _zAxis != 0)
        {
            Movement(_xAxis, _zAxis);
        }
    }

    private void Movement(float x, float z)
    {
        _dir = (transform.right * x + transform.forward * z).normalized;

        _rb.MovePosition(transform.position + _dir * _movSpeed * Time.fixedDeltaTime);
    }

    private void Jump()
    {
        _rb.AddForce(transform.up * _jumpForce, ForceMode.Impulse);
    }

    public void Attack()
    {
        _atkRay = new Ray(_atkOrigin.position, transform.forward);

        if(Physics.Raycast(_atkRay, out _atkHit, _atkRayDist, _atkRayMask))
        {
            if(_atkHit.collider.TryGetComponent<Enemy>(out Enemy enemy))
            {
                enemy.TakeDamage(_dmg);
            }
        }
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
