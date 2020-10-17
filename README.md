# customEliteBadges
A Blackwake mod that allows custom elite badges based on level

# Installation
1. Install the required files inside **\RequiredFiles** into **\Blackwake\Blackwake_data\Managed**
 - **NOTE** you will have ot overwrite the existing **Assembly-CSharp.dll**
2. Launch your game, it should generate the following folder: **\Blackwake\Blackwake_data\Managed\Mods**
3. Close the game, and place **customEliteBadges.dll** inside **\Managed\Mods**
4. Place (either your own badge assets, or the included ones) inside **\Blackwake\Blackwake_data\Managed\Mods\Assets\Archie\Textures**
5. Double check your file structure is similar to that shown in **TEMPLATE-LAYOUT**
6. Launch the game and play!

# Setting up custom badges
When running initially you wil notice that a txt will be generated in the same folder as your mod **customBadges.txt**. Inside this file you use the following format:
**EXAMPLE**
```
100=silver
200=default
500=eyes
```
The following is the default result if you do not setup a custom list ^.

The **Left** side of the **=** designates the level at which it starts, the **Right** side designates the name of the badge to use.

`NOTE I HAVE NOT YET TESTED, MAY NOT WORK`