using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests {

    public class PathfindingAStarTest {

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
            pf = new TankAStarPath(level);
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

            foreach (Vector v in route) {
                IntCoords c = Vector.PositionToCoords(v);
            }

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
        
        [Test]
        public void TheFastestRouteIsFound() {

            byte[,] level = TestLevel();

            pf = new TankAStarPath(level);

            IntCoords from = new IntCoords();
            IntCoords to = new IntCoords(0, 1);

            Vector[] route = pf.FindPath(from, to);

            Assert.AreEqual(2, route.Length);

            to = new IntCoords(3, 2);

            route = pf.FindPath(from, to);

            Assert.AreEqual(5, route.Length);
            Assert.AreEqual(Vector.CoordsToPosition(new IntCoords(2, 1)), route[3]);
        }

        private byte[,] TestLevel() {

            byte[,] level = new byte[5, 5];

            level[0, 0] = Allowed(TankDirection.Right, TankDirection.Up);
            level[1, 0] = Allowed(TankDirection.Right, TankDirection.Up);
            level[2, 0] = Allowed(TankDirection.Up);
            level[1, 1] = Allowed(TankDirection.Left);
            level[2, 1] = Allowed(TankDirection.Left, TankDirection.Up, TankDirection.UpRight);

            level[0, 2] = Allowed(TankDirection.Right);
            level[1, 2] = Allowed(TankDirection.Right);
            level[2, 2] = Allowed(TankDirection.Right);

            return level;
        }

        private byte Allowed(params TankDirection[] directions) {

            byte allowed = 0;

            foreach (TankDirection d in directions) {
                TankDirectionTools.SetDirectionBit(ref allowed, d);
            }

            return allowed;
        }
    }
}
