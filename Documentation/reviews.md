# Tank Game AI

[Main Page](https://github.com/porrasm/tiralabra-tank-game-ai)

[Documentation](https://github.com/porrasm/tiralabra-tank-game-ai/tree/master/Documentation/)

[Code Directory](https://github.com/porrasm/tiralabra-tank-game-ai/tree/master/Assets/_Assets/Scripts/Games/TankGame/TankAI/)

## Reviews

[Source code Directory](https://github.com/porrasm/tiralabra-tank-game-ai/tree/master/Assets/_Assets/Scripts/Games/TankGame/TankAI/)

[Tests can be found here](https://github.com/porrasm/tiralabra-tank-game-ai/tree/master/Assets/_Assets/Tests)

### Current build

If you have Windows, you can head over to [releases](https://github.com/porrasm/tiralabra-tank-game-ai/releases) to test the project.

### Testing and running from the editor

If you wish to run tests or run the project, you need to download Unity at https://unity.com/

Unity is available for MAC and Windows. Installing Unity just for the peer review might be too overkill unless you are interested in Unity or game development anyway.

Unity is free for personal commercial use.


### Running the project

If you decided to install Unity here are instructions on how to run the project or tests.

#### Opening & running the project

- Open the project in Unity. The easiest way to do this is open the scene file: Assets/_Assets/Scenes/TankAITesting/TankAITestingMenu.unity

- To test the project click the play icon on the top of the scene window


##### Testing pathfinding

- Open the pathfinding scene (from within Unity) at: _Assets/Scenes/TankAITesting/TankPathTesting.unity

- Click the play icon on the top of the scene window

Press R to generate a new maze. You can change the maze size in the TankSettings.cs script located in _Assets/Scripts/Games/TankGame/TankSettings.cs

After generating a new maze press T to initialize the pathfinding.

After this you can click anywhere in the level to find a path to the location.

Left click for A* pathfinding and right click for DFS pathfinding.

The path will be drawn from the bottom left to the mouse position. To update the start position, press SPACE.

#### Running the tests

- In Unity toolbar click Window -> General -> Test Runner

- In the test runner window, click EditMode (should be cliked by default)

- Click Run All in the top left corner or choose which tests you want to run

- Right click on a test to view source code