using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests {
    public class CStackTest {

        private CStack<int> stack;
        private int[] array;

        [SetUp]
        public void SetUp() {
            stack = new CStack<int>();
            array = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        }

        [Test]
        public void AddingAndPoppingAndPeekingAndCountAndFitToSizeWorks() {

            foreach (int i in array) {
                stack.Push(i);
            }

            Assert.AreEqual(array.Length, stack.Count);
            Assert.AreEqual(16, stack.Capacity);


            for (int i = array.Length - 1; i >= 0; i--) {
                Assert.AreEqual(i, stack.Peek());
                Assert.AreEqual(i, stack.Pop());
                Assert.AreEqual(i, stack.Count);
            }

            Assert.AreEqual(0, stack.Count);
            Assert.AreEqual(16, stack.Capacity);

            stack.FitToSize();

            Assert.AreEqual(8, stack.Capacity);
        }

        [Test]
        public void CapacityIncreasesCorrectly() {

            for (int i = 0; i < 1024; i++) {
                stack.Push(i);
            }

            Assert.AreEqual(1024, stack.Capacity);
            stack.Push(1024);
            Assert.AreEqual(2048, stack.Capacity);
            stack.Pop();

            Assert.AreEqual(2048, stack.Capacity);
            stack.FitToSize();
            Assert.AreEqual(1024, stack.Capacity);

            while (stack.Count > 0) {
                stack.Pop();
            }

            stack.FitToSize();

            Assert.AreEqual(8, stack.Capacity);
        }
    }
}
