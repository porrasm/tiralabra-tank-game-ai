using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests {

    public class PerformanceTest {

        private byte[,] level;
        private TankMazeGenerator generator;
        private TankDFSPath dfs;
        private TankAStarPath astar;

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
            dfs = new TankDFSPath(level);
            astar = new TankAStarPath(level);
        }

        [Test]
        public void PerformanceCalculations() {

            return;

            long closedDFS = 0;
            long routesDFS = 0;

            long closedAStar = 0;
            long routesAStar = 0;

            for (int i = 0; i < 1000000; i++) {

                IntCoords start = new IntCoords((int)(RNG.Float * 10), (int)(RNG.Float * 10));
                IntCoords end = new IntCoords((int)(RNG.Float * 10), (int)(RNG.Float * 10));

                Vector[] routeD = dfs.FindPath(start, end);
                Vector[] routeA = astar.FindPath(start, end);

                closedDFS += dfs.ProcessedCount;
                routesDFS += routeD.Length;

                closedAStar += astar.ProcessedCount;
                routesAStar += routeA.Length;

                //void To(int x, int y) {

                //    IntCoords end = new IntCoords(x, y);

                //    Vector[] route = pf.FindPath(start, end);

                //    closed += ((TankAStarPath)pf).closedCount;
                //    routes += route.Length;

                //    PathfindingHelper.TestRoute(start, end, route);
                //}

                //PathfindingHelper.IterateLevel(To);
            }

            double ratioD = (1.0 * closedDFS) / (1.0 * routesAStar);
            double ratioA = (1.0 * closedAStar) / (1.0 * routesAStar);

            MonoBehaviour.print("Closed DFS: " + closedDFS);
            MonoBehaviour.print("Route lengths DFS: " + routesDFS);
            MonoBehaviour.print("Ratio DFS: " + ratioD);
            MonoBehaviour.print("--------------------------------------");
            MonoBehaviour.print("Closed AStar: " + closedAStar);
            MonoBehaviour.print("Route lengths AStar: " + routesAStar);
            MonoBehaviour.print("Ratio AStar: " + ratioA);
        }
    }
}
