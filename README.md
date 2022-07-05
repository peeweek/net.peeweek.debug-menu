# Debug Menu

A debug menu for use at runtime in your Unity Projects. The goal is to keep it as simple as possible. The menu can register items to extend its possibilities. These items are easy to write and implement. There are some examples shipped with the base package.

![](https://raw.githubusercontent.com/peeweek/net.peeweek.debug-menu/main/Documentation%7E/Screenshot.png)

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

Editing this asset enables fine tuning over the configuration of the menu including position (left, center or right), width and height of lines, font type and size overrides, colors and inputs.


## C# API

There is a simple API revolving around the debug menu, it enables handling things when the menu is opened/closed (for instance to disable input for other objects). Also, new items can be created using simple class declaration.

The `DebugMenu` class can be accessed via the `using DebugMenuUtility;` namespace usage.

### Handling Open/Close of the menu

A callback is available through the `DebugMenu.onDebugMenuToggle` event. It calls a delegate with a bool argument that tells whether the menu is opened or not. In the example below, the callback sets a `paused` flag to prevent player movement while the menu is visible. 

```csharp
    private void OnEnable()
    {
        DebugMenuUtility.DebugMenu.onDebugMenuToggle += DebugMenu_onDebugMenuToggle;
    }

    private void OnDisable()
    {
        DebugMenuUtility.DebugMenu.onDebugMenuToggle -= DebugMenu_onDebugMenuToggle;
    }
    private void DebugMenu_onDebugMenuToggle(bool visible)
    {
        this.paused = visible;
    }
```

### Adding new Items to the menu

New menu items can be added through simple class declaration (inheriting `DebugMenuItem`), and using the `DebugMenuItem` class attribute to define in which category is set the element. Using slashes in the category path enable creating sub folders.

Here is an example below that showcases how to manipulate the `Screen.fullScreenMode` enum value.
The following members can/haveto be overridden:
- label (mandatory) : the label of the item
- value (mandatory) : the value displayed
- OnValidate (optional) : the method called when pressing the validate key/button.
- OnLeft (optional) : the method called when pressing the left key/button
- OnRight (optional) : the method called when pressing the right key/button

```csharp
    [DebugMenuItem("Rendering")]
    class FullScreenModeMenuItem : DebugMenuItem
    {
        public override string label => "Full Screen Mode";
        public override string value => Screen.fullScreenMode.ToString();
        public override Action OnValidate => Increase;
        public override Action OnLeft => Decrease;
        public override Action OnRight => Increase;

        void Increase() => Toggle(1);
        void Decrease() => Toggle(-1);
        void Toggle(int pad = 1)
        {
            Screen.fullScreenMode = (FullScreenMode)(((int)Screen.fullScreenMode + pad) % 4);
        }
    }
```
