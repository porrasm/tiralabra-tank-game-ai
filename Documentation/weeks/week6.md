# Tank Game AI

[Main Page](https://github.com/porrasm/tiralabra-tank-game-ai)

[Documentation](https://github.com/porrasm/tiralabra-tank-game-ai/tree/master/Documentation/)

[Code Directory](https://github.com/porrasm/tiralabra-tank-game-ai/tree/master/Assets/_Assets/Scripts/Games/TankGame/TankAI/)

## Week 6 Report

### Tu 27.8.2019

Added the initial testing document.

Improved the AIs shooting capabilities by allowing it to stop and find an angle to shoot. This however broke the movement of the AI and I had to revert back to the latest commit.

### Fr 30.8.2019

I implemented an array based stack CStack and wrote unit tests for it.

Replaced the HashSet in TankAStar with CoordsContainer. It has the same functionality as HashSet but is faster, but takes more space. Added tests for it as well.
