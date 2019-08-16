public enum TankDirection {
    None = -2, Backtrack = -1, Up = 0, Right = 1, Down = 2, Left = 3, UpRight = 4, DownRight = 5, DownLeft = 6, UpLeft = 7
}

[CoverInReport]
public static class TankDirectionTools {

    /// <summary>
    /// Transforms a direction into a byte value where the bit index is equal to the directions integer value.
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public static byte ToByte(TankDirection direction) {
        return (byte)(1 << (int)direction);
    }

    /// <summary>
    /// Sets a corresponding bit to 1 based on the given direction.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="direction"></param>
    public static void SetDirectionBit(ref byte value, TankDirection direction) {
        byte bit = ToByte(direction);
        value = (byte)(value | bit);
    }

    /// <summary>
    /// Checkes whether or not a direction bit is set to 1 based on the given direction.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="direction"></param>
    public static bool AllowedDirection(byte directions, TankDirection direction) {
        return (directions & ToByte(direction)) != 0;
    }
}