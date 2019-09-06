# Tank Game AI

[Main Page](https://github.com/porrasm/tiralabra-tank-game-ai)

[Documentation](https://github.com/porrasm/tiralabra-tank-game-ai/tree/master/Documentation/)

[Code Directory](https://github.com/porrasm/tiralabra-tank-game-ai/tree/master/Assets/_Assets/Scripts/Games/TankGame/TankAI/)

## Manual

The executable file of this project can be found in [releases](https://github.com/porrasm/tiralabra-tank-game-ai/releases)

### Starting the game

- Download the latest release, unpack it and run the file TankAIBot.exe
- Press Start game and select how many AIs you want to play against
- Start the game

You will play as the Red tank. Use WASD to move and mouse click to shoot. Press left shift to enable debugging information. Press ESC to to exit the application.

### Unity instructions

If you wish to run tests or run the project using Untiy, you need to download it at https://unity.com/

Unity is available for MAC, Windows and Linux (Google for instructions).

Unity is free for personal commercial use.

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

Running all tests will take 5+ minutes because of the heavy performance testing. If you skip the perfomance test the tests will take around 1 minute.