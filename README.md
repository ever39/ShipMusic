# ShipMusic

A simple mod that adds some music to the ship to play at all times, even in the worst situations you've ever been in!

## Installation, configuration.
- Installation is done by either using the Thunderstore Mod Manager (or r2modman), or it can be done manually by pressing the "Manual Download" button on the thunderstore page of the mod, and then dropping the mod folder inside the ```BepInEx/plugins``` folder. The config file will be made on the first game startup with the mod enabled.
- To configure the mod you can use either the Config editor provided by Thunderstore Mod Manager (alternatively r2modman) or you can access the ```command.ShipMusic.cfg``` file made by the mod in ```BepInEx/config```, and change the values from there.

## Mod Configs
Currently there are only three config settings for the mod, those being:
- ```Sound Volume``` (which controls the volume of the set music)
- ```Max Distance``` (which controls the maximum distance at which you can still hear the set music)
- ```Sound Filter``` (which adds a "radio" filter to the set music, for aesthetic purposes)

The sound volume and max distance can be updated in-game, however turning the sound filter on or off requires you to rejoin to apply

## Replacing the default music
The mod has a ```music.wav``` file, although .mp3 and .ogg files also work. Changing this music file requires deleting the old music file (or changing its location to somewhere else that isn't in the mod folder), then moving the new sound file to the mod folder. The mod first checks for ```.wav``` files, then for ```.ogg``` files and lastly for ```.mp3``` files, which is why deleting the old music file is advised. Make sure that the new file name is ```music```, as the mod only checks for that file name.

>[!IMPORTANT]
>This mod is CLIENT-SIDED, so you can only hear your music, and other players with the mod installed may only hear their set music.
