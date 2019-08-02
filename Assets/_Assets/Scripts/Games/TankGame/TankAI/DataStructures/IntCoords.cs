public struct IntCoords {

    public int x, y;

    public IntCoords (int x, int y) {
        this.x = x;
        this.y = y;
    }

    public float Distance(int x, int y) {
        float value = Maths.Power(x, 2) + Maths.Power(y, 2);
        return Maths.Sqrt(value);
    }

    public IntCoords MoveToDirection(TankDirection direction) {

        IntCoords coords = new IntCoords(x, y);

        switch (direction) {
            case TankDirection.Up:
                coords.y++;
                break;
            case TankDirection.Right:
                coords.x++;
                break;
            case TankDirection.Down:
                coords.y--;
                break;
            case TankDirection.Left:
                coords.x--;
                break;
            case TankDirection.UpRight:
                coords.x++;
                coords.y++;
                break;
            case TankDirection.DownRight:
                coords.x++;
                coords.y--;
                break;
            case TankDirection.DownLeft:
                coords.x--;
                coords.y--;
                break;
            case TankDirection.UpLeft:
                coords.x--;
                coords.y++;
                break;
        }

        return coords;
    }

    public override string ToString() {
        return "(" + x + ", " + y + ")";
    }
    #region Operators
    public static bool operator ==(IntCoords a, IntCoords b) {
        return a.x == b.x && a.y == b.y;
    }
    public static bool operator !=(IntCoords a, IntCoords b) {
        return a.x != b.x || a.y != b.y;
    }
    #endregion
}