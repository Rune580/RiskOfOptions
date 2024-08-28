using System;
using RoR2;
using RoR2.UI;
using UnityEngine;
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
        
    public string token;
    public string descriptionToken;
    public LanguageTextMeshController nameLabel;
    public HGTextMeshProUGUI descriptionLabel;
        
    public ModOptionPanelController Mopc { get; internal set; }
    public string modGuid;
    public HGHeaderNavigationController navigationController;
    public Image modIcon;

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        SetDescription();
    }
        
    public override void Awake()
    {
        base.Awake();

        if (nameLabel)
            nameLabel.token = token;

        if (!modIcon)
            modIcon = transform.Find("Icon Area").Find("Mod Icon").gameObject.GetComponent<Image>();

        // Prefer prefabs over sprite icons
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

        Instantiate(ModSettingsManager.OptionCollection[modGuid].iconPrefab!, modIcon.transform.parent);
        
        modIcon.gameObject.SetActive(false);
    }

    private void SpriteIcon()
    {
        if (!modIcon)
            return;

        modIcon.sprite = ModSettingsManager.OptionCollection[modGuid].icon!;
    }

    public override void Start()
    {
        base.Start();

        if (nameLabel)
            nameLabel.token = token;

        if (!Mopc)
            Mopc = GetComponentInParent<ModOptionPanelController>();
            
        onClick.AddListener(delegate
        {
            navigationController.ChooseHeaderByButton(this);

            Mopc.LoadModOptionsFromOptionCollection(modGuid);
        });
    }

    private new void Update()
    {
        if (!eventSystem)
        {
            return;
        }
        if (!disableGamepadClick && eventSystem.player.GetButtonDown(14) && eventSystem.currentSelectedGameObject == gameObject)
        {
            InvokeClick();
        }
        if (defaultFallbackButton && eventSystem.currentInputSource == MPEventSystem.InputSource.Gamepad && eventSystem.currentSelectedGameObject == null && CanBeSelected())
        {
            Select();
        }
    }


    private void SetDescription()
    {
        if (!descriptionLabel || descriptionToken is null)
            return;

        string text = Language.currentLanguage.GetLocalizedStringByToken(descriptionToken);
        if (text == descriptionToken)
            text = "No description provided";

        descriptionLabel.text = text;
    }
}