## 2.8.2
### Fixed
- Minor regression that caused Step Sliders to display their values as a percentage by default.

### Changed
- the field `formatString` is now obsolete please use the property `FormatString` instead.

## 2.8.1
### Added
- Support for latest RoR2 update.
- `TryParseDelegate` to relevant Numeric Fields and Slider configs.

### Notes
- The RoR2 update broke RoO pretty badly, there may be some visual issues right now, but functionally everything should work again.

## 2.8.0
### Added
- `FloatFieldOption`, `FloatFieldConfig` - Slider option but without the slider
- `IntFieldOption`, `IntFieldConfig` - IntSlider but without the slider :smirk_cat:

### Changed
- The background image of the InputField is now `Sliced` instead of `Simple` which results in it _not_ looking stretched anymore.

### Other
RoO is now on [![NuGet Version](https://img.shields.io/nuget/v/Rune580.Mods.RiskOfRain2.RiskOfOptions?style=for-the-badge&logo=nuget)](https://www.nuget.org/packages/Rune580.Mods.RiskOfRain2.RiskOfOptions)

## 2.7.2
### Added
`richText` `bool` field to `InputFieldConfig` to configure how the in-game input field handles rich text.

## 2.7.1
StringInputFields have improved newline behavior.
- InputFieldConfig has a new field `lineType`, it represents TMP's `lineType` enum.
- SubmitEnum is now marked with the `Flags` attribute, should allow for more fine-tuned input field behavior.
- `Shift` + `Enter` will always insert a newline, if the StringInputField is configured to allow newlines, without submitting.
Because of the above changes, StringInputFields may behave differently to prior versions. If you relied on the
previous behavior, sorry for making more work for you, but this should make input fields be more reliable.
In addition the default behavior for StringInputFields are `MultiLineSubmit` as this mimics the previous
behavior the closest.

## 2.7.0
All numeric InputFields now properly use InvariantCulture, for example numbers are formatted as `1,000,000.20`.
- RiskOfOptions exposes an option to change this behavior in-game.

RiskOfOptions option menu.
Experimental support for Prefabs as mod icons.
- The root object's RectTransform must have a width and height of 45.

Animated icon has been updated.

## 2.6.1
Forgot to include some assets that are required for the color picker.

## 2.6.0
- Added Method for setting mod descriptions with a language token.
- Added event when the mod options panel is closed.
- Slight behaviour change for color wheel, (I did this like a year ago and forgot to push it out in an update, so here you go.)
- Any other commits that happened between last year and now.

## 2.5.3
Merged PR by Bubbet https://github.com/Rune580/RiskOfOptions/pull/28
- Abstracts references to bepinexconfig out.
- Allows mods to extend off of RoO's options.
Updated to preview C# language.

## 2.5.2
Fixed descriptions not wrapping.

## 2.5.1
Minor improvements to the layout of the mod options menu.
Fixed some minor stutters when opening the settings menu for the first time.
Removed HookGenPatcher/MMHook as a dependency.

## 2.5.0
Added ColorOption.
Added ColorPickerUtil for manually opening a color picker.
Added 2 new submit modes for input fields:
- OnExitOrSubmit
- OnSubmit
These can be used with the `submitOn` Field in the InputFieldConfig.
Fixed an issue where having multiple input fields causes hitching.
Fixed an issue where input fields wouldn't visually revert.

## 2.4.2
Sliders can now have their values be manually set in the text box. No more finagling the slider to get the value you want.
The future is now!

## 2.4.1
Fixed category indicators taking up more vertical space than intended.

## 2.4.0
Added IntSlider option, which is just a normal slider but it accepts an ConfigEntry<int> instead.

## 2.3.2
Added overrides for modGuid and modName when adding an option.
CheckBox is now a prefab instead of being copied from a button in game. What does that mean for the normal user?
realistically nothing at all, you shouldn't notice anything different. For the dev however, this puts us a step closer
to having RiskOfOptions ui elements be accessible to other mods.

## 2.3.1
Disable functionality is now fully implemented on all simple options.

## 2.3.0
Quite a few things in this update, as always let me know if you have any issues.
- Added ChoiceOption, takes an CongiEntry<enum>.
- Removed dependency on R2API, I didn't really use it anyways, now you don't have to worry about an unnecessary dependency.
haha dependencies bad ;))))))))))))))))
- Fixed animation issues with the Mod Options menu while playing in singleplayer.
- Added workaround for when a mod embeds it's own settings api (the reason why one would do this alludes me...),
as they can break the Mod Options panel.

## 2.2.0
Added GenericButtonOption which allows for devs to supply a UnityAction that is invoked when the button is pressed.
Cleaned up a few things. Should work for the newest patch, let me know if you find any issues.

## 2.1.0
Added StringInputFieldOption. Configuration option `restartRequired` has been fully implemented,
set this to true to show a restart warning when the option is modified. Description panels now scroll.

## 2.0.0
Massive Rewrite of the entire mod. Now exclusively uses BepInEx ConfigEntry's.
Completely new UI, with a working revert button. Added Stepped Slider, and KeyBind options.
Mods dependent on 1.0 will not work with 2.0

## 1.0.4
Quick update for SOTV.

## 1.0.3
Added R2API as a dependency because I forgot about that. Also I'm currently rewriting ROO,
so hopefully the next update should be a pretty big one. No ETA on that, but progress is good
so far.