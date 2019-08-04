using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public struct Vector {

    public float x, y, z;

    public Vector(float x, float y, float z) {
        this.x = x;
        this.y = y;
        this.z = z;
    }
    public Vector(double x, double y, double z) {
        this.x = (float)x;
        this.y = (float)y;
        this.z = (float)z;
    }

    public float Magnitude() {
        float value = Maths.Power(x, 2) + Maths.Power(y, 2) + Maths.Power(z, 2);
        return Maths.Sqrt(value);
    }

    public Vector Normalized {
        get {
            double magnitude = Magnitude();
            if (magnitude == 0) {
                return this;
            }
            return this / magnitude;
        }
    }

    #region Vector tools
    public static Vector FromVector3(Vector3 vector) {
        return new Vector(vector.x, vector.y, vector.z);
    }
    public static Vector3 ToVector3(Vector vector) {
        return new Vector3(vector.x, vector.y, vector.z);
    }

    public static Vector Zero {
        get {
            return new Vector(0, 0, 0);
        }
    }

    /// <summary>
    /// This is used to get the position of a cell.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns>Cells position in the game.</returns>
    public static Vector CoordsToPosition(IntCoords coords) {

        Vector vector = Zero;

        vector.x = 0.5f + coords.x;
        vector.z = 0.5f + coords.y;

        return vector;
    }
    public static IntCoords PositionToCoords(Vector position) {

        position.x -= 0.5f;
        position.z -= 0.5f;

        return new IntCoords(Maths.Ceil(position.x), Maths.Ceil(position.z));
    }

    public static float Distance(Vector vector1, Vector vector2) {
        Vector distance = vector2 - vector1;
        return Maths.Abs(distance.Magnitude());
    }


    #endregion

    #region Operators
    public static Vector operator +(Vector a, Vector b) {
        return new Vector(a.x + b.x, a.y + b.y, a.z + b.z);
    }
    public static Vector operator -(Vector a, Vector b) {
        return new Vector(a.x - b.x, a.y - b.y, a.z - b.z);
    }

    public static Vector operator *(Vector a, Vector b) {
        return new Vector(a.x * b.x, a.y * b.y, a.z * b.z);
    }
    public static Vector operator /(Vector a, Vector b) {
        return new Vector(a.x / b.x, a.y / b.y, a.z / b.z);
    }

    public static Vector operator *(Vector a, float b) {
        return new Vector(a.x * b, a.y * b, a.z * b);
    }
    public static Vector operator /(Vector a, float b) {
        return new Vector(a.x / b, a.y / b, a.z / b);
    }
    public static Vector operator /(Vector a, double b) {
        return new Vector(a.x / b, a.y / b, a.z / b);
    }

    public static bool operator ==(Vector a, Vector b) {
        return a.x == b.x && a.y == b.y && a.z == b.z;
    }
    public static bool operator !=(Vector a, Vector b) {
        return a.x != b.x || a.y != b.y || a.z != b.z;
    }
    #endregion

    public override string ToString() {
        return "(" + x + ", " + y + ", " + z + ")";
    }
}