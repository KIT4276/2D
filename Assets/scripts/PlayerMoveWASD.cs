using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerMoveWASD : MonoBehaviour
{
    [SerializeField] private float _walkSpeed = 1;
    [SerializeField] private float _runningSpeed = 2;
    [Space]
    [SerializeField] private PlayerAnimator _animator;

    [SerializeField] private float _moveSpeed;
    private bool _isRun;
    private Vector2 _moveVector;

    private void Start() =>
        _moveSpeed = _walkSpeed;

    private void FixedUpdate()
    {
        ToMove();
        SpeedSelection();

        _animator.ToMove(_moveVector, _isRun);
    }

    private void SpeedSelection()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _moveSpeed = _runningSpeed;
            _isRun = true;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _moveSpeed = _walkSpeed;
            _isRun = false;
        }
    }

    private void ToMove() =>
        transform.position += (Vector3)(_moveSpeed * Time.fixedDeltaTime * MoveInput().normalized);

    private Vector2 MoveInput()
    {
        _moveVector = new(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        return _moveVector;
    }
}
