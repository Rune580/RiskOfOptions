# RiskOfOptions
![Animated icon made by UnsavedTrash#0001 on discord](https://cdn.rune580.dev/roo/images/roo-full.gif)

[![NuGet Version](https://img.shields.io/nuget/v/Rune580.Mods.RiskOfRain2.RiskOfOptions?style=for-the-badge&logo=nuget)](https://www.nuget.org/packages/Rune580.Mods.RiskOfRain2.RiskOfOptions)
[![Thunderstore Version](https://img.shields.io/thunderstore/v/Rune580/Risk_Of_Options?style=for-the-badge&logo=thunderstore&logoColor=white)](https://thunderstore.io/c/riskofrain2/p/Rune580/Risk_Of_Options/)
[![Thunderstore Downloads](https://img.shields.io/thunderstore/dt/Rune580/Risk_Of_Options?style=for-the-badge&logo=thunderstore&logoColor=white)](https://thunderstore.io/c/riskofrain2/p/Rune580/Risk_Of_Options/)
[![GitHub Actions Workflow Status](https://img.shields.io/github/actions/workflow/status/Rune580/RiskOfOptions/build.yml?branch=master&style=for-the-badge&logo=github)](https://github.com/Rune580/RiskOfOptions/actions/workflows/build.yml)

An API to provide a user interface in game to interact with BepInEx ConfigEntry's.

## Currently supported options
- CheckBoxes `bool`
- Sliders `float`
- StepSliders `float`
- IntSliders `int`
- KeyBinds `KeyboardShortcut`
- String Input Fields `string`
- Choice DropDowns `Enum`
- Color Picker `UnityEngine.Color`

### Additional Components
- GenericButtons

### For feature requests or issues head over to my [repository](https://github.com/Rune580/RiskOfOptions).

## Developer Resources
* [RiskOfOptions Wiki](https://github.com/Rune580/RiskOfOptions/wiki)
* [Usage Example](https://github.com/Rune580/RiskOfOptions-Example)

## Getting Started
First you need to grab the latest release from the [Thunderstore](https://thunderstore.io/package/Rune580/Risk_Of_Options/).
Extract the mod to your plugins folder, and then add a reference to the dll in your project in Visual Studio. [Project->Add Reference...->Browse]

Then add to where ever you will use this.
```C#
using RiskOfOptions;
```

Next you need to add Risk Of Options as a dependency for your mod.
```C#
[BepInDependency("com.rune580.riskofoptions")]
```

Finally make sure you know how to use [BepInEx Config](https://github.com/risk-of-thunder/R2Wiki/wiki/Configuration
)

Now you're ready to start adding options.

### Adding an option
Given a `ConfigEntry<bool>`
```C#
ConfigEntry<bool> enableThing = Config.Bind(...);

ModSettingsManager.AddOption(new CheckBoxOption(enableThing);
```

Need a volume slider?
```C#
ConfigEntry<float> volume = Config.Bind(...);

ModSettingsManager.AddOption(new SliderOption(volume));
```

Every option constructor can take a Config for the example above it would be `SliderConfig`.
Say you need a slider that only goes between 60 - 130. You would do:
```C#
ModSettingsManager.AddOption(new SliderOption(limitedRangeFloat, new SliderConfig() { min = 60, max = 130 }));
```

What about a slider that goes in increments of 0.15 and is limited between 1 - 5?
```C#
ModSettingsManager.AddOption(new StepSliderOption(incrementedFloat, new StepSliderConfig() { min = 1, max = 5, increment = 0.15f }));
```

Enough about floats, let's talk about the spaghetti and meatballs, KeyBinds.
```C#
ConfigEntry<KeyboardShortcut> keyBind = Config.Bind(...);

ModSettingsManager.AddOption(new KeyBindOption(keyBind)); // This also has a KeyBindConfig but can be omitted if defaults are desired.
```
And that's it, said KeyboardShortcut will show up on the ModOptions menu.

Checkbox and Slider configs can be set with a delegate that will be used to check if said option should be disabled in the menu.
```C#
ConfigEntry<bool> disableThing = Config.Bind(...);
ConfigEntry<bool> overridenThing = Config.Bind(...); 

ModSettingsManager.AddOption(new CheckBoxOption(disableThing));
ModSettingsManager.AddOption(new CheckBoxOption(overridenThing, new CheckBoxConfig() { checkIfDisabled = Check }));

...

private bool Check()
{
    return disabledThing.value;
}
```
When `disableThing` is enabled `overridenThing` will show up as non-interactable in the menu.

"Okay that's all fine but how do I, you know, do stuff when an value is changed?"
Well thankfully `ConfigEntry`'s have this innately:
```C#
ConfigEntry<bool> toggleThing = Config.Bind(...);

toggleThing.SettingChanged += (object, args) => { Debug.Log(toggleThing.Value) };
```
Of course when an option changes the value of a passed `ConfigEntry`, the value updates in real time,
so in some cases where you are checking the value of the entry directly you don't need to do anything.

There may be cases where you just want a convenient button to open your own menu, as such you can do this:
```C#
ModSettingsManager.AddOption(new GenericButtonOption("Custom Menu", "Misc", "Configure stuff in here", "Open Custom Menu", OpenMenu));

private void OpenMenu()
{
    /// Do stuff
}
```
The GenericButtonOption may be used to provide an entry point for opening your custom GUI's.

### Setting the description of the mod
```C#
ModSettingsManager.SetModDescription("Describe your mod in incredible detail over the course of the next 2 hours");
```

### Setting the icon of the mod
```C#
Sprite icon = ...;

ModSettingsManager.SetModIcon(icon);
```

# Quick Showcase
[Showcase](https://gfycat.com/GloomyShowyArrowana)

# Contact

Discord: @rune

Github: Rune580