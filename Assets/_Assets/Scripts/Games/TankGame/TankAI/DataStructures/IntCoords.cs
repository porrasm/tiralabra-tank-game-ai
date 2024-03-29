﻿/// <summary>
/// Simple structure containing x and y coordinates.
/// </summary>
[CoverInReport]
public struct IntCoords {

    public int x, y;

    public IntCoords(int x, int y) {
        this.x = x;
        this.y = y;
    }

    /// <summary>
    /// Moves the coordinates based on the direction.
    /// </summary>
    /// <param name="direction"></param>
    /// <returns>The new coordinates</returns>
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
    public override bool Equals(object obj) {
        if (obj.GetType() == GetType()) {
            return this == (IntCoords)obj;
        }

        return false;
    }

    /// <summary>
    /// 'Perfect' hash function if all coordinates are below short.MaxValue
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode() {

        int x = (this.x % short.MaxValue) & 0xFFFF;
        int y = (this.y % short.MaxValue) & 0xFFFF;

        return(x << 16) | y;
    }

    public static bool operator ==(IntCoords a, IntCoords b) {
        return a.x == b.x && a.y == b.y;
    }
    public static bool operator !=(IntCoords a, IntCoords b) {
        return a.x != b.x || a.y != b.y;
    }
    #endregion
}