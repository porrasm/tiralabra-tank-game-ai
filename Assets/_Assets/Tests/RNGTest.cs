using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests {
    public class RNGTest {

        [Test]
        public void RNGValuesTest() {

            int a = 0;
            int b = 0;
            int delta = 10000;


            for (int i = 0; i < 1000000; i++) {

                float random = RNG.Float;

                if (random < 0) {
                    Assert.Fail("Value cannot be negative");
                } if (random < 0.5) {
                    a++;
                } else if (random < 1) {
                    b++;
                } else {
                    Assert.Fail("Value cannot be more than 1");
                }
            }

            int dif = Math.Abs(a - b);
            if (dif > delta) {
                Assert.Fail("Random values are not random enough");
            }
        }
    }
}
