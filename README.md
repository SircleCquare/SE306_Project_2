# SE306_Project_2

## Building
This project is still in its prototype phase. At this point, we suggest building it as a standalone application and running the produced output. The PROTOTYPE release contains a compiled version of the Windows standalone. If you use this, you will not not need to build the project yourself. 

To build this as a standalone application for yourself...
- Open the project in Unity 
- Select File > Build Settings ... (or use the shortcut 'Ctrl + Shift + B')
- For the protoype, make sure the following 5 scenes are in the build: 
   - Start
   - Level Select 
   - Michael's tutorial level
   - level
   - Finish Scene
- Select the PC, Mac & Linux Standalone option from the list of platforms
- Choose the 'Target Platform' to build to (e.g. 'Windows') 
- Press 'Build' and choose where to store the output. 

## Running 
Either
- Download the binary supplied with the PROTOTYPE release, unzip the file, and run prototype.exe from within it

Or
- Navigate to the build output you generate and run that application

## Controls
- Use the 'left' and 'right' arrow keys (or 'A' and 'D') to move left or right
- Use 'Space' to jump
- Use 'F' to flip worlds
- Use 'E' to interact with some objects in the world
- Press 'Esc' to pause/unpause the game

## Goal & Score
- Complete evvery level to finish the game
- Try to achieve higher scores in levels
- Collect coins to increase your score
- Dying will also impact your score in future versions
- Finishing the stage with full health or in shorter times will also impact your score in later versions

## Enemies
- Currently there is one enemy in the game.
- The enemy will chase you in a short radius.
- Making contact with the enemy will reduce your health, and may push you off platforms and into hazards.
