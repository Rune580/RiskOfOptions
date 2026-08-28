using System;
using RoR2;
using RoR2.UI;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RiskOfOptions.Components.Panel;

public class ModListButton : HGButton
{
    #region Legacy

    // Don't know if any mods rely on these, so I'm just marking them as obsolete for now.
        
    [Obsolete("No longer used")]
    public string description = "";

    #endregion
        
    public string token = "";
    public string descriptionToken = "";
    public LanguageTextMeshController? nameLabel;
    public HGTextMeshProUGUI? descriptionLabel;
        
    public ModOptionPanelController Mopc { get; internal set; }
    public string modGuid = "";
    // TODO: Replace with own component for navigation mod list. 
    public HGHeaderNavigationController? navigationController;
    public Image? modIcon;

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        SetDescription();
    }
        
    public override void Awake()
    {
        base.Awake();

        if (nameLabel)
            nameLabel!.token = token;

        if (!modIcon)
            modIcon = transform.Find("Icon Area").Find("Mod Icon").gameObject.GetComponent<Image>();

        if (string.IsNullOrWhiteSpace(modGuid))
            return;

        // Prefer prefabs to sprite icons
        if (ModSettingsManager.OptionCollection[modGuid].iconPrefab is not null)
            PrefabIcon();

        if (ModSettingsManager.OptionCollection[modGuid].icon is not null)
            SpriteIcon();
    }

    // Kind of hacky, but I'm lazy
    private void PrefabIcon()
    {
        if (!modIcon)
            return;
        
        if (string.IsNullOrWhiteSpace(modGuid))
            return;

        Instantiate(ModSettingsManager.OptionCollection[modGuid].iconPrefab!, modIcon!.transform.parent);
        
        modIcon.gameObject.SetActive(false);
    }

    private void SpriteIcon()
    {
        if (!modIcon)
            return;
        
        if (string.IsNullOrWhiteSpace(modGuid))
            return;

        modIcon!.sprite = ModSettingsManager.OptionCollection[modGuid].icon!;
    }

    public override void Start()
    {
        base.Start();

        if (nameLabel)
            nameLabel!.token = token;

        if (!Mopc)
            Mopc = GetComponentInParent<ModOptionPanelController>();
            
        onClick.AddListener(OnClick);
    }

    private new void Update()
    {
        if (!eventSystem)
            return;
        
        // TODO: Where the fuck did I get the actionId of 14 from??? 
        if (!disableGamepadClick && eventSystem.player.GetButtonDown(14) && eventSystem.currentSelectedGameObject == gameObject)
            InvokeClick();
        
        // TODO: What is this? What does this even do? It's not like Gamepads even work with RoO's UI, so why did I write this here?
        if (defaultFallbackButton && eventSystem.currentInputSource == MPEventSystem.InputSource.Gamepad && !eventSystem.currentSelectedGameObject && CanBeSelected())
            Select();
    }

    private void OnClick()
    {
        if (navigationController)
            navigationController!.ChooseHeaderByButton(this);
        
        if (string.IsNullOrWhiteSpace(modGuid))
            return;
        
        Mopc.LoadModOptionsFromOptionCollection(modGuid);
    }

    private void SetDescription()
    {
        if (!descriptionLabel || !string.IsNullOrWhiteSpace(descriptionToken))
            return;

        var text = Language.currentLanguage.GetLocalizedStringByToken(descriptionToken);
        if (text == descriptionToken)
            text = "No description provided";

        descriptionLabel!.text = text;
    }
}