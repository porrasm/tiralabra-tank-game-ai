# Tank Game AI

[Main Page](https://github.com/porrasm/tiralabra-tank-game-ai)

[Documentation](https://github.com/porrasm/tiralabra-tank-game-ai/tree/master/Documentation/)

[Code Directory](https://github.com/porrasm/tiralabra-tank-game-ai/tree/master/Assets/_Assets/Scripts/Games/TankGame/TankAI/)

## Requirements Specification

The goal of this project is that the AI is able to play the game on different levels of difficulty. The hardest difficulty level should be difficult enough to make you lose against it most of the time.

### Problems to solve

The most obvious problem is that the AI will have to be able to do pathfinding in a maze. The level will be randomly generated every round and the AI should not havbe any problems navigating the maze. The AI will also need to predict some player movements (e.g. when a player will try to attack/retreat). 

The AIs movement has to be fluid in the maze (e.g. it should not only travel to the center of a maze cell). It needs to be able to use corners to bounce shots and be able to hide behind walls and/or dodge bullets to avoid losing health.

The AI needs to be able to calculate where a bullet will go. This will require vector math (reflection etc.).

#### Algorithms for said problems

At this stage it's quite difficult to say which algorithms to use, apart from the pathfinding.

##### Pathfinding

The 2 likely candidates are Dijkstras Algorithm or A*. The map is a simple 10x10 maze so the pathfinding algorithm will be very fast. However it will be used very often (likely multiple times every frame).

Both of there algorithms require a graph and vertices.

##### Prediction

No idea.

##### Shooting

The bullets reflect of the walls in the same way as light would. So the AI has to be able to fire a ray and reflect it and see it it hits a player, or if it hits a players future movement trajectory.


#### Vectors

The AI and the tank game itself relies heavily on vectors so a vector structure and multiple vector math function need to be implemented.