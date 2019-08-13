# Tank Game AI

[Main Page](https://github.com/porrasm/tiralabra-tank-game-ai)

[Documentation](https://github.com/porrasm/tiralabra-tank-game-ai/tree/master/Documentation/)

[Code Directory](https://github.com/porrasm/tiralabra-tank-game-ai/tree/master/Assets/_Assets/Scripts/Games/TankGame/TankAI/)

## Week 4 Report

### Mo 12.8.2019

Today I split my level generator into 2 scripts (1 which generates the map & 1 which builds it in the game). This allows me to test the pathfinding algorithms.

I also created initial tests for A* and DFS.

### Tu 13.8.2019

#### Test coverage

I found a solution which allows me to generate test coverage results in HTML. Unity does not support test coverage by default so I had to use an external tool for that. 

I use DotCover for the test coverage reports but it can only show coverage on tests which do not rely on any Unity assemblies. At the moment getting coverage for classes relying on Unity is impossible. Luckily most of my classes do not reference any Unity assemblies.

[Test test coverage report](https://github.com/porrasm/tiralabra-tank-game-ai/blob/master/Documentation/weeks/coverage_example.html) (without any configuration)


#### Documentation

Added documentation and method descriptions to most classes.