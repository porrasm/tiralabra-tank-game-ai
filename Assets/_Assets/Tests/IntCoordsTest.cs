using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests {
    public class IntCoordsTest {
        [Test]
        public void EqualsTest() {

            IntCoords a = new IntCoords();
            IntCoords b = new IntCoords();
            IntCoords c = new IntCoords(1, 0);

            Assert.IsTrue(a.Equals(b));
            Assert.IsFalse(a.Equals(c));
            Assert.IsFalse(a.Equals(1));
        }

        [Test]
        public void OperatorsTest() {

            IntCoords a = new IntCoords();
            IntCoords b = new IntCoords();
            IntCoords c = new IntCoords(1, 0);

            Assert.IsTrue(a == b);
            Assert.IsFalse(a != b);

            Assert.IsFalse(a == c);
            Assert.IsTrue(a != c);
        }

        [Test]
        public void CoordsAreMovedCorrectlyTest() {

            IntCoords c = new IntCoords();

            Assert.AreEqual(new IntCoords(0, 1), c.MoveToDirection(TankDirection.Up), "Up");
            Assert.AreEqual(new IntCoords(1, 0), c.MoveToDirection(TankDirection.Right), "Right");
            Assert.AreEqual(new IntCoords(0, -1), c.MoveToDirection(TankDirection.Down), "Down");
            Assert.AreEqual(new IntCoords(-1, 0), c.MoveToDirection(TankDirection.Left), "Left");
            Assert.AreEqual(new IntCoords(1, 1), c.MoveToDirection(TankDirection.UpRight), "UpRight");
            Assert.AreEqual(new IntCoords(1, -1), c.MoveToDirection(TankDirection.DownRight), "DownRight");
            Assert.AreEqual(new IntCoords(-1, -1), c.MoveToDirection(TankDirection.DownLeft), "DownLeft");
            Assert.AreEqual(new IntCoords(-1, 1), c.MoveToDirection(TankDirection.UpLeft), "UpLeft");
        }
    }
}
