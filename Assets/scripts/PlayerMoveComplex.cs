using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(PlayerAnimator))]
public class PlayerMoveComplex : MonoBehaviour
{
    [SerializeField] private float _walkSpeed = 1;
    [SerializeField] private float _runningSpeed = 2;

    private NavMeshAgent _navMeshAgent;
    private PlayerAnimator _animator;
    private Camera _camera;
    private ControlType _controlType;

    [SerializeField] private float _moveSpeed;
    private bool _isRunning;
    private Vector2 _moveVector;

    private Vector3 _moucePosition;
    private Vector3 _worldPosition;
    private Vector3 _targetPosition;


    private void Start()
    {
        Initialize();
        SetStartParameters();
    }

    private void Update()
    {
        TryToFindMoveVector();

        TryToFindClickPosition();

        if (_controlType == ControlType.AI)
            Debug.Log(_controlType);

        switch (_controlType)
        {
            case ControlType.WASD:
                UpdateWASD();
                break;
            case ControlType.AI:
                UpdateAI();
                break;
            default:
                UpdateWASD();
                break;
        }

        UpdateAnimator();
    }

    private void TryToFindMoveVector()
    {
        _moveVector = new(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (_moveVector != Vector2.zero)
        {
            _controlType = ControlType.WASD;
        }
    }

    private void TryToFindClickPosition()
    {
        if (Input.GetMouseButtonDown(1))
        {
            _controlType = ControlType.AI;

            _moucePosition = Input.mousePosition;
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(_moucePosition), Vector2.zero);

            if (hit.collider.CompareTag("Ground"))
            {
                _worldPosition = _camera.ScreenToWorldPoint(_moucePosition);
                _targetPosition = new Vector3(_worldPosition.x, _worldPosition.y, 0);
            }
        }
        else if(!_navMeshAgent.hasPath)
            _controlType = ControlType.WASD;
    }

    private void UpdateWASD()
    {
        _navMeshAgent.enabled = false;

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _moveSpeed = _runningSpeed;
            _isRunning = true;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _moveSpeed = _walkSpeed;
            _isRunning = false;
        }

        transform.position += (Vector3)(_moveSpeed * Time.fixedDeltaTime * _moveVector.normalized);
    }

    private void UpdateAI()
    {
        _navMeshAgent.enabled = true;
        _navMeshAgent.SetDestination(_targetPosition);
    }

    private void UpdateAnimator()
    {

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
