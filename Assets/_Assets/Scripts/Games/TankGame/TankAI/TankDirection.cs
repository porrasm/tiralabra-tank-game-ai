
public enum TankDirection { None = -2, Backtrack = -1, Up = 0, Right = 1, Down = 2, Left = 3, UpRight = 4, DownRight = 5, DownLeft = 6, UpLeft = 7 }

public static class TankDirectionTools {
    public static byte ToByte(TankDirection direction) {
        return (byte)(1 << (int)direction);
    }
    public static void SetDirectionBit(ref byte value, TankDirection direction) {
        byte bit = ToByte(direction);
        value = (byte)(value | bit);
    }
    public static bool AllowedDirection(byte directions, TankDirection direction) {
        return (directions & ToByte(direction)) != 0;
    }
}