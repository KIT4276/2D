using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
public class PlayerMoveAI : MonoBehaviour
{
    [SerializeField] private PlayerAnimator _animator;
    [SerializeField] private float _walkSpeed = 1;
    [SerializeField] private float _runningSpeed = 2;
    [Space]
    [SerializeField] float _doubleClickTime = 0.2f;

    private float _lastClickTime;
    private NavMeshAgent _navMeshAgent;
    private Camera _camera;
    private Vector3 _moucePosition;
    private Vector3 _worldPosition;
    private Vector3 _targetPosition;

    private bool _isRunning;

    private void Start()
    {
        _camera = Camera.main;
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.updateRotation = false;
        _navMeshAgent.updateUpAxis = false;
        _navMeshAgent.speed = _walkSpeed;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            float timeSinceLastClick = Time.time - _lastClickTime;
            SetPlayerDestination();

            if (timeSinceLastClick <= _doubleClickTime)
            {
                _navMeshAgent.speed = _runningSpeed;
                _isRunning = true;
            }
            else
            {
                _navMeshAgent.speed = _walkSpeed;
                _isRunning = false;
            }

            _lastClickTime = Time.time; 
        }

        if (_isRunning)
            _animator.ToMove(new Vector2(_navMeshAgent.velocity.x, _navMeshAgent.velocity.y), true);
        else
            _animator.ToMove(new Vector2(_navMeshAgent.velocity.x, _navMeshAgent.velocity.y), false);

        Debug.Log(new Vector2(_navMeshAgent.velocity.x, _navMeshAgent.velocity.y));
    }

    private void SetPlayerDestination()
    {
        _moucePosition = Input.mousePosition;
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(_moucePosition), Vector2.zero);

        if (hit.collider.CompareTag("Ground"))
        {
            _worldPosition = _camera.ScreenToWorldPoint(_moucePosition);
            _targetPosition = new Vector3(_worldPosition.x, _worldPosition.y, 0);
            _navMeshAgent.SetDestination(_targetPosition);
        }
    }
}