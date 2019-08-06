# Tank Game AI

[Main Page](https://github.com/porrasm/tiralabra-tank-game-ai)

[Documentation](https://github.com/porrasm/tiralabra-tank-game-ai/tree/master/Documentation/)

[Code Directory](https://github.com/porrasm/tiralabra-tank-game-ai/tree/master/Assets/_Assets/Scripts/Games/TankGame/TankAI/)

## Week 3 Report

### A* and DFS

This week I implemented A* for pathfinding. It took me a while to understand the algorithm but it should now be working properly. I have not yet tested it.

While the A* provides the fastest route, they are often quite boring and not something a player would necessarily do. That is why I will consider having the AI use both pathfinding algorithms (if it is not in a 'hurry'), simply because the DFS makes the AIs movement bit more unexpectable and interesting. 

I am also interested to see how much faster the A* implementation is. I have an idea of an impromevent for my DFS which could potentially make it's average search time faster than the A* while still providing the fastest or one of the fastest routes.

