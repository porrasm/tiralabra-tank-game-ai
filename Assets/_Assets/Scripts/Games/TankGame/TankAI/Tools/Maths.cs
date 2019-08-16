using System;

/// <summary>
/// Class containing various mathematical functions.
/// </summary>
[CoverInReport]
public class Maths {

    // Replace
    public static float Sqrt(float f) {
        return (float)Math.Sqrt(f);
    }

    public static float Power(float f, int power) {

        double d = f;
        double result = 1;

        if (power > 0) {
            for (int i = 0; i < power; i++) {
                result *= d;
            }
        } else if (power < 0) {
            for (int i = 0; i < power; i++) {
                result /= d;
            }
        }

        return (float)result;
    }

    public static int Power(int n, int power) {

        int result = 1;

        if (power > 0) {
            for (int i = 0; i < power; i++) {
                result *= n;
            }
        } else if (power < 0) {
            for (int i = 0; i < power; i++) {
                result /= n;
            }
        }

        return result;
    }

    public static float Abs(float f) {
        if (f < 0) {
            f *= -1;
        }
        return f;
    }

    /// <summary>
    /// Ceiling function which rounds a float value up to the nearest integer.
    /// </summary>
    /// <param name="f"></param>
    /// <returns></returns>
    public static int Ceil(float f) {

        int i = (int)f;

        if (f > i) {
            i++;
        }

        return i;
    }

    public static float Min(float a, float b) {
        if (a < b) {
            return a;
        } else {
            return b;
        }
    }
}