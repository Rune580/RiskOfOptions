using System;
using System.Collections.Generic;
using System.Reflection;
using BepInEx;
using R2API;
using R2API.Utils;
using RoR2.ConVar;
using RoR2.UI;
using RoR2.UI.SkinControllers;
using UnityEngine;

namespace RiskOfOptions
{
    public static class ModSettingsManager
    {
        //private static List<ModOption> modOptions = new List<ModOption>();

        private static List<OptionContainer> optionContainers = new List<OptionContainer>();

        //private static List<ModPanel> subPanels = new List<ModPanel>();


        //private static GameObject modOptionsPanel;
        //private static GameObject modOptionsButton;

        //private static GameObject[] panelControllers;
        //private static GameObject[] headerButtons;

        private static List<UnityEngine.Events.UnityAction> Listeners = new List<UnityEngine.Events.UnityAction>();

        //private static GameObject verticalLayout;

        //private static GameObject EntryButton;

        private static GameObject SliderPrefab;
        private static GameObject BoolPrefab;
        private static GameObject KeyBindingPrefab;

        internal static readonly string StartingText = "risk_of_options";


        public static void Init()
        {
            On.RoR2.Console.Awake += Console_Awake;

            SettingsMenu.Init();
        }

        public static void addStartupListener(UnityEngine.Events.UnityAction unityAction)
        {
            Listeners.Add(unityAction);
        }

        public static void addListener(ModOption modOption, UnityEngine.Events.UnityAction<float> unityAction)
        {
            modOption.onValueChangedFloat = unityAction;
        }

        public static void addListener(ModOption modOption, UnityEngine.Events.UnityAction<bool> unityAction)
        {
            modOption.onValueChangedBool = unityAction;
        }

        private static void Console_Awake(On.RoR2.Console.orig_Awake orig, RoR2.Console self)
        {
            orig(self);

            foreach (var container in optionContainers)
            {
                foreach (var mo in container.GetModOptionsCached())
                {
                    //Debug.Log($"{mo.Name} Default Value: {mo.conVar.defaultValue}");

                    mo.conVar.SetString(mo.defaultValue);

                    RoR2.Console.instance.InvokeMethod("RegisterConVarInternal", new object[] { mo.conVar });
                    Debug.Log($"{mo.conVar.name} Option registered to console.");
                }
            }

            RoR2.Console.instance.SubmitCmd(null, "exec config", false);

            foreach (var item in Listeners)
            {
                item.Invoke();
            }
        }

            //private static void SettingsPanelController_Update(On.RoR2.UI.SettingsPanelController.orig_Update orig, SettingsPanelController self)
            //{
            //    orig(self);

            //    if (GameObject.Find("GenericHeaderButton (Mod Options)") == null)
            //    {
            //        initilized = false;
            //        isOptionsRegistered = false;
            //    }

            //    if (!isOptionsRegistered)
            //    {
            //        InstOptions();

            //        isOptionsRegistered = true;
            //    }

            //    if (GameObject.FindObjectsOfType<SettingsPanelController>().Length > 1)
            //    {
            //        unloadPanel();
            //    }

            //    if (modOptionsButton != null)
            //    {
            //        if (modOptionsButton.GetComponentInChildren<HGTextMeshProUGUI>().color != Color.white)
            //        {
            //            modOptionsButton.GetComponentInChildren<HGTextMeshProUGUI>().color = Color.white;
            //        }
            //    }

            //    if (!initilized)
            //    {
            //        GameObject gameplayButton = GameObject.Find("GenericHeaderButton (Audio)");

            //        GameObject testButton = UnityEngine.Object.Instantiate<GameObject>(gameplayButton, gameplayButton.transform.parent);

            //        testButton.name = "GenericHeaderButton (Mod Options)";

            //        testButton.GetComponentInChildren<HGTextMeshProUGUI>().SetText("MOD OPTIONS");

            //        testButton.GetComponentInChildren<HGButton>().onClick.AddListener(loadMainModPanel);

            //        initilized = true;
            //    }
            //}

            //private static void SettingsPanelController_Start(On.RoR2.UI.SettingsPanelController.orig_Start orig, SettingsPanelController self)
            //{
            //    orig(self);

            //    InitPanel();
            //}

            //public static void setPanelDescription(string description)
            //{
            //    var classes = Assembly.GetCallingAssembly().GetExportedTypes();

            //    string id = "";

            //    foreach (var item in classes)
            //    {
            //        BepInPlugin bepInPlugin = item.GetCustomAttribute<BepInPlugin>();

            //        if (bepInPlugin != null)
            //        {
            //            id = bepInPlugin.GUID;
            //            break;
            //        }
            //    }

            //    modLocals[$"{StartingText}.PanelButton.{id}.PanelButtonDescription".ToUpper().Replace(" ", "_")] = description;
            //}

        public static void setPanelDescription(string description)
        {

        }

        public static void setPanelTitle(string title)
        {

        }

        //public static void setPanelTitle(string title)
        //{
        //    var classes = Assembly.GetCallingAssembly().GetExportedTypes();

        //    string id = "";

        //    foreach (var item in classes)
        //    {
        //        BepInPlugin bepInPlugin = item.GetCustomAttribute<BepInPlugin>();

        //        if (bepInPlugin != null)
        //        {
        //            id = bepInPlugin.GUID;
        //            break;
        //        }
        //    }

        //    modLocals[$"{StartingText}.PanelButton.{id}.PanelButtonTitle".ToUpper().Replace(" ", "_")] = title;
        //}

        //public static void loadMainModPanel()
        //{
        //    foreach (var item in panelControllers)
        //    {
        //        if (item.activeSelf)
        //        {
        //            item.SetActive(false);
        //        }
        //    }

        //    foreach (var item in headerButtons)
        //    {
        //        item.GetComponent<HGButton>().interactable = true;
        //    }

        //    foreach (var item in subPanels)
        //    {
        //        if (item.entryButton == null)
        //        {
        //            var newEntryButton = UnityEngine.Object.Instantiate<GameObject>(EntryButton, EntryButton.transform.parent);

        //            string Titletext = $"{item.modName} Options";

        //            if (modLocals.ContainsKey($"{StartingText}.PanelButton.{item.modGUID}.PanelButtonTitle".ToUpper().Replace(" ", "_")))
        //            {
        //                Titletext = modLocals[$"{StartingText}.PanelButton.{item.modGUID}.PanelButtonTitle".ToUpper().Replace(" ", "_")];
        //            }

        //            newEntryButton.GetComponentInChildren<HGTextMeshProUGUI>().SetText(Titletext);
        //            newEntryButton.GetComponent<HGButton>().onClick.RemoveAllListeners();
        //            newEntryButton.GetComponent<HGButton>().onClick.AddListener(delegate { loadModPanel(item); });
        //            newEntryButton.GetComponent<HGButton>().hoverToken = $"{StartingText}.PanelButton.{item.modGUID}.PanelButtonDescription".ToUpper().Replace(" ", "_");

        //            item.entryButton = newEntryButton;

        //            if (!newEntryButton.activeSelf)
        //            {
        //                newEntryButton.SetActive(true);
        //            }
        //        }
        //    }


        //    if (modOptionsButton == null)
        //    {
        //        modOptionsButton = GameObject.Find("GenericHeaderButton (Mod Options)");
        //    }

        //    GameObject highlightedButton = GameObject.Find("GenericHeaderHighlight");

        //    modOptionsButton.GetComponent<HGButton>().interactable = false;

        //    highlightedButton.transform.position = modOptionsButton.transform.position;

        //    modOptionsPanel.SetActive(true);

        //}

        //public static void loadModPanel(ModPanel modPanel)
        //{
        //    modOptionsPanel.SetActive(false);

        //    modPanel.panel.SetActive(true);

        //    modPanel.backButton.SetActive(true);

        //    foreach (var mo in modOptions)
        //    {
        //        if (modPanel.ContainsModOption(mo))
        //        {
        //            mo.gameObject.SetActive(true);
        //        }
        //    }
        //}

        //public static void backToModPanel()
        //{
        //    unloadPanel();
        //    loadMainModPanel();
        //}

        //public static void unloadPanel()
        //{

        //    if (modOptionsPanel.activeSelf)
        //    {
        //        modOptionsPanel.SetActive(false);
        //    }

        //    foreach (var item in subPanels)
        //    {
        //        if (item.panel.activeSelf)
        //        {
        //            item.panel.SetActive(false);
        //        }
        //    }

        //    modOptionsButton.GetComponent<HGButton>().interactable = true;
        //}

        //private static void InitPanel()
        //{
        //    GameObject audioPanel = null;
        //    GameObject keybindingPanel = null;

        //    if (modOptionsPanel == null)
        //    {
        //        SettingsPanelController[] objects = Resources.FindObjectsOfTypeAll<SettingsPanelController>();

        //        panelControllers = new GameObject[6];
        //        headerButtons = new GameObject[] { GameObject.Find("GenericHeaderButton (Gameplay)"), GameObject.Find("GenericHeaderButton (KB & M)"), GameObject.Find("GenericHeaderButton (Controller)"),
        //                                           GameObject.Find("GenericHeaderButton (Audio)"), GameObject.Find("GenericHeaderButton (Video)"), GameObject.Find("GenericHeaderButton (Graphics)")};

        //        for (int i = 0; i < objects.Length / 2; i++)
        //        {
        //            if (objects[i].name == "SettingsSubPanel, Audio")
        //            {
        //                audioPanel = objects[i].gameObject;
        //            }
        //            else if (objects[i].name == "SettingsSubPanel, Controls (M&KB)")
        //            {
        //                keybindingPanel = objects[i].gameObject;
        //            }

        //            panelControllers[i] = objects[i].gameObject;
        //        }

        //        if (audioPanel != null)
        //        {
        //            modOptionsPanel = UnityEngine.Object.Instantiate<GameObject>(audioPanel, audioPanel.transform.parent);

        //            modOptionsPanel.name = "SettingsSubPanel, Mod Options";

        //            verticalLayout = modOptionsPanel.transform.Find("Scroll View").Find("Viewport").Find("VerticalLayout").gameObject;

        //            var masterVolume = verticalLayout.transform.Find("SettingsEntryButton, Slider (Master Volume)").gameObject;

        //            SliderPrefab = UnityEngine.Object.Instantiate<GameObject>(masterVolume, masterVolume.transform.parent);
        //            SliderPrefab.SetActive(false);

        //            var audioFocus = verticalLayout.transform.Find("SettingsEntryButton, Bool (Audio Focus)").gameObject;

        //            BoolPrefab = UnityEngine.Object.Instantiate<GameObject>(audioFocus, audioFocus.transform.parent);
        //            BoolPrefab.SetActive(false);

        //            UnityEngine.Object.DestroyImmediate(verticalLayout.transform.Find("SettingsEntryButton, Slider (Master Volume)").gameObject);
        //            UnityEngine.Object.DestroyImmediate(verticalLayout.transform.Find("SettingsEntryButton, Slider (SFX Volume)").gameObject);
        //            UnityEngine.Object.DestroyImmediate(verticalLayout.transform.Find("SettingsEntryButton, Slider (MSX Volume)").gameObject);
        //            UnityEngine.Object.DestroyImmediate(verticalLayout.transform.Find("SettingsEntryButton, Bool (Audio Focus)").gameObject);

        //            var keybindButton = keybindingPanel.transform.Find("Scroll View").Find("Viewport").Find("VerticalLayout").Find("SettingsEntryButton, Binding (Jump)").gameObject;

        //            KeyBindingPrefab = UnityEngine.Object.Instantiate<GameObject>(keybindButton, keybindButton.transform.parent);
        //            KeyBindingPrefab.SetActive(false);

        //            modOptionsPanel.SetActive(false);

        //            foreach (var item in modOptions)
        //            {
        //                if (!subPanels.ContainsModPanel(item))
        //                {
        //                    GameObject subPanel = UnityEngine.Object.Instantiate<GameObject>(modOptionsPanel, modOptionsPanel.transform.parent);
        //                    ModPanel modPanel = new ModPanel(subPanel, item.ModGUID, item.ModGUID);

        //                    subPanel.SetActive(false);

        //                    subPanels.Add(modPanel);
        //                }
        //            }

        //            foreach (var item in subPanels)
        //            {
        //                if (item.panel == null)
        //                {
        //                    item.panel = UnityEngine.Object.Instantiate<GameObject>(modOptionsPanel, modOptionsPanel.transform.parent);
        //                }

        //                if (EntryButton == null)
        //                {
        //                    EntryButton = UnityEngine.Object.Instantiate<GameObject>(BoolPrefab, BoolPrefab.transform.parent);

        //                    UnityEngine.Object.DestroyImmediate(EntryButton.GetComponentInChildren<CarouselController>());

        //                    UnityEngine.Object.DestroyImmediate(EntryButton.GetComponentInChildren<ButtonSkinController>());

        //                    UnityEngine.Object.DestroyImmediate(EntryButton.transform.Find("CarouselRect").GetComponentInChildren<UnityEngine.UI.Image>());

        //                    foreach (var button in EntryButton.transform.Find("CarouselRect").GetComponentsInChildren<HGButton>())
        //                    {
        //                        UnityEngine.Object.DestroyImmediate(button);
        //                    }

        //                    foreach (var component in EntryButton.transform.Find("CarouselRect").GetComponentsInChildren<Component>())
        //                    {
        //                        if (component.GetType() == typeof(UnityEngine.UI.Image))
        //                        {
        //                            UnityEngine.Object.DestroyImmediate(component);
        //                        }
        //                    }

        //                    EntryButton.GetComponent<HGButton>().interactable = true;
        //                    EntryButton.GetComponent<HGButton>().enabled = true;
        //                    EntryButton.GetComponent<HGButton>().disablePointerClick = false;
        //                    EntryButton.GetComponent<HGButton>().onClick.RemoveAllListeners();

        //                    item.longName = $"{StartingText}.PanelButton.{item.modGUID}";

        //                    UnityEngine.Object.DestroyImmediate(EntryButton.GetComponent<LanguageTextMeshController>());

        //                    EntryButton.SetActive(false);
        //                }

        //                //var newEntryButton = UnityEngine.Object.Instantiate<GameObject>(EntryButton, EntryButton.transform.parent);

        //                //newEntryButton.GetComponentInChildren<HGTextMeshProUGUI>().SetText($"{item.modName} Options");
        //                //newEntryButton.GetComponent<HGButton>().onClick.AddListener(delegate { loadModPanel(item); });
        //                //newEntryButton.GetComponent<HGButton>().hoverToken = $"{StartingText}.PanelButton.{item.modGUID}.PanelButtonDescription".ToUpper().Replace(" ", "_");

        //                //item.entryButton = newEntryButton;

        //                GameObject verticalLayout = item.panel.transform.Find("Scroll View").Find("Viewport").Find("VerticalLayout").gameObject;

        //                var newExitButton = UnityEngine.Object.Instantiate<GameObject>(EntryButton, verticalLayout.transform);

        //                newExitButton.GetComponentInChildren<HGTextMeshProUGUI>().SetText($"Back to Mod Options");
        //                newExitButton.GetComponent<HGButton>().onClick.AddListener(backToModPanel);
        //                newExitButton.GetComponent<HGButton>().hoverToken = $"{StartingText}.GenericBackButton.PanelButtonDescription".ToUpper().Replace(" ", "_");

        //                modLocals[$"{StartingText}.GenericBackButton.PanelButtonDescription".ToUpper().Replace(" ", "_")] = "Back To The Main Mod Options Panel";

        //                item.backButton = newExitButton;
        //            }
        //        }

        //    }
        //}

        //public static void InstOptions()
        //{
        //    foreach (var mo in modOptions)
        //    {
        //        GameObject newOption = null;

        //        ModPanel modPanel = subPanels.GetPanel(mo);

        //        GameObject verticalLayout = modPanel.panel.transform.Find("Scroll View").Find("Viewport").Find("VerticalLayout").gameObject;

        //        if (mo.optionType == ModOption.OptionType.Slider)
        //        {
        //            newOption = UnityEngine.Object.Instantiate<GameObject>(SliderPrefab, verticalLayout.transform);

        //            newOption.GetComponentInChildren<SettingsSlider>().settingName = mo.OptionToken;
        //            newOption.GetComponentInChildren<SettingsSlider>().nameToken = mo.NameToken;
        //            //newOption.GetComponentInChildren<SettingsSlider>().nameToken = mo.DescriptionToken.ToUpper().Replace(" ", "_");

        //            if (mo.onValueChangedFloat != null)
        //            {
        //                newOption.GetComponentInChildren<SettingsSlider>().slider.onValueChanged.AddListener(mo.onValueChangedFloat);
        //            }

        //            newOption.name = $"ModOptions, Slider ({mo.OptionToken})";
        //        }
        //        else if (mo.optionType == ModOption.OptionType.Bool)
        //        {
        //            newOption = UnityEngine.Object.Instantiate<GameObject>(BoolPrefab, verticalLayout.transform);

        //            newOption.GetComponentInChildren<CarouselController>().settingName = mo.OptionToken;
        //            newOption.GetComponentInChildren<CarouselController>().nameToken = mo.NameToken;

        //            if (mo.onValueChangedBool != null)
        //            {
        //                newOption.AddComponent<BoolListener>().onValueChangedBool = mo.onValueChangedBool;
        //            }

        //            newOption.name = $"ModOptions, Bool ({mo.OptionToken})";
        //        }
        //        else if (mo.optionType == ModOption.OptionType.Keybinding)
        //        {

        //        }

        //        newOption.GetComponentInChildren<HGButton>().hoverToken = mo.DescriptionToken;

        //        LanguageAPI.Add(mo.NameToken, mo.Name);
        //        LanguageAPI.Add(mo.DescriptionToken, mo.Description);

        //        mo.gameObject = newOption;
        //    }
        //}

        public static void RegisterOption(RiskOfOption mo)
        {
            if (mo.optionType == RiskOfOption.OptionType.Slider)
            {
                mo.conVar = new FloatConVar(mo.ConsoleToken, RoR2.ConVarFlags.Archive, mo.defaultValue, mo.Description);
            }
            else if (mo.optionType == RiskOfOption.OptionType.Bool)
            {
                mo.conVar = new BoolConVar(mo.ConsoleToken, RoR2.ConVarFlags.Archive, mo.defaultValue, mo.Description);
            }
            else if (mo.optionType == RiskOfOption.OptionType.Keybinding)
            {
                mo.conVar = new KeyConVar(mo.ConsoleToken, RoR2.ConVarFlags.Archive, mo.defaultValue, mo.Description);
            }

            optionContainers.Add(ref mo);
        }

        public static void addOption(string Name, string Description, bool DefaultValue, string CategoryName = "")
        {
            string ModGUID = "";
            string ModName = "";

            var classes = Assembly.GetCallingAssembly().GetExportedTypes();

            foreach (var item in classes)
            {
                BepInPlugin bepInPlugin = item.GetCustomAttribute<BepInPlugin>();

                if (bepInPlugin != null)
                {
                    ModGUID = bepInPlugin.GUID;
                    ModName = bepInPlugin.Name;
                }
            }

            RegisterOption(new RiskOfOption(ModGUID, ModName, RiskOfOption.OptionType.Bool, Name, Description, DefaultValue.ToString(), CategoryName));
        }

        public static void addOption(string Name, string Description, float DefaultValue, string CategoryName = "")
        {
            string ModGUID = "";
            string ModName = "";

            var classes = Assembly.GetCallingAssembly().GetExportedTypes();

            foreach (var item in classes)
            {
                BepInPlugin bepInPlugin = item.GetCustomAttribute<BepInPlugin>();

                if (bepInPlugin != null)
                {
                    ModGUID = bepInPlugin.GUID;
                    ModName = bepInPlugin.Name;
                }
            }

            RegisterOption(new RiskOfOption(ModGUID, ModName, RiskOfOption.OptionType.Slider, Name, Description, DefaultValue.ToString(), CategoryName));
        }

        public static void addOption(string Name, string Description, KeyCode DefaultValue, string CategoryName = "")
        {
            string ModGUID = "";
            string ModName = "";

            var classes = Assembly.GetCallingAssembly().GetExportedTypes();

            foreach (var item in classes)
            {
                BepInPlugin bepInPlugin = item.GetCustomAttribute<BepInPlugin>();

                if (bepInPlugin != null)
                {
                    ModGUID = bepInPlugin.GUID;
                    ModName = bepInPlugin.Name;
                }
            }

            RegisterOption(new RiskOfOption(ModGUID, ModName, RiskOfOption.OptionType.Keybinding, Name, Description, $"{(int)DefaultValue}", CategoryName));
        }

        [Obsolete("ModOptions are handled internally now. Please use the other constructors.", false)]
        public static void addOption(ModOption mo)
        {
            Debug.Log($"Legacy ModOption {mo.name} constructed, converting to RiskOfOption...");

            string ModGUID = "";
            string ModName = "";

            var classes = Assembly.GetCallingAssembly().GetExportedTypes();

            foreach (var item in classes)
            {
                BepInPlugin bepInPlugin = item.GetCustomAttribute<BepInPlugin>();

                if (bepInPlugin != null)
                {
                    ModGUID = bepInPlugin.GUID;
                    ModName = bepInPlugin.Name;
                }
            }

            RegisterOption(new RiskOfOption(ModGUID, ModName, (RiskOfOption.OptionType)mo.optionType, mo.name, mo.description, mo.defaultValue, ""));
        }

        public static void createCategory(string Name, string Description)
        {
            string ModGUID = "";
            string ModName = "";

            var classes = Assembly.GetCallingAssembly().GetExportedTypes();

            foreach (var item in classes)
            {
                BepInPlugin bepInPlugin = item.GetCustomAttribute<BepInPlugin>();

                if (bepInPlugin != null)
                {
                    ModGUID = bepInPlugin.GUID;
                    ModName = bepInPlugin.Name;
                }
            }

            if (!optionContainers.Contains(ModGUID))
            {
                optionContainers.Add(new OptionContainer(ModGUID));
            }

            for (int i = 0; i < optionContainers[optionContainers.GetContainerIndex(ModGUID)].GetCategoriesCached().Count; i++)
            {
                if (optionContainers[optionContainers.GetContainerIndex(ModGUID)].GetCategoriesCached()[i].Name == Name)
                {
                    Debug.Log($"Category {Name} already exists!, please make sure you aren't assigning a category before creating one, or you aren't creating the same category twice!", BepInEx.Logging.LogLevel.Warning);
                    return;
                }
            }

            OptionCategory newCategory = new OptionCategory(Name, ModGUID);

            newCategory.Description = Description;

            optionContainers[optionContainers.GetContainerIndex(ModGUID)].Add(ref newCategory);
        }

        #region ModOption Legacy Stuff

        [Obsolete("ModOption is obsolete, please use RiskOfOption instead.")]
        public static ModOption getOption(string Name)
        {
            var classes = Assembly.GetCallingAssembly().GetExportedTypes();

            string ModGUID = "";

            foreach (var item in classes)
            {
                BepInPlugin bepInPlugin = item.GetCustomAttribute<BepInPlugin>();

                if (bepInPlugin != null)
                {
                    ModGUID = bepInPlugin.GUID;
                    break;
                }
            }

            foreach (var item in optionContainers[optionContainers.GetContainerIndex(ModGUID)].GetModOptionsCached())
            {
                if (item.Name == Name)
                {
                    var temp = new ModOption((ModOption.OptionType)item.optionType, item.Name, item.Description, item.defaultValue);

                    temp.conVar = item.conVar;

                    return temp;
                }
            }

            Debug.Log($"ModOption {Name} not found!", BepInEx.Logging.LogLevel.Error);
            return null;
        }


        //[Obsolete("Use GetOption(Name, Category).GetBool() / .GetFloat() / .GetKeyCode() instead")]
        public static string getOptionValue(string name)
        {
            BaseConVar conVar = null;

            var classes = Assembly.GetCallingAssembly().GetExportedTypes();

            string ModGUID = "";

            foreach (var item in classes)
            {
                BepInPlugin bepInPlugin = item.GetCustomAttribute<BepInPlugin>();

                if (bepInPlugin != null)
                {
                    ModGUID = bepInPlugin.GUID;
                    break;
                }
            }

            if (ModGUID == "")
            {
                Debug.Log("GUID of calling mod not found!", BepInEx.Logging.LogLevel.Error);
                return "";
            }

            foreach (var item in optionContainers[optionContainers.GetContainerIndex(ModGUID)].GetModOptionsCached())
            {
                if (item.Name == name)
                {
                    //Debug.Log(item.ConsoleToken);
                    conVar = RoR2.Console.instance.FindConVar(item.ConsoleToken);
                    break;
                }
            }


            if (conVar == null)
            {
                Debug.Log($"Convar {name} not found in convars.", BepInEx.Logging.LogLevel.Error);
                return "";
            }

            return conVar.GetString();
        }


        //public static bool ContainsModOption(this List<ModOption> modOptions, ModOption modOption)
        //{
        //    foreach (var item in modOptions)
        //    {
        //        if (item == modOption)
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}

        //public static bool ContainsModOption(this ModPanel modPanel, ModOption modOption)
        //{
        //    var temp = subPanels.GetPanel(modOption);

        //    if (modPanel == temp)
        //    {
        //        return true;
        //    }
        //    return false;
        //}

        //public static bool ContainsModPanel(this List<ModPanel> modPanels, ModOption modOption)
        //{
        //    foreach (var item in modPanels)
        //    {
        //        if (item.modGUID == modOption.ModGUID)
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}

        //public static ModPanel GetPanel(this List<ModPanel> modPanels, ModOption modOption)
        //{
        //    foreach (var item in modPanels)
        //    {
        //        if (item.modGUID == modOption.ModGUID)
        //        {
        //            return item;
        //        }
        //    }

        //    Debug.Log($"Modpanel not found.", BepInEx.Logging.LogLevel.Warning);
        //    return null;
        //}
        #endregion
    }
}
