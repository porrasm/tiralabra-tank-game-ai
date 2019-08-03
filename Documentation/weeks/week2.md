# Tank Game AI

[Main Page](https://github.com/porrasm/tiralabra-tank-game-ai)

[Documentation](https://github.com/porrasm/tiralabra-tank-game-ai/tree/master/Documentation/)

[Code Directory](https://github.com/porrasm/tiralabra-tank-game-ai/tree/master/Assets/_Assets/Scripts/Games/TankGame/TankAI/)

## Week 2 Report

[Hours](https://github.com/porrasm/tiralabra-tank-game-ai/tree/master/Documentation/hours.md)

### Initialization

The idea in the 2nd week was to get an AI with pathfinding and simple shooting skills up and running. However I ran into several problems with the game and initializing the testing environment. 

### Problems

Normally the game would require a host instance and then other clients would connect to the game. In order to easily test the AI everything should run on the same application instance. Implementing this took more time than expected but it is now working properly.

### Level

I modified my [TankLevelGenerator](https://github.com/porrasm/tiralabra-tank-game-ai/blob/master/Assets/_Assets/Scripts/Games/TankGame/TankLevelGenerator.cs#L319) script to save the level as a 2 dimensional byte array.

The level is built from cells which have an upper & right wall. From each cell there are 8 possible directions to go. Naturally these directions are up, right, down, left, up-right, down-right, down-left and up-left.

Each bit in the array value corresponds to a possible direction. First bit is for up, second for left, 3rd for down and so on.

Below is an example image which shows which bit value corresponds to which direction. In the example you can move to every direction from the center cell, which means that the byte value for that cell is 1111 1111.

<img src="https://github.com/porrasm/tiralabra-tank-game-ai/blob/master/Documentation/weeks/level_cells_demonstration.png" width="300" height="300" />

### Pathfinding

Since the shortest possible route is not very important in this case I decided to start this with a simple depth first search modification. In a normal maze this would not work but the levels in this game are very open.

The DFS modification always chooses the closest cell next.

After the initial route is found, it re-searches from every node to the closest other node to see if the route can be shortened.

The implementation is not yet ready.


### Testign

This week I started writing tests on IntCoords, Maths and Vector classes. The test directory can be found [here](https://github.com/porrasm/tiralabra-tank-game-ai/tree/master/Assets/_Assets/Tests/)