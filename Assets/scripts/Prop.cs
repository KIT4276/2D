using UnityEngine;

public class Prop : MonoBehaviour
{
    [SerializeField] private Vector2 _cellSize = Vector2Int.one;
    [Space]
    [SerializeField] private Transform _spriteTransform;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Collider2D _collider;
    [Space]
    [SerializeField] private Color _noOkColor;
    [SerializeField] private Color _okColor;

    public Vector2 CellSize { get => _cellSize; }

    public void SetNoOkColor() => 
        _spriteRenderer.color = _noOkColor;

    public void SetOkColor() => 
        _spriteRenderer.color = _okColor;

    public void SetNormColor()
    {
        _spriteRenderer.color = Color.white;
        _collider.enabled = true;
    }
}
