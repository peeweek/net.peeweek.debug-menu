# Debug Menu

A debug menu for use at runtime in your Unity Projects. The goal is to keep it as simple as possible. The menu can register items to extend its possibilities. These items are easy to write and implement. There are some examples shipped with the base package.

## Requirements

* Unity 2021.3
* Input System Package

## Install

### OpenUPM Scoped Registry (2020.1 and newer)

Open Project Preferences and go to Package manager Window.

If not present, add this scoped registry:

* Name: **OpenUPM**
* URL : `https://package.openupm.com`
* Scopes : `net.peeweek`

Once added, you can close the project settings window.

Open Package manager (Window/Package Manager), select the **Debug Menu** package, and click the install button.

## How to use

* By default, a default debug menu is spawned when playing.
* Press the F12 key at runtime to toggle the Console (Can be customized using the Debug Menu Configuration Asset)

## Customizing the Debug Menu

The debug menu can be customized using the menu `Tools/Debug Menu/Create DebugMenu Configuration Asset`



