using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests {
    public class LinkedPriorityListTest {

        private LinkedPriorityList<int> list;

        [SetUp]
        public void SetUp() {
            list = new LinkedPriorityList<int>();

            list.Add(8, 8);
            list.Add(4, 4);
            list.Add(5, 5);
            list.Add(0, 0);
            list.Add(2, 2);
            list.Add(6, 6);
            list.Add(3, 3);
            list.Add(7, 7);
            list.Add(9, 9);
            list.Add(1, 1);
        }


        [Test]
        public void ToArrayTest() {

            int[] array = list.ToArray();

            for (int i = 0; i < array.Length; i++) {
                Assert.AreEqual(i, array[i]);
            }

            list.Add(10, 10);
            array = list.ToArray();

            for (int i = 0; i < array.Length; i++) {
                Assert.AreEqual(i, array[i]);
            }
        }

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

            Assert.AreEqual(10, list.Count, "Count should be 10");

            for (int i = 0; i < 10; i++) {
                Assert.AreEqual(i, list.Remove(), "The " + (i + 1) + "th value should be " + i);
            }
        }

        [Test]
        public void GetTest() {

            if (list == null) {
                Assert.Fail("list null");
            }

            for (int i = 0; i < 10; i++) {
                Assert.AreEqual(i, list.Get(i), "The " + (i + 1) + "th value should be " + i);
            }

            try {
                list.Get(10);
                Assert.Fail("Getting an item out of the index range should not be allowed");
            } catch (System.Exception) {

            }
        }

        [Test]
        public void ClearTest() {

            list.Clear();

            Assert.AreEqual(0, list.Count);

            try {
                int a = list.First;
                Assert.Fail("Getting from an empty list should cause an error");
            } catch (System.Exception) {
            }

            list.Add(1, 1);
            list.Add(2, 2);

            Assert.AreEqual(1, list.Remove());
            Assert.AreEqual(2, list.Remove());
        }

        [Test]
        public void RemoveTest() {

            Assert.AreEqual(0, list.Remove());
            Assert.AreEqual(1, list.Remove());

            list.Clear();

            try {

                Assert.AreEqual(0, list.Remove());
                Assert.Fail("Removing from empty list is not allowed");
            } catch (System.Exception) {

            }
        }

        [Test]
        public void UpdateTest() {

            list.Update(0, 10);

            Assert.AreEqual(1, list.First);
            Assert.AreEqual(0, list.Get(9));

            list.Update(9, 0);

            Assert.AreEqual(9, list.First);
        }

        [Test]
        public void RemoveElementOfTypeTest() {

            Assert.IsTrue(list.Remove(0));
            Assert.IsFalse(list.Remove(10));

            Assert.AreEqual(1, list.First);
            Assert.AreEqual(9, list.Count);

            Assert.AreEqual(2, list.Get(1));

            SetUp();

            list.Remove(1);

            Assert.AreEqual(0, list.First);
            Assert.AreEqual(2, list.Get(1));

            Assert.IsFalse(list.Remove(1));

            SetUp();

            Assert.IsTrue(list.Remove(9));

            Assert.IsTrue(list.Remove(8));

            Assert.IsFalse(list.Remove(8));
        }

        [Test]
        public void FindTest() {

            int param = -1;

            Assert.IsTrue(list.Find(0, out param));

            Assert.AreEqual(0, param);

            Assert.IsTrue(list.Find(1, out param));

            Assert.AreNotEqual(0, param);

            Assert.IsFalse(list.Find(10, out param));
            Assert.AreEqual(0, param);

            list.Clear();

            param = 10;

            Assert.IsFalse(list.Find(0, out param));
            Assert.AreEqual(0, param);
        }

        [Test]
        public void ContainsTest() {

            Assert.AreEqual(true, list.Contains(0));
            Assert.AreEqual(true, list.Contains(9));
            Assert.AreEqual(false, list.Contains(10));

            list.Clear();

            Assert.AreEqual(false, list.Contains(0), "List should not contain any elements after clearing the list");
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
