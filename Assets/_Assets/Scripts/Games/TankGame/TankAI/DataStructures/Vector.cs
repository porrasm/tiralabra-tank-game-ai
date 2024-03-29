﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Vector struct containing multiple vector related mathematical functions.
/// </summary>
[CoverInReport]
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

    /// <summary>
    /// Returns the length of the vector
    /// </summary>
    /// <returns></returns>
    public float Magnitude() {

        float value = Maths.Power(x, 2) + Maths.Power(y, 2) + Maths.Power(z, 2);
        return Maths.Sqrt(value);
    }

    /// <summary>
    /// Returns a vector with the same direction with magnitude 1
    /// </summary>
    public Vector Normalized {
        get {
            double magnitude = Magnitude();
            if (magnitude == 0) {
                return this;
            }
            return this / magnitude;
        }
    }

    /// <summary>
    /// Transform the vector into a Unity Vector3
    /// </summary>
    public Vector3 Vector3 {
        get {
            return new Vector3(x, y, z);
        }
    }

    #region Vector tools
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

    /// <summary>
    /// Returns the corresponding integer coordinates of a position vector.
    /// </summary>
    /// <param name="position"></param>
    /// <returns>Coordinates as IntCoords</returns>
    public static IntCoords PositionToCoords(Vector position) {
        return new IntCoords((int)position.x, (int)position.z);
    }

    /// <summary>
    /// Returns the distance between to vectors.
    /// </summary>
    /// <param name="vector1"></param>
    /// <param name="vector2"></param>
    /// <returns></returns>
    public static float Distance(Vector vector1, Vector vector2) {
        Vector distance = vector2 - vector1;
        return Maths.Abs(distance.Magnitude());
    }


    #endregion

    #region Math
    /// <summary>
    /// Vector dot product as float.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static float Dot(Vector a, Vector b) {

        double xA = a.x;
        double xB = b.x;
        double yA = a.y;
        double yB = b.y;
        double zA = a.z;
        double zB = b.z;

        return (float)(xA * xB + yA * yB + zA * zB);
    }

    /// <summary>
    /// Vector dot product as double.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Reflects a vector off a normal.
    /// </summary>
    /// <param name="d"></param>
    /// <param name="n"></param>
    /// <returns></returns>
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

    public static implicit operator Vector(Vector3 v) {
        return new Vector(v);
    }
    public static implicit operator Vector(IntCoords c) {
        return CoordsToPosition(c);
    }
    #endregion

    public override string ToString() {
        return "(" + x + ", " + y + ", " + z + ")";
    }

    public override bool Equals(object obj) {
        if (!(obj is Vector)) {
            return false;
        }

        var vector = (Vector)obj;
        return x == vector.x &&
               y == vector.y &&
               z == vector.z;
    }

    public override int GetHashCode() {
        var hashCode = 1886620659;
        hashCode = hashCode * -1521134295 + x.GetHashCode();
        hashCode = hashCode * -1521134295 + y.GetHashCode();
        hashCode = hashCode * -1521134295 + z.GetHashCode();
        return hashCode;
    }
}