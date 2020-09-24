# RiskOfOptions
An API to add Mod Options in game to ROR2.

## Getting Started
First you need to grab the latest release from the [Thunderstore](https://thunderstore.io/package/Rune48a891aab771429d/Risk_Of_Options/).
Extract the mod to your plugins folder, and then add a reference to the dll in your project in Visual Studio. [Project->Add Reference...->Browse]

Then add to where ever you will use this.
```C#
using RiskOfOptions;
```

Next you need to add Risk Of Options as a dependecy for your mod.
```C#
[BepInDependency("com.rune580.riskofoptions")]
```

Now you're ready to start adding options.

### Adding an option
This needs to be run on Awake()
```C#
ModSettingsManager.addOption(new ModOption(ModOption.OptionType.Slider, "Test Slider", "This is a Slider test."));

ModSettingsManager.addOption(new ModOption(ModOption.OptionType.Bool, "Test Bool", "This is a Bool test."));
```

### Changing the description of the mod panel
```C#
ModSettingsManager.setPanelDescription("Testing stuff");
```

### Changing the title of the mod panel
```C#
ModSettingsManager.setPanelTitle("Risk of Options Testing Stuff");
```

### Fire event when value has changed
for a slider
```C#
ModSettingsManager.addListener(ModSettingsManager.getOption("Test Slider"), new UnityEngine.Events.UnityAction<float>(floatEvent));

public void floatEvent(float f)
        {
            Debug.Log(f);
        }
```
or for a bool
```C#
ModSettingsManager.addListener(ModSettingsManager.getOption("Test Bool"), new UnityEngine.Events.UnityAction<bool>(boolEvent));

public void boolEvent(bool b)
        {
            Debug.Log(b);
        }
```

### Get the current value of an option
```
string b = ModSettingsManager.getOptionValue("Test Bool");

// BaseConVar returns a string as a value.
// for example it could be "1" or "0"

```

### Get the ModOption of an option
```
ModSettingsManager.getOption("Test Bool")
```

## Previews

![Preview 1](/images/example1.jpg)
![Preview 2](/images/example2.jpg)
