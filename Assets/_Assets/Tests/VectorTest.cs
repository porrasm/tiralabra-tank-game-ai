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
            Assert.AreEqual(new Vector3(1, 1, 1).normalized.magnitude, vector.Normalized.Magnitude(), "(1, 1, 1) Vector3 comparison");
            Assert.AreEqual(1, vector.Normalized.Magnitude(), "(1, 1, 1)");

            vector = new Vector(0, 0, 0);
            Assert.AreEqual(0, vector.Normalized.Magnitude(), "(0, 0, 0)");

            vector = new Vector(10, 10, 10);
            Assert.AreEqual(1, vector.Normalized.Magnitude(), "(10, 10, 10)");

            vector = new Vector(100, 0, 0);
            Assert.AreEqual(1, vector.Normalized.Magnitude(), "(100, 0, 0)");
            Assert.AreEqual(1, vector.Normalized.x, "X value should be 1");
            Assert.AreEqual(0, vector.Normalized.y, "Y value should be 0");
        }

        [Test]
        public void OperatorsTest() {

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
            Assert.AreEqual(d, a * d, "d == a * d");

            Assert.AreEqual(d, d / a, "d == d / a");
            Assert.AreEqual(a, d / 2, "a == d / 2");
            Assert.AreEqual(c, c / d, "c == c / d");
        }

        [Test]
        public void ConversionsTest() {

            Vector a = new Vector(1, 1, 1);
            Vector3 b = new Vector3(1, 1, 1);

            Assert.IsTrue(a == Vector.FromVector3(b));
            Assert.IsTrue(b == Vector.ToVector3(a));
            Assert.IsTrue(a == Vector.FromVector3(Vector.ToVector3(a)));
            Assert.IsTrue(b == Vector.ToVector3(Vector.FromVector3(b)));
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
