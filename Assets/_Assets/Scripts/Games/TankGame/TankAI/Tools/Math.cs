using System;
using UnityEngine;

class Maths {

    // Replace
    public static float Sqrt(float f) {
        return Mathf.Sqrt(f);
    }

    public static float Power(float f, int power) {

        float result = 1;

        if (power > 0) {
            for (int i = 0; i < power; i++) {
                result *= f;
            }
        } else if (power < 0) {
            for (int i = 0; i < power; i++) {
                result /= f;
            }
        }


        return result;
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
}