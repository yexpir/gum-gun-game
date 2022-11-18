using System;
using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using Cinemachine;
using DefaultNamespace;
//using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace SalvadorNovo.CharacterRelated
{
    public class PlayerMove : MonoBehaviour, IKillable
    {
        public CharacterController _characterController;
        public Animator _animator;


        //Movement Aim
        private Camera _cam;
        private Transform _aim;


        public bool hasKey;
        public bool hasBear;
        public GameObject teddyBear;
        public GameObject goldKey;
    
        //GunAim Dependencies
        public bool hasGun;
        [SerializeField] private GameObject _gumGun;
        
        [SerializeField] private RigBuilder _rigBuilder;
        [SerializeField] private MultiAimConstraint _handAim;
        [SerializeField] private MultiAimConstraint _armAim;
        [SerializeField] private MultiAimConstraint _foreArmAim;
        [SerializeField] private MultiAimConstraint _bodyAim;
        [SerializeField] private Transform _aimTarget;
        //private GameObject _aimTarget;
        [SerializeField] private Transform _rifleTip;
    

        //Movement
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _walkSpeed;
        [SerializeField] private float _jumpSpeed;
        [SerializeField] private float _flipSpeed;
    
        private Vector3 _move;
        private Vector3 _moveRaw;
        private Vector3 _movement;
        private Vector3 _airVelocity;

        //Cardinal Orientation Rotation
        [SerializeField] private float _rotationSpeed;
        private Vector3 _facingDirection = Vector3.forward;
        private float _prevLargeDir;
    
        private Quaternion lookRotation;
        private Vector3 _currentDirection;
        private float _rotationTimer;
    
        //Jump
        private bool _jumpInput;
        [SerializeField] private float _jumpForce;
        [SerializeField] private float _jumpGravity;
        [SerializeField] private float _airGravity;
        [SerializeField] private float _fallGravity;
        private TrailRenderer _trail;
        [SerializeField] private LayerMask _groundCheckLayer;
        private Collider _feetTrigger;
        
        //Airflip
        /*[SerializeField] private Vector3 _rotation;
        [SerializeField] private float _rotationSpeed;*/
        [SerializeField] private float _airFLipStart;
        [SerializeField] private float _airFLipEnd;
        [SerializeField] private LayerMask _aimRayIgnoreLayer;

        //Gun Aim
        private bool _shootInput;
        [SerializeField] private float _aimSpeed;
        private bool _hasReachedMax;
        private bool _hasReachedMin;
        private RaycastHit hit;
        private Ray ray;
        
        //Shooting
        [SerializeField] public Transform _bulletSpawner;
        [SerializeField] public GameObject _bullet;
        [SerializeField] public GameObject _inflatableBullet;
        [SerializeField] private float _bulletSpeed;
        private bool _aimEnter;
        private bool _aimExit;

        //Inflating BubbleGum
        private GameObject _tempBaloon;
        private bool _inflateInput;
        private bool _shotGun;
        private bool _hasBaloon;
        public bool isInflating;

    
        //Crosshair
        [SerializeField] private Texture2D _crossHair;
        [SerializeField] private CinemachineFreeLook _cinemachine;

    
    
        //Animator parameters hashes
        private int _isWalkingHash;
        private int _isJumpingHash;
        private int _isFallingHash;
        private int _isGroundedHash;
        private int _isLandingHash;
        private int _isAimingHash;
        private int _speedHash;
        private int _verVelHash;
        private int _landTimeHash;
    

        private void Awake()
        {
            Player = this;
        
            PlayerTransform = transform;
        
            _cam = Camera.main;

            _trail = GetComponent<TrailRenderer>();
        
            _aim = new GameObject().transform;

            GetAllAnimHashes();
        }

        private void Start()
        {
            Cursor.SetCursor(_crossHair, new Vector2(_crossHair.width/2.0f, _crossHair.height/2.0f), CursorMode.ForceSoftware);
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            MovementManager();

            if(hasGun)
                HandleShooting();
        }


        private void MovementManager()
        {
            GetInput();
            SetMovement();
            SetJump();
            Move();
            LookDirection();
            Aim();
            SetAnimatorParameters();
        }

        private void GetInput()
        {
            _move.z = Input.GetAxis("Vertical");
            _move.x = Input.GetAxis("Horizontal");
        
            _moveRaw.z = Input.GetAxisRaw("Vertical");
            _moveRaw.x = Input.GetAxisRaw("Horizontal");
        
            _jumpInput = Input.GetButtonDown("Jump");
            _shootInput = Input.GetButton("Fire1");
            _inflateInput = Input.GetButton("Aim");
            
        
            GetInputSpeed();
        }


        private void SetMovement()
        {
            _aim.position = ResetYAxis(_cam.transform.position);
            _aim.LookAt(ResetYAxis(PlayerPos));
            _movement = _aim.forward * _moveRaw.z + _aim.right * _moveRaw.x;
        }
        private void SetJump()
        {
            IsLanding = GetLandingState();
            if (IsFalling || IsLanding) IsJumping = false;
            if (IsGrounded && Input.GetButton("Jump"))
            {
                IsJumping = true;
                /*StartCoroutine(*/JumpRoutine()/*)*/;
            }

            if (IsAirFlipping) _moveSpeed = _flipSpeed;
            else if(!IsAirFlipping && IsFalling) _moveSpeed = _jumpSpeed;
            else if (IsLanding) _moveSpeed = _walkSpeed;
        
            _airVelocity.y -= Gravity * Time.deltaTime;
            if (IsGrounded && _airVelocity.y <= -1.0f) _airVelocity.y = -1.0f;
        
            _trail.emitting = IsJumping || IsAirFlipping;

            //AirFlip();
        }
        private void JumpRoutine()
        {
            /*_moveSpeed = 0;
        yield return new WaitForSeconds(0.025f);*/
            _airVelocity.y = _jumpForce;
            _moveSpeed = _jumpSpeed;
        }
        private void AirFlip()
        {
            /*if (IsAirFlipping && !IsAiming) transform.Rotate(_rotation * (Time.deltaTime * _rotationSpeed));
        else if (IsFalling || IsLanding) transform.rotation = DefaultRotation;*/
        } private Quaternion DefaultRotation
        {
            get
            {
                _defaultRotation = Quaternion.Euler(0.0f, PlayerTransform.eulerAngles.y, 0.0f);
                return _defaultRotation;
            }
        } private Quaternion _defaultRotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
        private void Move() => _characterController.Move((_movement.normalized * (_moveSpeed * InputSpeed) + _airVelocity) * Time.deltaTime);
    
        private void LookDirection()
        {
            if (IsWalking) _facingDirection = _movement;
            lookRotation = Quaternion.LookRotation(_facingDirection);
            if (_moveRaw != _currentDirection)
            {
                _rotationTimer = 0.0f;
                _currentDirection = _moveRaw;
            }

            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, _rotationTimer);
            _rotationTimer += _rotationSpeed * Time.deltaTime;
        }

        private void InflateBubblegum()
        {
            if (Input.GetButtonDown("Aim"))
            {
                StopCoroutine(Gun());
                _tempBaloon = Instantiate(_inflatableBullet, _bulletSpawner.position, _bulletSpawner.rotation);
                _hasBaloon = true;
                StartCoroutine(InflateRoutine());
            }

            if (_tempBaloon == null) return;
            if (Input.GetButtonDown("Fire1"))
            {
                _shotGun = true;
                Shoot();
                isInflating = false;
            }
            else
            {
                _tempBaloon.transform.position = _bulletSpawner.position;
                _tempBaloon.transform.forward = _bulletSpawner.forward;
            }
        }

        public IEnumerator InflateRoutine()
        {
            while (_tempBaloon && _tempBaloon.transform.localScale.x < 2.5f)
            {
                if (Input.GetButtonUp("Aim")) break;
                isInflating = true;
                _tempBaloon.transform.localScale *= 1.0f + Time.deltaTime;
                yield return null;
            }
        }

        private void Aim()
        {
            if(!hasGun) return;
            SmoothAim();

            ray = _cam.ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(ray, out hit, 1000.0f, _aimRayIgnoreLayer)) return;
            _aimTarget.position = hit.point;
            if (Input.mousePosition.y < Screen.height * 0.4f && !IsGrounded) _aimTarget.position = new Vector3(PlayerPos.x, _aimTarget.position.y, PlayerPos.z);
            if (_hasReachedMax)
            {
                Debug.DrawLine(_rifleTip.position, hit.point, Color.red);
            }
        }

        private void SmoothAim()
        {
            var speed = _aimSpeed * Time.deltaTime;
            if (IsAiming || _hasBaloon)
            {
                _aimTarget.gameObject.SetActive(true);
                
                _cinemachine.m_YAxis.m_MaxSpeed = 0.0f;
                //_cinemachine.m_XAxis.m_MaxSpeed = 0.0f;
                Cursor.lockState = CursorLockMode.Confined;
            
                _hasReachedMin = false;
                if (_hasReachedMax) return;
                if (_bodyAim.weight >= 0.75f)
                {
                    _bodyAim.weight = 0.75f;
                    _armAim.weight = 0.75f;
                    _foreArmAim.weight = 0.75f;
                }
                else
                {
                    _bodyAim.weight += speed * 0.75f;
                    _armAim.weight += speed * 0.75f;
                    _foreArmAim.weight += speed * 0.75f;
                }

                if (_handAim.weight >= 1.0f)
                {
                    _handAim.weight = 1.0f;
                    _hasReachedMax = true;
                }
                else _handAim.weight += speed;
            }
            else
            {
                _aimTarget.gameObject.SetActive(false);
                
                _cinemachine.m_YAxis.m_MaxSpeed = 1.5f;
                _cinemachine.m_XAxis.m_MaxSpeed = 250.0f;
                Cursor.lockState = CursorLockMode.Locked;
            
                _hasReachedMax = false;
                if(_hasReachedMin) return;
                if (_bodyAim.weight <= 0.0f)
                {
                    _bodyAim.weight = 0.0f;
                    _armAim.weight = 0.0f;
                    _foreArmAim.weight = 0.0f;
                }
                else
                {
                    _bodyAim.weight -= speed * 0.75f;
                    _armAim.weight -= speed * 0.75f;
                    _foreArmAim.weight -= speed * 0.75f;
                }

                if (_handAim.weight <= 0.0f)
                {
                    _handAim.weight = 0.0f;
                    _hasReachedMin = true;
                }
                else _handAim.weight -= speed;
            }
        }
        
        private IEnumerator Gun()
        {
            while (Input.GetButton("Fire1"))
            {
                Shoot();
                yield return new WaitForSeconds(0.15f);
            }
        }
        
        private void Shoot()
        {
            var tempBullet = isInflating ? _tempBaloon : Instantiate(_bullet, _bulletSpawner.position, _bulletSpawner.rotation);
            var speed = _bulletSpeed;
            if (_shotGun)
            {
                _shotGun = false;
                //tempBullet = _tempBaloon;
                _tempBaloon = null;
                _hasBaloon = false;
                speed *= 50;
            }
            var rg = tempBullet.GetComponent<Rigidbody>();
            rg.AddForce(_bulletSpawner.transform.forward * speed);
        }

        private void HandleShooting()
        {
            if (_hasReachedMax && !_aimEnter)
            {
                _aimExit = false;
                _aimEnter = true;
                StartCoroutine(Gun());
            }

            if (!IsAiming && !_aimExit)
            {
                _aimEnter = false;
                _aimExit = true;
                StopCoroutine(Gun());
            }

            InflateBubblegum();
        }
        
        private void GetAllAnimHashes()
        {
            _isWalkingHash = Animator.StringToHash("isWalking");
            _isJumpingHash = Animator.StringToHash("isJumping");
            _isFallingHash = Animator.StringToHash("isFalling");
            _isGroundedHash = Animator.StringToHash("isGrounded");
            _isLandingHash = Animator.StringToHash("isLanding");
            _isAimingHash = Animator.StringToHash("IsAiming");
            _speedHash = Animator.StringToHash("Speed");
            _verVelHash = Animator.StringToHash("VerVel");
            _landTimeHash = Animator.StringToHash("LandTime");
        }
        private void SetAnimatorParameters()
        {
            _animator.SetBool(_isWalkingHash, IsWalking);
            _animator.SetBool(_isJumpingHash, IsJumping);
            _animator.SetBool(_isFallingHash, IsFalling);
            _animator.SetBool(_isGroundedHash, IsGrounded);
            _animator.SetBool(_isLandingHash, IsLanding);
            _animator.SetBool(_isAimingHash, IsAiming);
            _animator.SetFloat(_speedHash, InputSpeed);
            _animator.SetFloat(_verVelHash, VerVel);
            _animator.SetFloat(_landTimeHash, LandingTime);
        }
    
        private void GetInputSpeed()
        {
            var temp = _prevLargeDir;
            _prevLargeDir = GetLargestAbsolute(_moveRaw.x, _moveRaw.z);
            if (_prevLargeDir >= temp && _prevLargeDir != 0.0f)
            {
                InputSpeed = 1.0f;
            }
            else
            {
                InputSpeed = GetLargestAbsolute(_move.x, _move.z);
            }
        }
    
        private static float GetLargestAbsolute(float hor, float ver)
        {
            float absHor = Mathf.Abs(hor), absVer = Mathf.Abs(ver);
            return absHor > absVer ? absHor : absVer;
        }

        private static Vector3 ResetYAxis(Vector3 pos) => new Vector3(pos.x, 0.0f, pos.z);

        public void PickUpGun()
        {
            _gumGun.SetActive(true);
            hasGun = true;
        }

        public void PickUpKey()
        {
            hasKey = true;
            goldKey.SetActive(true);
        }

        public void PickUpBear()
        {
            hasBear = true;
            teddyBear.SetActive(true);
        }

        public void UseKey()
        {
            goldKey.SetActive(false);
        }
        
        
        /*private void GroundCheck()
        {
            RaycastHit groundHit;
            if (Physics.Raycast(PlayerPos+Vector3.up, Vector3.down, out groundHit, 1.25f, _groundCheckLayer))
            {
                print("GROUNDED");
                IsGrounded = true;
            }
            else
            {
                IsGrounded = false;
            }
            Debug.DrawRay(PlayerPos, Vector3.down * 0.25f, Color.yellow);
        }*/


        //Parameters------------------------------------------------------------
        public static PlayerMove Player { get; private set; }
        private static Transform PlayerTransform { get; set; }
        public Vector3 PlayerPos => PlayerTransform.position;

        public Vector3 PlayerMovementDir => _moveRaw;
        public Transform PlayerAim => _aim;


        private Vector3 Velocity => _characterController.velocity;
        private float InputSpeed { get; set; }
        private float Gravity => IsAirFlipping ? _airGravity : IsFalling ? _fallGravity : _jumpGravity;
        private float LandingTime
        {
            get
            {
                if (!IsGrounded) _landTime = 0.0f;
                else if (_landTime < 1.0f) _landTime += Time.deltaTime*2;
                else _landTime = 1.0f;
                return _landTime;
            }
        } private float _landTime;
        private float VerVel => IsGrounded ? -_fallGravity : Velocity.y;


        

        //States
        public bool IsGrounded => _characterController.isGrounded;
        private bool IsWalking => _moveRaw.x != 0.0f || _moveRaw.z != 0.0f;
        private bool IsJumping { get; set; }
        private bool IsAirFlipping => Velocity.y < _airFLipStart && Velocity.y > _airFLipEnd && !IsGrounded;
        private bool IsFalling => _characterController.velocity.y < 0.5f && !IsGrounded;
        private bool IsLanding { get; set; } private bool GetLandingState()
        {
            if (!IsGrounded) _isLanding = false;
            else if(!_isLanding)
            {
                _isLanding = true;
                return true;
            }
            return false;
        } private bool _isLanding;
        private bool IsAiming => _shootInput || _inflateInput;
        public void Die()
        {
            SceneLoader.ReloadScene();
        }
    }
}
