using UnityEngine;

public class PropsGrid : MonoBehaviour
{
    private Prop _activeProp;
    private Camera _camera;

    private Vector3 _worldPosition;
    private Vector2 _targetPosition;
    private Vector3 _moucePosition;
    private RaycastHit2D _hit;

    private int _int_X;
    private int _int_Y;

    public void StartPlacingBuilding(Prop propPrefab)
    {
        if (_activeProp != null)
            Destroy(_activeProp.gameObject);

        _activeProp = Instantiate(propPrefab);
    }

    public void Close() => 
        Application.Quit();

    private void Awake() => 
        _camera = Camera.main;

    private void Update()
    {
        if (_activeProp != null)
        {
            if (FindHit().collider != null && _hit.collider.CompareTag("Ground"))
            {
                FindPosition();
                PlaceProp();
            }

            if (IsAvailable(_hit))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    _activeProp.SetNormColor();
                    _activeProp = null;
                }
                else
                    _activeProp.SetOkColor();
            }
            else
                _activeProp.SetNoOkColor();
        }
    }

    private RaycastHit2D FindHit()
    {
        _moucePosition = Input.mousePosition;
        _hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(_moucePosition), Vector2.zero);
        return _hit;
    }

    private void FindPosition()
    {
        _worldPosition = _camera.ScreenToWorldPoint(_moucePosition);

        _int_X = Mathf.RoundToInt(_worldPosition.x);
        _int_Y = Mathf.RoundToInt(_worldPosition.y);

        _targetPosition = new Vector2(_int_X, _int_Y);
    }

    private void PlaceProp()
    {
        _activeProp.transform.position = new Vector2(_targetPosition.x, _targetPosition.y);
        _activeProp.SetOkColor();
    }

    private bool IsAvailable(RaycastHit2D hit)
    {
        if (hit.collider != null && hit.collider.gameObject.layer == 6)
                return true;
        else
            return false;
    }
}
