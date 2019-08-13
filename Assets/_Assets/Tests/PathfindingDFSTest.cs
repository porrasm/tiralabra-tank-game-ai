using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests {

    public class PathfindingDFSTest {

        private byte[,] level;
        private TankMazeGenerator generator;
        private TankAIPathfinding pf;

        [SetUp]
        public void Setup() {

            TankSettings.LevelWidth = 10;
            TankSettings.LevelHeight = 10;

            generator = new TankMazeGenerator();

            List<TankLevelGenerator.Step> steps;

            generator.GenerateMaze(out steps, out level);

            pf = new TankDFSPath(level);
        }

        [Test]
        public void RouteToEndIsStraight() {

            ClearLevel();

            IntCoords start = new IntCoords();

            // Right
            IntCoords end = new IntCoords(9, 0);

            Vector[] route = pf.FindPath(start, end);

            TestRoute(start, end, route);

            for (int i = 0; i < 10; i++) {
                TestCoords(new IntCoords(i, 0), route[i], "Route to right was not straight");
            }

            // Up
            end = new IntCoords(0, 9);

            route = pf.FindPath(start, end);

            TestRoute(start, end, route);

            for (int i = 0; i < 10; i++) {
                TestCoords(new IntCoords(0, i), route[i], "Route to up was not straight");
            }

            // Up-Right
            end = new IntCoords(9, 9);

            route = pf.FindPath(start, end);

            TestRoute(start, end, route);

            for (int i = 0; i < 10; i++) {
                TestCoords(new IntCoords(i, i), route[i], "Route to up-right was not straight");
            }
        }

        [Test]
        public void RouteIsFoundToEveryCell() {

            IntCoords start = new IntCoords();

            void To(int x, int y) {

                IntCoords end = new IntCoords(x, y);

                Vector[] route = pf.FindPath(start, end);

                TestRoute(start, end, route);
            }

            IterateLevel(To);
        }
        [Test]
        public void RouteIsFoundFromEveryCell() {
       
            IntCoords end = new IntCoords(0, 0);

            void From(int x, int y) {

                IntCoords start = new IntCoords(x, y);

                Vector[] route = pf.FindPath(start, end);

                TestRoute(start, end, route);
            }

            IterateLevel(From);
        }
        [Test]
        public void RouteIsFoundToEveryCellFromEveryCell() {  

            void From(int x, int y) {

                IntCoords start = new IntCoords(x, y);

                void To(int x2, int y2) {

                    IntCoords end = new IntCoords(x2, y2);

                    Vector[] route = pf.FindPath(start, end);

                    TestRoute(start, end, route);
                }

                IterateLevel(To);
            }

            IterateLevel(From);
        }

        #region TestHelpers
        private void ClearLevel() {

            void InitLevel(int x, int y) {
                level[x, y] = 255;         
            }

            IterateLevel(InitLevel);
        }

        private void TestRoute(IntCoords start, IntCoords end, Vector[] route) {
            Assert.AreNotEqual(null, route, "Route was null");
            TestCoords(start, route[0], "Start coordinates were not equal");
            TestCoords(end, route[route.Length - 1], "Route to " + end + " was not found");
        }

        private void TestCoords(IntCoords coords, Vector pos, string message) {
            Assert.AreEqual(coords, Vector.PositionToCoords(pos), message);
        }

        private delegate void LevelIterateFunc(int x, int y);
        private void IterateLevel(LevelIterateFunc f) {
            for (int x = 0; x < 10; x++) {
                for (int y = 0; y < 10; y++) {
                    f(x, y);
                }
            }
        }
        #endregion
    }
}
