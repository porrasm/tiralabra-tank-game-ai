# Tank Game AI

[Main Page](https://github.com/porrasm/tiralabra-tank-game-ai)

[Documentation](https://github.com/porrasm/tiralabra-tank-game-ai/tree/master/Documentation/)

[Code Directory](https://github.com/porrasm/tiralabra-tank-game-ai/tree/master/Assets/_Assets/Scripts/Games/TankGame/TankAI/)

## Week 4 Report

### Mo 12.8.2019

Today I split my level generator into 2 scripts (1 which generates the map & 1 which builds it in the game). This allows me to test the pathfinding algorithms.

I also created initial tests for A* and DFS but ran into serious problem. The A* gives different results when run from a test environment.

When testing the algorithm manually (in an empty level with no walls) the algorithm gives the straightest and therefore shortest route to the goal. The route from coordinates (0, 0) to (0, 9) is a straight line. However when testing the algorithm on an empty level it won't give a straight path. The path from (0, 0) to (0, 9) is not straight and is different every time I run the test.

### Tu 13.8.2019

#### Test coverage

I found a solution which allows me to generate test coverage results in HTML. Unity does not support test coverage by default so I had to use an external tool for that. 

I use DotCover for the test coverage reports but it can only show coverage on tests which do not rely on any Unity assemblies. At the moment getting coverage for classes relying on Unity is impossible. Luckily most of my classes do not reference any Unity assemblies.

[Test test coverage report](https://github.com/porrasm/tiralabra-tank-game-ai/blob/master/Documentation/weeks/coverage_example.html) (without any configuration)


#### Documentation

Added documentation and method descriptions to most classes.