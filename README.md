# app launcher
### A simple yet powerful launcher for the command line meant to be an all in one solution

# installation
1. Download the app form the [Releases](https://github.com/Tommy1250/app-launcher/releases) page.
1. Add the app to PATH.
1. Restart any terminal window open.
1. Congrats.

# Usage

- applauncher help
> Displays the help message
- applauncher add [Shortcutname] [location] "[args]"
> Adds a new shortcut withg the args if specified
The args are optional depending on the app
if there is multiple arguments put them between quotes ("")
The location can be a launcher link
> the shortcut name can't have spaces
- applauncher remove [shortcutname]
> Removes the specified shorcut name
- applauncher list
> Lists all the avalable shortcuts and aliases
- applanucher alias add [alias] [shortcutname]
> Adds a new alias to a shortcut
A shortcut can have multiple aliases
- applauncher alias remove [alias]
> Removes the specified alias
- applauncher launch [shortcutname/alias]
> Launches the given app or alias 
- applauncher listjson
> Prints the main file in a json format in the console
Can be useful in integrating other apps with this app
- applauncher getlocation
> Prints the current app location to the Console