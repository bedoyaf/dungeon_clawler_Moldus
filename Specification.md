# Specification of the project

The game will be a topdown perspective rogue lite dungeon crawler with proceduraly generated levels. 

This project is an extention of an existing game I created. The game will be in Unity, using C# and tools in the Unity game engine. The only external tool in used will be the Astar Pathfinding project for pathfinding https://arongranberg.com/astar/.

## The already existing game
The original game was actualy a game jam game, but it expanded into a Advanced C# project where I redid the whole game.
### Differences with the original
The game jam game and the Advanced C# project differed mainly, in the fact, that the game jam game has no procedural generation, it was a broken mess
### How the game looks like at the moment and how it will be extended

#### The game created so far
The game is a Dungeon Crawler where you need to search a procedurally generated level for the exit. The level is filled with enemies of three types, and you must defeat them using either your starting flamethrower or when you kill enaugh enemies of a certain type you can change you form to heal yourself and use the same attack as your enemy temporarily. This can be achieved by killing enough enemies of a certain type and then pressing one of the 1, 2, 3 keys that are associated with the enemy type.

## What will be added upon and what the new project consists of

### Upgrading old features and polishing
I am planning to add much more Dot tween functions, thats an animation engine for Unity, that I can use to add more impact to attacks or more visual clues to what is happing on screen, that the game lcaks and add some much needed polish to the experience. I also want to change certain enemies and their attacks such as the red mushrooms bomb placing. I also will change ty health system from a decreasing value of 100% to hearts that you lose after being hit, with a short time of immortality after being hit.

### Adding rogue lite elements
By that there will be a system that lets the player unlock progress even though they lose a session. The system will work by collecting upgrade points by either defeating bosses, or by finding it in chests. 

#### Skill tree
I will be adding a skill tree. It will open up by clicking a button by canvas. In it you can use your upgrade points to upgrade the player. When the player dies, all of the progress will be lost, but the skill tree will remain, so the character improves each game. Some of the skills will be simple stat increases, but others add some special abilities or change existing ones.
##### Ability 1
##### Ability 2
##### Ability 3
##### Ability 4
### Bosses
I want to add at least three boss types, after their defeat they would drop an upgrade point. Three because of the three enemy types, each being an expasion of the original.
#### First
#### Second
#### Third

#### Changing the procedural generation
Ofcourse it will be change in respect to things that are mentioned above, but also I would like to add pillars to rooms. That could get destroyed 