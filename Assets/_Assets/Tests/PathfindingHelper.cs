using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests {
    static class PathfindingHelper {

        private static TankMazeGenerator generator;

        static PathfindingHelper() {
            generator = new TankMazeGenerator();
        }

        #region TestHelpers
        public static void ClearLevel(out byte[,] level) {

            List<TankLevelGenerator.Step> steps = new List<TankLevelGenerator.Step>();

            void InitLevel(int x, int y) {
                TankLevelGenerator.Step step = new TankLevelGenerator.Step();
                step.Coords = new IntCoords(x, y);
                step.Wall = TankCell.CellWall.Both;
                steps.Add(step);
            }

            IterateLevel(InitLevel);

            generator.LevelFromSteps(out level, steps);
        }

        public static void TestRoute(IntCoords start, IntCoords end, Vector[] route) {
            Assert.AreNotEqual(null, route, "Route was null, start: " + start + ", end: " + end);
            Assert.AreNotEqual(0, route.Length, "Route was empty, start: " + start + ", end: " + end);
            TestCoords(start, route[0], "Start coordinates were not equal, start: " + start + ", end: " + end + ", first: " + route[0]);
            TestCoords(end, route[route.Length - 1], "Route to end was not found, start: " + start + ", end: " + end + ", first: " + route[0]);
        }

        public static void TestCoords(IntCoords coords, Vector pos, string message) {
            Assert.AreEqual(coords, Vector.PositionToCoords(pos), message);
        }

        public delegate void LevelIterateFunc(int x, int y);
        public static void IterateLevel(LevelIterateFunc f) {
            for (int x = 0; x < 10; x++) {
                for (int y = 0; y < 10; y++) {
                    f(x, y);
                }
            }
        }
        #endregion
    }
}
