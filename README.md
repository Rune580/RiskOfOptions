# RiskOfOptions
An API to add Mod Options in game to ROR2.

## Getting Started
First you need to grab the latest release from the Thunderstore [insert link here].
Extract the mod to your plugins folder, and then add a reference to it in Visual Studio.

Next you need to add Risk Of Options as a dependecy for your mod.
```C#
[BepInDependency("com.rune580.riskofoptions")]
```

Now you're ready to start adding options.

### Adding an option

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
