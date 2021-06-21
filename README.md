<h1>Agnopsycho w a v e</h1>

<h2>About the project</h2>
Agnopsycho w a v e is a little game made in Unity during the 48-hour Global Game Jam in 2017 as a result of interdisciplinary collaboration efforts.

The goal of the game is to follow the correct paths that lead to houses. Taking the wrong path resets player back to the start (and so does completing the game :D )

While this is an easy task for a healthy human being, a person with Agnosia, condition where one cannot recognise objects, shapes and so on may struggle. Therefore, this game would serve as a therapy for such people, featuring fast response (i.e. resetting a level immediately when the player takes a wrong path or touches a wrong house) and lots of bright colours, inspired by Vaporwave art style (the theme for the Game Jam was "Waves").

Game only features one level where player has to complete 3 stages of following paths to the correct houses. 

This game was my first hand experience with Unity and 3D game development.

**Most source code for the game can be found in Assets/Scripts/Assembly-CSharp directory**

<h2>What has been changed with this build?</h2>
Most changes in this build are code-related: 

-Separated Player controller, game state management and UI management code into separate scripts to make it more modular where one script is responsible just for its own task.

-Created prefabs of paths and instantiated/destroyed them via code rather than make them visible/invisible but still keeping them in the game world regardless which was not as good for the memory. UI Manager has received the same treatment.

I also made some cosmetic changes to the game:

-Textures on the paths are no longer streched out. I have achieved this by setting texture tiling to the x and y scale of the path via code.

-House model now has a visible interior. I have achieved this by having a separate model for the house with normals facing inwards

To see the older build in action and compare: https://www.youtube.com/watch?v=deGr0L7rbSs

<h2>Running the game</h2>
<h3>From Source</h3>
To run it from source, simply clone this repo using *git clone https://github.com/Erikas-Minelga/Agnopsycho-w-a-v-e*, then open the folder using Unity. Version 2018.3.7f1 is recommended. In the project view, navigate to Assets/Scene/levels and load *test*. This will load the level. Hit Play on the Unity editor to play the game, or have a browse through the assets if you'd like :)

<h3>From Executable</h3>
Go to Releases tab on this repo and download Agnopsycho w a v e.zip. Extract it anywhere you'd like and run Agnopsycho w a v e.exe inside the folder

<h3>Controls</h3>

| **Control**        | **Action**           |
| ------------- |:-------------:|
| W      | Walk Forward |
| A      | Straffe Left      | 
| S | Walk Back      |
| D | Straffe Right      |
| Mouse Move | Look Around      |
| Esc | Quit Game      |
