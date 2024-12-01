public class GridCell
{
    public float _centerX;
    public float _centerY;
    public bool _isOccupied;

    public GridCell(float x, float y, bool isOccupied)
    {
        this._centerX = x;
        this._centerY = y;
        this._isOccupied = isOccupied;
    }
}
     