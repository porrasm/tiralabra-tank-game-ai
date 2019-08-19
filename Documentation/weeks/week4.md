# Tank Game AI

[Main Page](https://github.com/porrasm/tiralabra-tank-game-ai)

[Documentation](https://github.com/porrasm/tiralabra-tank-game-ai/tree/master/Documentation/)

[Code Directory](https://github.com/porrasm/tiralabra-tank-game-ai/tree/master/Assets/_Assets/Scripts/Games/TankGame/TankAI/)

## Week 4 Report

### Release

[Week 4 Release](https://github.com/porrasm/tiralabra-tank-game-ai/releases/tag/week4)

### Mo 12.8.2019

Today I split my level generator into 2 scripts (1 which generates the map & 1 which builds it in the game). This allows me to test the pathfinding algorithms.

I also created initial tests for A* and DFS.

#### Test coverage

I found a solution which allows me to generate test coverage results in HTML. Unity does not support test coverage by default so I had to use an external tool for that. 

I use DotCover for the test coverage reports but it can only show coverage on tests which do not rely on any Unity assemblies. At the moment getting coverage for classes relying on Unity is impossible. Luckily most of my classes do not reference any Unity assemblies.

### Tu 13.8.2019

### Th 15.8.2019

I spent most of today fighting with VisualStudio, which suddenly stopped working.

### Fr 16.8.2019

I found & fixed a huge bug in the TankBulletTrajectory script. The function which was used to calculate which cells the bullet will pass over was seriously flawed. I had to resort to using Unitys Vector3.MoveTowards which I have not yet had time to implement. After fixing this, bug the AI is now able to dodge bullets. It is still quite buggy since the AI can't reverse so dodging a bullet might require a 180 turn at best, which almost guarantees a hit with the bullet anywa.

The rest of today I spent implementing the AIs behaviour. It can shoot, move to a target and dodge bullets but it will stay inactive at other times. 

I implemented a class which uses a prioritized list to sort jobs. The job with the highest priority will be executed. Different jobs are to target a player (follow & actively hunt), evade a player (Actively evade other players) and evade bullets (actively evade bullets). Right now only the 1st is (partially) implemented. If I have time to implement all 3 of these the AI should already be fun to play against. 

Today (and this week) I realised that this project may be too large for this course. I'm having problems managing all the different components this AI needs and the lack of time means that often I had to use the first solution that I could think of. Now I have come up with better solutions for every implemented feature but there is not enough time to fix them.

The biggest problem I have might be the testing. I have no idea how to test my AI (except unit tests) and it seems that integration testing will be extremely time consuming.

#### Testing

In order to view the test report, you need to clone the repository. The coverage is at 76% but in reality it's lower. I've excluded all classes which reference Unitys assemblies since these assemblies can't be run outside of Unity and Unity does not support code coverage reports.

At the moment I don't know how I should start testing these classes since most of the require the game to be running. This would require the use of 'Play mode tests' which run in the background of the game and I currently don't have an idea how these work.

[Test coverage report](https://porrasm.github.io/tiralabra-tank-game-ai/)
#### Documentation

Added documentation and method descriptions to most classes.
