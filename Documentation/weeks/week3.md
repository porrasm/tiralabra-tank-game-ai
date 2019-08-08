# Tank Game AI

[Main Page](https://github.com/porrasm/tiralabra-tank-game-ai)

[Documentation](https://github.com/porrasm/tiralabra-tank-game-ai/tree/master/Documentation/)

[Code Directory](https://github.com/porrasm/tiralabra-tank-game-ai/tree/master/Assets/_Assets/Scripts/Games/TankGame/TankAI/)

## Week 3 Report

### Tu 6.8.2019

#### A* and DFS

This week I implemented A* for pathfinding. It took me a while to understand the algorithm but it should now be working properly. I have not yet tested it.

While the A* provides the fastest route, they are often quite boring and not something a player would necessarily do. That is why I will consider having the AI use both pathfinding algorithms (if it is not in a 'hurry'), simply because the DFS makes the AIs movement bit more unexpectable and interesting. 

I am also interested to see how much faster the A* implementation is. I have an idea of an impromevent for my DFS which could potentially make it's average search time faster than the A* while still providing the fastest or one of the fastest routes.

##### Pure DFS vs pure A*

Without the aforementioned DFS modifications, I had both of them calculate a route to every cell in the level and these were the results.

DFS: 7-11 ms
A*: 30-34 ms

So the DFS is considerably faster. This could be the result of poor implementation of A* or DFS and I will definitely look into it.

When the map size was increased A* became increasingly slow while DFS. At 50x50 level, DFS was more than 35 times faster than A*.