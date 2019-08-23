# Tank Game AI

[Main Page](https://github.com/porrasm/tiralabra-tank-game-ai)

[Documentation](https://github.com/porrasm/tiralabra-tank-game-ai/tree/master/Documentation/)

[Code Directory](https://github.com/porrasm/tiralabra-tank-game-ai/tree/master/Assets/_Assets/Scripts/Games/TankGame/TankAI/)

# Design Document

The project is an Tank AI/bot based on my practice Tank Game project. The goal was to get the AI to a lever where it could challenge human players and provide a good playing experience.

## Structure

The project structure is quite simple

![Project structure](https://github.com/porrasm/tiralabra-tank-game-ai/blob/master/Documentation/resources/project_structure.png)

On the top of the hierarchy we have the game itself. Below that we have the Tank player control classes which the AI can use to control the tank and gather information about player like health.

Under that we have the TankAI class itself which handles everything the AI does. The AI class has multiple AI component classes which implement a certain function, like bullet dodging, shooting or deciding what the tank should do at the moment. 

The TankAI class acts as a bridge between the game functionality and the AI. Most things in the AI are separated completely from the main game and it has very few dependencies to the actual game aside from the control classes.

The only game funcionality that is dependent on the AI is the TankAIManager, which enables/disables the AIs based on the game state.

## Algorithms and time complexities

### Pathfinding

#### A*

The implemented A* is a traditional A* which uses a simple 'straight line distance' as the heuristic function.

The worst case time complexity of A* is O(|E|) = O(b<sup>d</sup>) where d is the depth of the shortest path and b is the branching factor. The optimal best case time complexity for A* is O(n) where n is the shortest path length.

The above is for an unbounded space, however my space is bounded so the worst case scenario of the algorithms is O(|V|), where |V| is the vertex count (level width * level height). Since the amount of edges is constant, it will not affect the time complexity. The algorithm will 'visit' each node max once. The best case remains the same.

In my case it's statistically impossible for the absolute worst case scenario to happen because of how to level is generated. Calculating the true average time complexity is very difficult since it depends on the heuristic function and on the level generator settings (specifically TankSettings.CleanProbability which is the chance of walls to be removed). 

I ran some tests to approximate the average time consumption of the algorithm. The algorithm searched 1 route from a random start position to a random end position on 1 million levels. The results are listed below:

The parameters for this test:

| Parameter     | Value          
| ------------- |:-------------|
| Level width     | 10 | 
| Level height     | 10      | 
| TankSettings.CleanProbability | 0.8     |
| Searched route count | 1 route per 1000000 levels  |


Results

| Result     | Value          
| ------------- |:-------------|
| Processed nodes     | 50453198 | 
| Length of the optimal routes     | 8745752      | 
| Ratio | 5.76888048048927    |
| Total processing time | 316.186s  |

From these results the approximate time required for the algorithm is around 6 times the optimal O(n) time complexity. This is by no means a scientific method to solve this problem, but just a calculated estimation.

#### DFS

The implemented DFS is a heuristic DFS, which always processes the best (closest to the goal) possible cell (vertex) next.

The worst case time complexity of a traditional DFS is O(|V| + |E|) where |V| is the vertex count and |E| is the edge count and the best case time complexity is O(n) where n is shortest path length. 

In my case the worst case time complexity will be O(|V|) since the edge count is constant. The best case remains the same.

Again, because of the same problem as with the A*, the average time complexity of the DFS algorithm is hard to calculate. I ran the same test (with same parameters and levels) for DFS. The results are listed below:

The parameters for this test:

| Parameter     | Value          
| ------------- |:-------------|
| Level width     | 10 | 
| Level height     | 10      | 
| TankSettings.CleanProbability | 0.8     |
| Searched route count | 1 route per 1000000 levels  |

Results

| Result     | Value          
| ------------- |:-------------|
| Processed nodes     | 27079901 | 
| Length of the optimal routes     | 12025690      | 
| Ratio | 3.09634906180738    |
| Total processing time | 316.186s  |

From these results the approximate time required for the algorithm is around 3 times the optimal O(n) time complexity. Again, this is just an approximation.

#### A* vs DFS

Comparion table:

| Result        | A*            | DFS   |
| ------------- |:-------------| -----|
| Processed nodes     | 50453198| 27079901 |
| Path lengths     | 8745752      |   12025690 |
| AVG Processed nodes     | 50.453198| 27.079901 |
| AVG path lengths     | 8.745752      |   12.02569 |


AStar processed more nodes than DFS, on average 1.8631234287 times more nodes than DFS.

DFS paths lengths were longer than the optimal lengths by A*, on average DFS routes were 1.37503212988 times longer.

So the heuristic DFS is faster, but gives longer routes.

#### Why the edge count is constant?

The edge count is varies from 0-8 to but is still technically a constant because all the 8 edges have to be checked anyway.

Code from the A* algorithm:
```
byte allowed = level[n.Coords.x, n.Coords.y];
for (int i = 0; i < 8; i++) {

    Node neighbour = GetChild(n, allowed, i);

    if (neighbour == null) {
        continue;
    }

    ...
}
```

i is the bit index, which also acts as a direction.

Each edge is processed and the GetChild method checks if the allowed byte had bit index i set to 1.


## Algorithms and space complexities

### Pathfinding

For both A* and DFS the worst case space complecity is O(|V|). A* will have max |V| count of active and closed nodes and the recursive DFSs maximum depth will be |V|.

## Problems

In my opinion the project turned out to be too large for this course. There were and still are multiple problems that require solving. The main problem was that the project consists of multiple components and managing these components and creating a good interface for them was a problem.

I did not have time to think everything through and I sometimes had to just take the first solution that came to mind. While the overarching project structure is good, it was not implemented in the best possible way to meet the requirements of good software engineering. The project is not very testable, maintainable or extensible at the moment and this is the biggest flaw of the project in my opinion. To fix this, I would need a lot of time to refactor many parts of the project.

Other minor problems include that not all the requirements from the requirement specifications have been implemented. For example the AI does not have a difficulty slider. The AIs 'skill' could also be higher, but I have time to improve this in the last few weeks.

## Improvements

## Sources

A* search algorithm - https://en.wikipedia.org/wiki/A*_search_algorithm
Depth-first search - https://en.wikipedia.org/wiki/Depth-first_search