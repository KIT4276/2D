using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(PlayerAnimator))]
public class PlayerMoveComplex : MonoBehaviour
{
    [SerializeField] private float _walkSpeed = 1;
    [SerializeField] private float _runningSpeed = 2;
    [Space]
    [SerializeField] private float _doubleClickTime = 0.2f;
    [SerializeField] private CharacterController _characterController;

    private const string Horizontal = "Horizontal";
    private const string Vertical = "Vertical";
    private const string GroundTag = "Ground";

    private NavMeshAgent _navMeshAgent;
    private PlayerAnimator _animator;
    private Camera _camera;
    private ControlType _controlType;

    private float _moveSpeed;
    private bool _isRunning;
    private Vector2 _moveVector;

    private Vector3 _moucePosition;
    private Vector3 _worldPosition;
    private Vector3 _targetPosition;
    private float _lastClickTime;

    private void Start()
    {
        Initialize();
        SetStartParameters();
    }

    private void Update()
    {
        TryToFindMoveVector();

        TryToFindClickPosition();

        SpeedSelection();

        if (_controlType == ControlType.WASD)
        {
            UpdateWASD();
            _animator.ToMove(_moveVector, _isRunning);
        }

        if (_controlType == ControlType.AI)
        {
            UpdateAI();
            _animator.ToMove(_navMeshAgent.velocity.normalized, _isRunning);
        }
    }

    private void TryToFindMoveVector()
    {
        _moveVector = new Vector2(Input.GetAxisRaw(Horizontal), Input.GetAxisRaw(Vertical)).normalized;

        if (_moveVector != Vector2.zero)
        {
            _controlType = ControlType.WASD;
        }
    }

    private void TryToFindClickPosition()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _controlType = ControlType.AI;
            _moucePosition = Input.mousePosition;

            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(_moucePosition), Vector2.zero);

            if (hit.collider != null && hit.collider.CompareTag(GroundTag))
            {
                _worldPosition = _camera.ScreenToWorldPoint(_moucePosition);
                _targetPosition = new Vector3(_worldPosition.x, _worldPosition.y, 0);
            }
        }
        else if (!_navMeshAgent.hasPath)
            _controlType = ControlType.WASD;
    }

    private void UpdateWASD()
    {
        _navMeshAgent.enabled = false;

        if (_isRunning)
            _moveSpeed = _runningSpeed;
        else
            _moveSpeed = _walkSpeed;

        _characterController.Move((Vector3)(_moveSpeed * Time.deltaTime * _moveVector));
    }

    private void UpdateAI()
    {
        _navMeshAgent.enabled = true;

        if (_isRunning)
            _navMeshAgent.speed = _runningSpeed;
        else
            _navMeshAgent.speed = _walkSpeed;

        _navMeshAgent.SetDestination(_targetPosition);
        
    }

    private void SpeedSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            float timeSinceLastClick = Time.time - _lastClickTime;

            if (timeSinceLastClick <= _doubleClickTime)
            {
                _isRunning = true;
            }
            else
            {
                _isRunning = false;
            }

            _lastClickTime = Time.time;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            _isRunning = true;
        }
        else if (_controlType == ControlType.WASD)
            _isRunning = false;

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _isRunning = false;
        }

        if (_controlType == ControlType.WASD && _moveVector == Vector2.zero)
            _isRunning = false;
    }


    #region Service
    private void Initialize()
    {
        _moveSpeed = _walkSpeed;
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<PlayerAnimator>();
        _camera = Camera.main;
    }

    private void SetStartParameters()
    {
        _navMeshAgent.updateRotation = false;
        _navMeshAgent.updateUpAxis = false;
        _navMeshAgent.speed = _walkSpeed;
    }

    public enum ControlType
    {
        WASD = 0,
        AI = 1,
    }
    #endregion Service
}
