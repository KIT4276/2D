using UnityEngine;

[RequireComponent (typeof(Animator))]
public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private const string RunHash = "Run";
    private const string X_Hash = "X";
    private const string Y_Hash = "Y";

    private bool _facingRight;

    public void ToMove(Vector2 moveVector, bool isRun)
    {
        _animator.SetFloat(X_Hash, moveVector.normalized.x);
        _animator.SetFloat(Y_Hash, moveVector.normalized.y);

        if (moveVector != Vector2.zero)
        {
            if (isRun)
                _animator.SetBool(RunHash, true);
            else
                _animator.SetBool(RunHash, false);

            if (moveVector.x > 0 && !_facingRight)
                Flip();
            else if (moveVector.x < 0 && _facingRight)
                Flip();
        }
        else
        {
            _animator.SetBool(RunHash, false);
        }

    }

    private void Flip()
    {
        _facingRight = !_facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
