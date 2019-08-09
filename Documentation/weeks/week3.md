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


### Th 8.8.2019

Today I worked on the tank AIs movement. The AIs can now follow a path of (x, y) integer coordinates. Adding support for float coordinates will be useful in the future. The AIs also have a simple stuck prevention. 


### Fr 9.8.2019

Today I've mostly worked on predicting bullet movements and having the AI dodge an incoming bullet. The script should work now (in theory) but I have a slight problem with predicting the correct bullet trajectory. 

The bouncing off a wall is slightly different for the physics object (bullet) and the raytracing so the AI can't perfectly predict a bullets trajectory. This is only a slight problem since the AI updates the trajectory after every collision so the miscalculations are corrected.

A more major issue right now is that when a ray hits the tank, it stops and the bullets last known predicted location is the AIs position. However when the AI moves the bullets trajectory changes since the tank isn't blocking it anymore. This is a problem because the AI start dodging the bullet at this point and has no idea that the bullet can pass through it's 'old' position. I have not yet figured out a fix for this problem.

All in all the dodging script is very very buggy.

### Testing release

I have made a release if you wish to test out the game (Windows required). Press R to have the AI come to your position, click to shoot a bullet.

[Releases](https://github.com/porrasm/tiralabra-tank-game-ai/releases)