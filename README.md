## Unity Test 2
This is Unity project made as a skill test:

* Top view 2D game
* Generating grid
* Implemented pathfinding A* algoritm
* Colorization of selected objects in the scene

### Dependencies
Unity Version: 2019.4.10f1, whole project folder is uploaded, all you need to run this project is proper version of Unity

### Controls
After selecting start point and end point player can see the process of finding closest path between these two points 

#### Editor controls (presets)
* Grid size - x & y values can be changed in the "Terrain_Main" script attached to the "GameManager" object

#### Ingame controls
* Left mouse button click - selects point in the grid and sets it as "EndPoint" (If EndPoint already existed, set it as new StartPoint before storing new selection)
* Right mouse button click - selects point in the grid and destroys it (it becomes obstacle)
* Space - provokes next pathfinding step
