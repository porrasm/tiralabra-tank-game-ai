using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests {
    public class LinkedPriorityListTest {

        

        [Test]
        public void AddTest() {

            LinkedPriorityList<int> list = new LinkedPriorityList<int>();

            list.Add(0, 10);

            Assert.AreEqual(1, list.Count);
            Assert.AreEqual(0, list.First);
            Assert.AreEqual(0, list.Remove());

            list.Add(0, 10);
            list.Add(1, 8);

            Assert.AreEqual(2, list.Count);
            Assert.AreEqual(1, list.First);

        }

        [Test]
        public void OrderTest() {

            LinkedPriorityList<int> list = new LinkedPriorityList<int>();

            list.Add(8,8);
            list.Add(4,4);
            list.Add(5,5);
            list.Add(0,0);
            list.Add(2,2);
            list.Add(6,6);
            list.Add(3,3);
            list.Add(7,7);
            list.Add(9,9);
            list.Add(1,1);

            Assert.AreEqual(10, list.Count, "Count should be 10");

            for (int i = 0; i < 10; i++) {
                Assert.AreEqual(i, list.Remove(), "The " + (i+1) + "th value should be " + i);
            }
        }

        [Test]
        public void ReverseOrderTest() {

            LinkedPriorityList<int> list = new LinkedPriorityList<int>();

            for (int i = 99; i > -1; i--) {
                list.Add(i, i);
            }

            Assert.AreEqual(100, list.Count, "Count should be 100");

            for (int i = 0; i < 100; i++) {
                Assert.AreEqual(i, list.Remove());
            }

        }

    }
}
