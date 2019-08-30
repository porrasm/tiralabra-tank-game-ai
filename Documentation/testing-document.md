# Tank Game AI

[Main Page](https://github.com/porrasm/tiralabra-tank-game-ai)

[Documentation](https://github.com/porrasm/tiralabra-tank-game-ai/tree/master/Documentation/)

[Code Directory](https://github.com/porrasm/tiralabra-tank-game-ai/tree/master/Assets/_Assets/Scripts/Games/TankGame/TankAI/)

# Testing document

The automated tests cover all of the classes which do not depend on any Unity assemblies. I am working on testing the other classes but can't provide any coverage reports on those classes.


## Unit testing

I have detailed unit tests for all of the data structures and classes which do not require any Unity functionality.

[Test coverage report](https://porrasm.github.io/tiralabra-tank-game-ai/)

The directory for the unit tests can be found here:
[Tests Directory](https://github.com/porrasm/tiralabra-tank-game-ai/tree/master/Assets/_Assets/Tests/)

## Manual testing

Because testing game related functions is very hard, I've had to resort to some manual testing. For example to test out the bullet trajectories I've made a script which visualizes the future trajectory of the bullet. The numbers in each cell visualize the TankAIBulletChecker.CellBulletCounts array which shows how many times a bullet will pass over a certain cell. 

![Bullet trajectory](https://github.com/porrasm/tiralabra-tank-game-ai/blob/master/Documentation/resources/bullet-trajectory.png)

