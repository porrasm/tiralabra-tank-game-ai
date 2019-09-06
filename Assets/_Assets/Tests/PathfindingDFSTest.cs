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

            Init();
        }
        private void Init() {
            pf = new TankDFSPath(level);
        }

        [Test]
        public void RouteToEndIsStraight() {

            PathfindingHelper.ClearLevel(out level);
            Init();

            IntCoords start = new IntCoords();

            // Right
            IntCoords end = new IntCoords(9, 0);

            Vector[] route = pf.FindPath(start, end);

            PathfindingHelper.TestRoute(start, end, route);

            for (int i = 0; i < 10; i++) {
                PathfindingHelper.TestCoords(new IntCoords(i, 0), route[i], "Route to right was not straight");
            }

            // Up
            end = new IntCoords(0, 9);

            route = pf.FindPath(start, end);

            PathfindingHelper.TestRoute(start, end, route);

            for (int i = 0; i < 10; i++) {
                PathfindingHelper.TestCoords(new IntCoords(0, i), route[i], "Route to up was not straight");
            }

            // Up-Right
            end = new IntCoords(9, 9);

            route = pf.FindPath(start, end);

            PathfindingHelper.TestRoute(start, end, route);

            for (int i = 0; i < 10; i++) {
                PathfindingHelper.TestCoords(new IntCoords(i, i), route[i], "Route to up-right was not straight");
            }
        }

        [Test]
        public void RouteIsFoundToEveryCell() {

            IntCoords start = new IntCoords();

            void To(int x, int y) {

                IntCoords end = new IntCoords(x, y);

                Vector[] route = pf.FindPath(start, end);

                PathfindingHelper.TestRoute(start, end, route);
            }

            PathfindingHelper.IterateLevel(To);
        }
        [Test]
        public void RouteIsFoundFromEveryCell() {
       
            IntCoords end = new IntCoords(0, 0);

            void From(int x, int y) {

                IntCoords start = new IntCoords(x, y);

                Vector[] route = pf.FindPath(start, end);

                PathfindingHelper.TestRoute(start, end, route);
            }

            PathfindingHelper.IterateLevel(From);
        }
        [Test]
        public void RouteIsFoundToEveryCellFromEveryCell() {

            void From(int x, int y) {

                IntCoords start = new IntCoords(x, y);

                void To(int x2, int y2) {

                    IntCoords end = new IntCoords(x2, y2);

                    Vector[] route = pf.FindPath(start, end);

                    PathfindingHelper.TestRoute(start, end, route);
                }

                PathfindingHelper.IterateLevel(To);
            }

            PathfindingHelper.IterateLevel(From);
        }
    }
}
