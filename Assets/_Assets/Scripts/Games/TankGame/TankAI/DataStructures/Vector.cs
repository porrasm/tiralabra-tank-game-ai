using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public struct Vector {

    public float x, y, z;

    public Vector(Vector3 vector3) {
        x = vector3.x;
        y = vector3.y;
        z = vector3.z;
    }
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
    public Vector3 Vector3 {
        get {
            return new Vector3(x, y, z);
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

    public static IntCoords PositionToCoords(Vector3 vector) {
        return PositionToCoords(FromVector3(vector));
    }
    public static IntCoords PositionToCoords(Vector position) {
        return new IntCoords((int)position.x, (int)position.z);
    }

    public static float Distance(Vector3 vector1, Vector3 vector2) {
        return Distance(FromVector3(vector1), FromVector3(vector2));
    }
    public static float Distance(Vector vector1, Vector vector2) {
        Vector distance = vector2 - vector1;
        return Maths.Abs(distance.Magnitude());
    }


    #endregion

    #region Math
    public static float Dot(Vector a, Vector b) {

        double xA = a.x;
        double xB = b.x;
        double yA = a.y;
        double yB = b.y;
        double zA = a.z;
        double zB = b.z;

        return (float)(xA * xB + yA * yB + zA * zB);
    }
    public static double DotAccurate(Vector a, Vector b) {

        double xA = a.x;
        double xB = b.x;
        double yA = a.y;
        double yB = b.y;
        double zA = a.z;
        double zB = b.z;

        return xA * xB + yA * yB + zA * zB;
    }

    // Replace this
    public static Vector Reflect(Vector d, Vector n) {

        return new Vector(Vector3.Reflect(d.Vector3, n.Vector3));

        double dot = DotAccurate(d, n);

        Vector v2dn = 2 * dot * n;

        return d - v2dn;
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

    public static Vector operator *(float f, Vector v) {
        return new Vector(v.x * f, v.y * f, v.z * f);
    }
    public static Vector operator *(double f, Vector v) {
        return new Vector(v.x * f, v.y * f, v.z * f);
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