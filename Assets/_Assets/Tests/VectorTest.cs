using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests {
    public class VectorTest {

        private float delta = 0.000001f;

        [Test]
        public void MagnitudeTest() {

            Vector vector = new Vector(1, 1, 1);
            Vector3 vector3 = new Vector3(1, 1, 1);

            Assert.AreEqual(vector3.magnitude, vector.Magnitude(), delta, "(1, 1, 1)");


            vector = new Vector(0, 0, 0);
            vector3 = new Vector3(0, 0, 0);

            Assert.AreEqual(vector3.magnitude, vector.Magnitude(), delta, "(0, 0, 0)");

            vector = new Vector(10, 0, 0);
            vector3 = new Vector3(10, 0, 0);

            Assert.AreEqual(vector3.magnitude, vector.Magnitude(), 0, "(10, 0, 0)");
        }

        [Test]
        public void NormalizedTest() {

            Vector vector = new Vector(1, 0, 0);

            Assert.AreEqual(1, vector.Normalized.Magnitude(), "(1, 0, 0)");

            vector = new Vector(1, 1, 1);
            Assert.AreEqual(new Vector3(1, 1, 1).normalized.magnitude, vector.Normalized.Magnitude(), delta, "(1, 1, 1) Vector3 comparison");
            Assert.AreEqual(1, vector.Normalized.Magnitude(), delta, "(1, 1, 1)");

            vector = new Vector(0, 0, 0);
            Assert.AreEqual(0, vector.Normalized.Magnitude(), delta, "(0, 0, 0)");

            vector = new Vector(10, 10, 10);
            Assert.AreEqual(1, vector.Normalized.Magnitude(), delta, "(10, 10, 10)");

            vector = new Vector(100, 0, 0);
            Assert.AreEqual(1, vector.Normalized.Magnitude(), delta, "(100, 0, 0)");
            Assert.AreEqual(1, vector.Normalized.x, delta, "X value should be 1");
            Assert.AreEqual(0, vector.Normalized.y, "Y value should be 0");
        }

        [Test]
        public void OperatorsAndEqualsTest() {

            Vector a = new Vector(1, 1, 1);
            Vector b = new Vector(1, 1, 1);
            Vector c = new Vector(0, 0, 0);
            Vector d = new Vector(2, 2, 2);

            Assert.IsTrue(a == b);
            Assert.IsFalse(a == c);

            Assert.AreEqual(a, a + c, "a == a + c");
            Assert.AreEqual(d, a + b, "d == a + b");

            Assert.AreEqual(c, a - b, "c == a - b");
            Assert.AreEqual(a, d - b, "a == d - b");

            Assert.AreEqual(c, c * a, "c == c * a");
            Assert.AreEqual(d, 2 * a, "d == a * 2");
            Assert.AreEqual(d, 2.0 * a, "d == a * 2");
            Assert.AreEqual(d, a * d, "d == a * d");

            Assert.AreEqual(d, d / a, "d == d / a");
            Assert.AreEqual(a, d / 2, "a == d / 2");
            Assert.AreEqual(c, c / d, "c == c / d");

            Assert.IsFalse(a.Equals(1));
        }

        [Test]
        public void ConversionsTest() {

            Vector a = new Vector(1, 1, 1);
            Vector3 b = new Vector3(1, 1, 1);

            Assert.IsTrue(a == b);
            Assert.IsTrue(b == a);
            Assert.IsTrue(a == new Vector(a.Vector3));
            Assert.IsTrue(b == new Vector(b).Vector3);

            Assert.IsFalse(a != b);
            Assert.IsFalse(b != a);
            Assert.IsFalse(a != new Vector(a.Vector3));
            Assert.IsFalse(b != new Vector(b).Vector3);
        }

        [Test]
        public void ConversionsTest2() {

            Vector a = new Vector(1, 1, 1);
            Vector b = new Vector(0.5f, 0, 0.5f);
            Vector3 c = new Vector3(1, 1, 1);
            IntCoords d = new IntCoords();

            Vector aV = c;
            Vector bV = d;

            Assert.AreEqual(a, aV);
            Assert.AreEqual(b, bV);
        }

        [Test]
        public void DotProductTest() {

            Vector a = new Vector(1, 1, 1);
            Vector b = new Vector(2, 2, 2);
            Vector c = new Vector(2, 3, 4);

            Assert.AreEqual(3, Vector.Dot(a, a));
            Assert.AreEqual(6, Vector.Dot(a, b));
            Assert.AreEqual(9, Vector.Dot(a, c));
            Assert.AreEqual(18, Vector.Dot(c, b));

            Assert.AreEqual(3, Vector.DotAccurate(a, a));
            Assert.AreEqual(6, Vector.DotAccurate(a, b));
            Assert.AreEqual(9, Vector.DotAccurate(a, c));
            Assert.AreEqual(18, Vector.DotAccurate(c, b));
        }

        [Test]
        public void ReflectTest() {

            Vector v = new Vector(1, 0, 0);

            Vector n = new Vector(-1, 0, 0);

            Assert.AreEqual(n.Normalized, Vector.Reflect(v, n).Normalized);

            v = new Vector(1, -1, 0);

            n = new Vector(0, 1, 0);

            Assert.AreEqual(new Vector(1, 1, 0).Normalized, Vector.Reflect(v, n).Normalized);

            v = new Vector(1, 1, 1);

            n = new Vector(-1, -1, -1);

            Assert.AreEqual(n.Normalized, Vector.Reflect(v, n).Normalized);
        }

        [Test]
        public void IntCoordsToVector() {

            IntCoords coords = new IntCoords();
            Assert.AreEqual(new Vector(0.5f, 0, 0.5f), Vector.CoordsToPosition(coords));

            coords = new IntCoords(1, 1);
            Assert.AreEqual(new Vector(1.5f, 0, 1.5f), Vector.CoordsToPosition(coords));
        }

        [Test]
        public void VectorToIntCoords() {

            Vector vector = new Vector();

            IntCoords coords = new IntCoords();
            Assert.AreEqual(coords, Vector.PositionToCoords(vector));

            vector = new Vector(0.001f, 0.001f, 0.001f);
            coords = new IntCoords();
            Assert.AreEqual(coords, Vector.PositionToCoords(vector));
        }
    }
}
