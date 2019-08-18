using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
//using UnityEngine;
using UnityEngine.TestTools;
using System;

namespace Tests {
    public class MathsTest {

        private System.Random rnd = new System.Random();

        #region Square root
        [Test]
        public void MathsSquareRootTest() {

            float f = 4;
            Assert.AreEqual(2, Maths.Sqrt(f));

            f = 16;
            Assert.AreEqual(4, Maths.Sqrt(f));

            f = 1;
            Assert.AreEqual(1, Maths.Sqrt(f));

            f = 0;
            Assert.AreEqual(0, Maths.Sqrt(f));

            f = 4;
            Assert.AreEqual(2, Maths.Sqrt(f));
        }
        [Test]
        public void MathsSquareRootTestRandom() {
            for (int i = 0; i < 100; i++) {
                float f = RndFloat * 10000;
                Assert.AreEqual((float)Math.Sqrt(f), Maths.Sqrt(f));
            }
        }
        #endregion

        #region Power
        [Test]
        public void IntegerPowerTest() {

            Assert.AreEqual(1, Maths.Power(0, 0), "0 ^ 0");

            Assert.AreEqual(1, Maths.Power(1, 1), "1 ^ 1");

            Assert.AreEqual(1, Maths.Power(1, 2), "1 ^ 2");

            Assert.AreEqual(2, Maths.Power(2, 1), "2 ^ 1");

            Assert.AreEqual(4, Maths.Power(2, 2), "2 ^ 2");

            Assert.AreEqual(8, Maths.Power(2, 3), "2 ^ 3");

            Assert.AreEqual(1, Maths.Power(2, 0), "2 ^ 0");

            try {
                Maths.Power(2, -1);
                Assert.Fail("Power cannot be negative");
            } catch (Exception) {
            }
        }
        [Test]
        public void IntegerPowerTestRandom() {
            for (int i = 0; i < 10; i++) {
                for (int p = 0; p < 10; p++) {
                    Assert.AreEqual(Math.Pow(i, p), Maths.Power(i, p), i + " ^ " + p);
                }
            }
        }

        [Test]
        public void MinTest() {

            Assert.AreEqual(1, Maths.Min(1, 2));
            Assert.AreEqual(1f, Maths.Min(1.0001f, 1));

            Assert.AreEqual(-10, Maths.Min(0, -10));
        }

        [Test]
        public void FloatPowerTest() {
            Assert.AreEqual(1, Maths.Power(0f, 0), "0 ^ 0");
            Assert.AreEqual(1, Maths.Power(1f, 1), "1 ^ 1");
            Assert.AreEqual(1, Maths.Power(1f, 2), "1 ^ 2");
            Assert.AreEqual(2, Maths.Power(2f, 1), "2 ^ 1");
            Assert.AreEqual(4, Maths.Power(2f, 2), "2 ^ 2");
            Assert.AreEqual(8, Maths.Power(2f, 3), "2 ^ 3");
            Assert.AreEqual(1, Maths.Power(2f, 0), "2 ^ 0");
            Assert.AreEqual(6.25f, Maths.Power(2.5f, 2), "2.5 ^ 2");

            Assert.AreEqual(0.5f, Maths.Power(2f, -1), "2 ^ -1");
            Assert.AreEqual(0.25f, Maths.Power(2f, -2), "2 ^ -2");

            Assert.AreEqual(1f / 3f, Maths.Power(3f, -1), "3 ^ -1");
        }
        [Test]
        public void FloatPowerTestRandom() {
            for (int i = 0; i < 10; i++) {

                float f = i + RndFloat;

                for (int p = 0; p < 10; p++) {
                    Assert.AreEqual((float)Math.Pow(f, p), Maths.Power(f, p), 0.0001f, f + " ^ " + p);
                }
            }
        }
        #endregion

        #region Absolute
        [Test]
        public void AbsoluteTest() {
            Assert.AreEqual(5, Maths.Abs(5));
            Assert.AreEqual(0, Maths.Abs(0));
            Assert.AreEqual(5, Maths.Abs(-5));
        }
        #endregion

        #region Ceiling
        [Test]
        public void CeilingTest() {
            Assert.AreEqual(1, Maths.Ceil(0.5f));
            Assert.AreEqual(0, Maths.Ceil(-0.5f));
            Assert.AreEqual(1, Maths.Ceil(1));
            Assert.AreEqual(1, Maths.Ceil(0.000001f));
        }
        #endregion

        private float RndFloat {
            get { return (float)rnd.NextDouble(); }
        }
    }
}
