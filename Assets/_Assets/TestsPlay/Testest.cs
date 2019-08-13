using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Tests {

    public class Testest {

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

        [UnityTest]
        public IEnumerator RouteToEndIsStraight() {

            SceneManager.LoadScene("TankPathTesting");

            yield return new WaitForSeconds(2);

            ClearLevel();

            TankPathVisualizer vi = GameObject.FindObjectOfType<TankPathVisualizer>();
            

            IntCoords start = new IntCoords();

            // Right
            IntCoords end = new IntCoords(9, 0);

            Vector[] route = pf.FindPath(start, end);

            vi.level = level;
            TankPathVisualizer.DrawRoute(route);
            
            TestRoute(start, end, route);

            for (int i = 0; i < 10; i++) {
                TestCoords(new IntCoords(i, 0), route[i], "Route to right was not straight");
            }

            yield return new WaitForSeconds(2000);

            // Up
            end = new IntCoords(0, 9);

            route = pf.FindPath(start, end);

            vi.level = level;
            TankPathVisualizer.DrawRoute(route);

            yield return new WaitForSeconds(2);
            TestRoute(start, end, route);

            for (int i = 0; i < 10; i++) {
                TestCoords(new IntCoords(0, i), route[i], "Route to up was not straight");
            }

            // Up-Right
            end = new IntCoords(9, 9);

            route = pf.FindPath(start, end);

            vi.level = level;
            TankPathVisualizer.DrawRoute(route);

            yield return new WaitForSeconds(2);
            TestRoute(start, end, route);

            for (int i = 0; i < 10; i++) {
                TestCoords(new IntCoords(i, i), route[i], "Route to up-right was not straight");
            }


        }


        #region TestHelpers
        private void ClearLevel() {

            List<TankLevelGenerator.Step> steps = new List<TankLevelGenerator.Step>();

            void InitLevel(int x, int y) {
                TankLevelGenerator.Step step = new TankLevelGenerator.Step();
                step.Coords = new IntCoords(x, y);
                step.Wall = TankCell.CellWall.Both;
                steps.Add(step);
            }

            IterateLevel(InitLevel);

            Debug.Log("Created test level: " + steps.Count);


            generator.LevelFromSteps(out level, steps);

        }

        private void TestRoute(IntCoords start, IntCoords end, Vector[] route) {
            if (route.Length == 0) {
                Debug.Log("ROUTE WAS 0");
                return;
            }
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
