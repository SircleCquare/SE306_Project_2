# Flip Side, by dynam.io (SE306 Project 2) 
Note: The game was renamed from 'Ups and Downs' to 'Flip Side'. There may still be legacy references to this working title, including in file names. 

## Team member Github IDs
- SircleCquare - Asheer Ahmad
- MisterAndersen - Ben Andersen
- hchokshi - Harsh Chokshi
- PastaTime - Arran Davis
- T77GIT - Tim Hughes
- michaelieti - Michael Ieti
- EmandM - Emma McMillan
- zhsitao - Sitao Zheng

## Building
We suggest building the project as a standalone application and running the produced output. The FINAL release contains a compiled version of the Windows standalone. If you use this, you will not not need to build the project yourself. 

To build this as a standalone application for yourself...
- Open the project in Unity 
- Select File > Build Settings ... (or use the shortcut 'Ctrl + Shift + B')
- For the final build, make sure the following 7 scenes are in the build: 
   - Start
   - Level Select 
   - Tutorial
   - Level 1
   - Level 2
   - Finish Scene
   - Game Over Scene
- Select the PC, Mac & Linux Standalone option from the list of platforms
- Choose the 'Target Platform' to build to (e.g. 'Windows') 
- Press 'Build' and choose where to store the output

## Running 
Either
- Download the binary supplied with the FINAL release, unzip the file, and run final.exe from within it

Or
- Navigate to the build output you generate and run that application

## Controls
- Use the 'left' and 'right' arrow keys (or 'A' and 'D') to move left or right
- Use 'Space' to jump
- Use 'F' to flip worlds
- Use 'E' to interact with some objects in the world
- Press 'Esc' to pause/unpause the game

## Goal & Score
- Complete every level to finish the game
- Try to achieve higher scores in levels
- Collect coins to increase your score
- Dying will negatively impact your score
- Finishing the stage in shorter times will positively impact your score

## Enemies
- Currently there are two enemies in the game.
- The sphere enemy will chase you in a short radius.
- Making contact with the sphere enemy will knock you back, and may push you off platforms and into hazards.
- The leech enemy will latch onto you if you approach it
- After latching onto you, the leech will cause dizzyness and loss of balance
