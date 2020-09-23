using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using BepInEx;
using MonoMod.RuntimeDetour.HookGen;
using R2API.Utils;
using RoR2.ConVar;
using RoR2.UI;
using RoR2.UI.SkinControllers;
using UnityEngine;

namespace RiskOfOptions
{
    public static class ModSettingsManager
    {
        private static List<ModOption> modOptions = new List<ModOption>();
        private static List<ModPanel> subPanels = new List<ModPanel>();
        private static List<GameObject> modPanelButtons = new List<GameObject>();

        private static bool initilized = false;
        private static bool isOptionsRegistered = false;

        private static GameObject modOptionsPanel;
        private static GameObject modOptionsButton;
        private static GameObject descriptionPanel;

        private static GameObject[] panelControllers;
        private static GameObject[] headerButtons;

        private static GameObject verticalLayout;

        private static GameObject EntryButton;

        private static GameObject SliderPrefab;
        private static GameObject BoolPrefab;
        private static GameObject KeyBindingPrefab;

        private static readonly string StartingText = "risk_of_options";

        private static Dictionary<string, string> modLocals = new Dictionary<string, string>();


        public static void Init()
        {
            On.RoR2.UI.SettingsPanelController.Start += SettingsPanelController_Start;

            On.RoR2.UI.SettingsPanelController.Update += SettingsPanelController_Update;

            On.RoR2.Console.Awake += Console_Awake;

            On.RoR2.Language.GetLocalizedStringByToken += Language_GetLocalizedStringByToken;
        }

        private static string Language_GetLocalizedStringByToken(On.RoR2.Language.orig_GetLocalizedStringByToken orig, RoR2.Language self, string token)
        {
            string result = orig(self, token);

            if (result == token)
            {
                if (result != "" || result != null)
                {
                    if (modLocals.ContainsKey(token))
                    {
                        result = modLocals[token];
                        //Debug.Log($"-------{result}---{token}");
                    }
                }
            }

            return result;
        }

        public static void addListener(ModOption modOption, UnityEngine.Events.UnityAction<float> unityAction)
        {
            modOption.onValueChangedFloat = unityAction;

            if (modOption.gameObject != null)
            {
                modOption.gameObject.GetComponentInChildren<SettingsSlider>().slider.onValueChanged.AddListener(unityAction);
            }
        }

        private static void Console_Awake(On.RoR2.Console.orig_Awake orig, RoR2.Console self)
        {
            orig(self);

            foreach (var item in modOptions)
            {
                RoR2.Console.instance.InvokeMethod("RegisterConVarInternal", new object[] { item.conVar });
                Debug.Log($"[Risk of Options]: {item.conVar.name} ConVar registered.");
            }
        }

        private static void SettingsPanelController_Update(On.RoR2.UI.SettingsPanelController.orig_Update orig, SettingsPanelController self)
        {
            orig(self);

            if (GameObject.Find("GenericHeaderButton (Mod Options)") == null)
            {
                initilized = false;
                isOptionsRegistered = false;
            }

            if (!isOptionsRegistered)
            {
                InstOptions();

                //ModOption hitmarker = ModSettingsManager.getOption("Hitmarker Volume");
                //BaseSettingsControl baseSettings = hitmarker.gameObject.GetComponentInChildren<BaseSettingsControl>();
                //hitmarker.gameObject.GetComponentInChildren<SettingsSlider>().slider.onValueChanged.AddListener(new UnityEngine.Events.UnityAction<float>(eatdick));

                isOptionsRegistered = true;
            }

            if (GameObject.FindObjectsOfType<SettingsPanelController>().Length > 1)
            {
                unloadPanel();
            }

            if (modOptionsButton != null)
            {
                if (modOptionsButton.GetComponentInChildren<HGTextMeshProUGUI>().color != Color.white)
                {
                    modOptionsButton.GetComponentInChildren<HGTextMeshProUGUI>().color = Color.white;
                }
            }

            //if (GameObject.Find("SettingsSubPanel, Mod Options") != null)
            //{
            //    //descriptionPanel.GetComponent<LanguageTextMeshController>().token = "";
            //}

            if (!initilized)
            {
                GameObject gameplayButton = GameObject.Find("GenericHeaderButton (Audio)");

                GameObject testButton = UnityEngine.Object.Instantiate<GameObject>(gameplayButton, gameplayButton.transform.parent);

                testButton.name = "GenericHeaderButton (Mod Options)";

                testButton.GetComponentInChildren<HGTextMeshProUGUI>().SetText("MOD OPTIONS");

                testButton.GetComponentInChildren<HGButton>().onClick.AddListener(loadMainModPanel);

                descriptionPanel = GameObject.Find("DescriptionText");

                initilized = true;
            }
        }

        private static void SettingsPanelController_Start(On.RoR2.UI.SettingsPanelController.orig_Start orig, SettingsPanelController self)
        {
            orig(self);

            InitPanel();
        }

        public static void setPanelDescription(string description)
        {
            var classes = Assembly.GetCallingAssembly().GetExportedTypes();

            string id = "";

            foreach (var item in classes)
            {
                BepInPlugin bepInPlugin = item.GetCustomAttribute<BepInPlugin>();

                if (bepInPlugin != null)
                {
                    id = bepInPlugin.GUID;
                    break;
                }
            }

            modLocals[$"{StartingText}.PanelButton.{id}.PanelButtonDescription".ToUpper().Replace(" ", "_")] = description;
        }

        public static void loadMainModPanel()
        {
            foreach (var item in panelControllers)
            {
                if (item.activeSelf)
                {
                    item.SetActive(false);
                }
            }

            foreach (var item in headerButtons)
            {
                item.GetComponent<HGButton>().interactable = true;
            }

            foreach (var item in subPanels)
            {
                if (item.entryButton == null)
                {
                    var newEntryButton = UnityEngine.Object.Instantiate<GameObject>(EntryButton, EntryButton.transform.parent);

                    newEntryButton.GetComponentInChildren<HGTextMeshProUGUI>().SetText($"{item.modName} Options");
                    newEntryButton.GetComponent<HGButton>().onClick.RemoveAllListeners();
                    newEntryButton.GetComponent<HGButton>().onClick.AddListener(delegate { loadModPanel(item); });
                    newEntryButton.GetComponent<HGButton>().hoverToken = $"{StartingText}.PanelButton.{item.modGUID}.PanelButtonDescription".ToUpper().Replace(" ", "_");

                    item.entryButton = newEntryButton;

                    if (!newEntryButton.activeSelf)
                    {
                        newEntryButton.SetActive(true);
                    }
                }
            }


            if (modOptionsButton == null)
            {
                modOptionsButton = GameObject.Find("GenericHeaderButton (Mod Options)");
            }

            GameObject highlightedButton = GameObject.Find("GenericHeaderHighlight");

            modOptionsButton.GetComponent<HGButton>().interactable = false;

            highlightedButton.transform.position = modOptionsButton.transform.position;

            modOptionsPanel.SetActive(true);

        }

        public static void loadModPanel(ModPanel modPanel)
        {
            Debug.Log(modPanel.modName);

            modOptionsPanel.SetActive(false);

            modPanel.panel.SetActive(true);

            modPanel.backButton.SetActive(true);

            foreach (var mo in modOptions)
            {
                if (modPanel.ContainsModOption(mo))
                {
                    mo.gameObject.SetActive(true);
                }
            }
        }

        public static void backToModPanel()
        {
            unloadPanel();
            loadMainModPanel();
        }

        public static void unloadPanel()
        {

            if (modOptionsPanel.activeSelf)
            {
                modOptionsPanel.SetActive(false);
            }

            foreach (var item in subPanels)
            {
                if (item.panel.activeSelf)
                {
                    item.panel.SetActive(false);
                }
            }

            modOptionsButton.GetComponent<HGButton>().interactable = true;
        }

        private static void InitPanel()
        {
            GameObject audioPanel = null;
            GameObject keybindingPanel = null;

            if (modOptionsPanel == null)
            {
                SettingsPanelController[] objects = Resources.FindObjectsOfTypeAll<SettingsPanelController>();

                panelControllers = new GameObject[6];
                headerButtons = new GameObject[] { GameObject.Find("GenericHeaderButton (Gameplay)"), GameObject.Find("GenericHeaderButton (KB & M)"), GameObject.Find("GenericHeaderButton (Controller)"),
                                                   GameObject.Find("GenericHeaderButton (Audio)"), GameObject.Find("GenericHeaderButton (Video)"), GameObject.Find("GenericHeaderButton (Graphics)")};

                for (int i = 0; i < objects.Length / 2; i++)
                {
                    if (objects[i].name == "SettingsSubPanel, Audio")
                    {
                        audioPanel = objects[i].gameObject;
                    }
                    else if (objects[i].name == "SettingsSubPanel, Controls (M&KB)")
                    {
                        keybindingPanel = objects[i].gameObject;
                    }

                    panelControllers[i] = objects[i].gameObject;
                }

                if (audioPanel != null)
                {
                    modOptionsPanel = UnityEngine.Object.Instantiate<GameObject>(audioPanel, audioPanel.transform.parent);

                    modOptionsPanel.name = "SettingsSubPanel, Mod Options";

                    verticalLayout = modOptionsPanel.transform.Find("Scroll View").Find("Viewport").Find("VerticalLayout").gameObject;

                    var masterVolume = verticalLayout.transform.Find("SettingsEntryButton, Slider (Master Volume)").gameObject;

                    SliderPrefab = UnityEngine.Object.Instantiate<GameObject>(masterVolume, masterVolume.transform.parent);
                    SliderPrefab.SetActive(false);

                    var audioFocus = verticalLayout.transform.Find("SettingsEntryButton, Bool (Audio Focus)").gameObject;

                    BoolPrefab = UnityEngine.Object.Instantiate<GameObject>(audioFocus, audioFocus.transform.parent);
                    BoolPrefab.SetActive(false);

                    UnityEngine.Object.DestroyImmediate(verticalLayout.transform.Find("SettingsEntryButton, Slider (Master Volume)").gameObject);
                    UnityEngine.Object.DestroyImmediate(verticalLayout.transform.Find("SettingsEntryButton, Slider (SFX Volume)").gameObject);
                    UnityEngine.Object.DestroyImmediate(verticalLayout.transform.Find("SettingsEntryButton, Slider (MSX Volume)").gameObject);
                    UnityEngine.Object.DestroyImmediate(verticalLayout.transform.Find("SettingsEntryButton, Bool (Audio Focus)").gameObject);

                    var keybindButton = keybindingPanel.transform.Find("Scroll View").Find("Viewport").Find("VerticalLayout").Find("SettingsEntryButton, Binding (Jump)").gameObject;

                    KeyBindingPrefab = UnityEngine.Object.Instantiate<GameObject>(keybindButton, keybindButton.transform.parent);
                    keybindingPanel.SetActive(false);

                    modOptionsPanel.SetActive(false);

                    foreach (var item in modOptions)
                    {
                        if (!subPanels.ContainsModPanel(item))
                        {
                            GameObject subPanel = UnityEngine.Object.Instantiate<GameObject>(modOptionsPanel, modOptionsPanel.transform.parent);
                            ModPanel modPanel = new ModPanel(subPanel, item.owner, item.modName);

                            subPanel.SetActive(false);

                            subPanels.Add(modPanel);
                        }
                    }

                    foreach (var item in subPanels)
                    {
                        if (item.panel == null)
                        {
                            item.panel = UnityEngine.Object.Instantiate<GameObject>(modOptionsPanel, modOptionsPanel.transform.parent);
                        }

                        if (EntryButton == null)
                        {
                            EntryButton = UnityEngine.Object.Instantiate<GameObject>(BoolPrefab, BoolPrefab.transform.parent);

                            UnityEngine.Object.DestroyImmediate(EntryButton.GetComponentInChildren<CarouselController>());

                            UnityEngine.Object.DestroyImmediate(EntryButton.GetComponentInChildren<ButtonSkinController>());

                            UnityEngine.Object.DestroyImmediate(EntryButton.transform.Find("CarouselRect").GetComponentInChildren<UnityEngine.UI.Image>());

                            foreach (var button in EntryButton.transform.Find("CarouselRect").GetComponentsInChildren<HGButton>())
                            {
                                UnityEngine.Object.DestroyImmediate(button);
                            }

                            foreach (var component in EntryButton.transform.Find("CarouselRect").GetComponentsInChildren<Component>())
                            {
                                if (component.GetType() == typeof(UnityEngine.UI.Image))
                                {
                                    UnityEngine.Object.DestroyImmediate(component);
                                }
                            }

                            EntryButton.GetComponent<HGButton>().interactable = true;
                            EntryButton.GetComponent<HGButton>().enabled = true;
                            EntryButton.GetComponent<HGButton>().disablePointerClick = false;
                            EntryButton.GetComponent<HGButton>().onClick.RemoveAllListeners();

                            item.longName = $"{StartingText}.PanelButton.{item.modGUID}";

                            UnityEngine.Object.DestroyImmediate(EntryButton.GetComponent<LanguageTextMeshController>());

                            EntryButton.SetActive(false);
                        }

                        //var newEntryButton = UnityEngine.Object.Instantiate<GameObject>(EntryButton, EntryButton.transform.parent);

                        //newEntryButton.GetComponentInChildren<HGTextMeshProUGUI>().SetText($"{item.modName} Options");
                        //newEntryButton.GetComponent<HGButton>().onClick.AddListener(delegate { loadModPanel(item); });
                        //newEntryButton.GetComponent<HGButton>().hoverToken = $"{StartingText}.PanelButton.{item.modGUID}.PanelButtonDescription".ToUpper().Replace(" ", "_");

                        //item.entryButton = newEntryButton;

                        GameObject verticalLayout = item.panel.transform.Find("Scroll View").Find("Viewport").Find("VerticalLayout").gameObject;

                        var newExitButton = UnityEngine.Object.Instantiate<GameObject>(EntryButton, verticalLayout.transform);

                        newExitButton.GetComponentInChildren<HGTextMeshProUGUI>().SetText($"Back to Mod Options");
                        newExitButton.GetComponent<HGButton>().onClick.AddListener(backToModPanel);
                        newExitButton.GetComponent<HGButton>().hoverToken = $"{StartingText}.GenericBackButton.PanelButtonDescription".ToUpper().Replace(" ", "_");

                        modLocals[$"{StartingText}.GenericBackButton.PanelButtonDescription".ToUpper().Replace(" ", "_")] = "Back To The Main Mod Options Panel";

                        item.backButton = newExitButton;
                    }
                }

            }
        }

        public static void InstOptions()
        {
            foreach (var mo in modOptions)
            {
                GameObject newOption = null;

                ModPanel modPanel = subPanels.GetPanel(mo);

                GameObject verticalLayout = modPanel.panel.transform.Find("Scroll View").Find("Viewport").Find("VerticalLayout").gameObject;

                if (mo.optionType == ModOption.OptionType.Slider)
                {
                    newOption = UnityEngine.Object.Instantiate<GameObject>(SliderPrefab, verticalLayout.transform);

                    newOption.GetComponentInChildren<SettingsSlider>().settingName = mo.longName;
                    newOption.GetComponentInChildren<SettingsSlider>().nameToken = mo.longName.ToUpper().Replace(" ", "_");

                    if (mo.onValueChangedFloat != null)
                    {
                        newOption.GetComponentInChildren<SettingsSlider>().slider.onValueChanged.AddListener(mo.onValueChangedFloat);
                    }

                    modLocals[mo.longName.ToUpper().Replace(" ", "_")] = mo.name;

                    newOption.name = $"ModOptions, Slider ({mo.longName})";
                }
                else if (mo.optionType == ModOption.OptionType.Bool)
                {
                    newOption = UnityEngine.Object.Instantiate<GameObject>(BoolPrefab, verticalLayout.transform);

                    newOption.GetComponentInChildren<CarouselController>().settingName = mo.longName;
                    newOption.GetComponentInChildren<CarouselController>().nameToken = mo.longName.ToUpper().Replace(" ", "_");

                    newOption.GetComponentInChildren<HGButton>().hoverToken = mo.longName.ToUpper().Replace(" ", "_"); ;

                    modLocals[mo.longName.ToUpper().Replace(" ", "_")] = mo.name;

                    newOption.name = $"ModOptions, Bool ({mo.longName})";
                }
                else if (mo.optionType == ModOption.OptionType.Keybinding)
                {

                }

                newOption.GetComponentInChildren<HGButton>().hoverToken = $"{mo.longName.ToUpper().Replace(" ", "_")}_DESCRIPTION";
                modLocals[$"{mo.longName.ToUpper().Replace(" ", "_")}_DESCRIPTION"] = mo.description;

                //UnityEngine.Object.DestroyImmediate(newOption.GetComponentInChildren<ButtonSkinController>());
                //UnityEngine.Object.DestroyImmediate(newOption.GetComponentInChildren<HGButton>());

                

                mo.gameObject = newOption;
            }
        }

        public static void RegisterOption(ModOption mo)
        {
            if (mo.optionType == ModOption.OptionType.Slider)
            {
                mo.conVar = new FloatConVar(mo.longName, RoR2.ConVarFlags.Archive, null, mo.description);
            }
            else if (mo.optionType == ModOption.OptionType.Bool)
            {
                mo.conVar = new BoolConVar(mo.longName, RoR2.ConVarFlags.Archive, null, mo.description);
            }
            else if (mo.optionType == ModOption.OptionType.Keybinding)
            {
                throw new NotImplementedException("Keybinding options are not yet supported.");
            }

            modOptions.Add(mo);
        }

        public static void addOption(ModOption mo)
        {
            mo.longName = $"{StartingText}.{mo.owner.ToLower()}.{mo.name.ToLower().Replace(" ", "_")}";
            RegisterOption(mo);
        }

        public static ModOption getOption(string name)
        {
            var classes = Assembly.GetCallingAssembly().GetExportedTypes();

            string id = "";

            foreach (var item in classes)
            {
                BepInPlugin bepInPlugin = item.GetCustomAttribute<BepInPlugin>();

                if (bepInPlugin != null)
                {
                    id = bepInPlugin.GUID;
                    Debug.LogWarning($"ModOption request from {id}.");
                    break;
                }
            }

            foreach (var item in modOptions)
            {
                if (item.longName == $"{StartingText}.{id.ToLower()}.{name.ToLower().Replace(" ", "_")}")
                {
                    return item;
                }
            }

            Debug.LogError($"ModOption {name} not found!");
            return null;
        }

        public static string getOptionValue(string name)
        {
            BaseConVar conVar = null;

            var classes = Assembly.GetCallingAssembly().GetExportedTypes();

            string id = "";

            foreach (var item in classes)
            {
                BepInPlugin bepInPlugin = item.GetCustomAttribute<BepInPlugin>();

                if (bepInPlugin != null)
                {
                    id = bepInPlugin.GUID;
                    break;
                }
            }

            if (id == "")
            {
                Debug.LogError("GUID of calling mod not found!");
                return "";
            }

            conVar = RoR2.Console.instance.FindConVar($"{StartingText}.{id.ToLower()}.{name.ToLower().Replace(" ", "_")}");

            if (conVar == null)
            {
                Debug.LogError($"Convar {name} not found in convars.");
            }

            return conVar.GetString();
        }


        public static bool ContainsModOption(this List<ModOption> modOptions, ModOption modOption)
        {
            foreach (var item in modOptions)
            {
                if (item == modOption)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool ContainsModOption(this ModPanel modPanel, ModOption modOption)
        {
            var temp = subPanels.GetPanel(modOption);

            if (modPanel == temp)
            {
                return true;
            }
            return false;
        }

        public static bool ContainsModPanel(this List<ModPanel> modPanels, ModOption modOption)
        {
            foreach (var item in modPanels)
            {
                if (item.modGUID == modOption.owner)
                {
                    return true;
                }
            }
            return false;
        }

        public static ModPanel GetPanel(this List<ModPanel> modPanels, ModOption modOption)
        {
            foreach (var item in modPanels)
            {
                if (item.modGUID == modOption.owner)
                {
                    return item;
                }
            }

            Debug.LogWarning($"Modpanel not found.");
            return null;
        }
    }
}
