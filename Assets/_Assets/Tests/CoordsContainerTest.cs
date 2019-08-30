using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests {
    public class CoordsContainerTest {

        [Test]
        public void CoordsContainerTestSimplePasses() {

            TankSettings.LevelWidth = 10;
            TankSettings.LevelHeight = 10;

            CoordsContainer cont = new CoordsContainer();

            cont.Add(new IntCoords(1, 1));

            Assert.AreEqual(1, cont.Count);
            Assert.AreEqual(true, cont.Contains(new IntCoords(1, 1)));

            for (int i = 0; i < 10; i++) {
                cont.Add(new IntCoords(i, 1));
            }

            Assert.AreEqual(10, cont.Count);
            Assert.AreEqual(true, cont.Contains(new IntCoords(1, 1)));
        }
    }
}
