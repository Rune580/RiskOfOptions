using System;
using RiskOfOptions.Components.Recycle;
using RiskOfOptions.Containers;
using RoR2.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RiskOfOptions.Components.Panel;

[RequireComponent(typeof(RectTransform))]
public class ModListButton : HGButton, IRecycleViewItem<OptionCollection>
{
    #region Legacy

    // Don't know if any mods rely on these, so I'm just marking them as obsolete for now.
        
    [Obsolete("No longer used")]
    public string description = "";

    #endregion
        
    public string token = "";
    public string descriptionToken = "";
    public LanguageTextMeshController nameLabel = null!;
    public string modGuid = "";
    public Image? modIcon;
    public Sprite fallbackIcon = null!;

    private GameObject? _prefabIcon = null!;

    public Action<string>? onSetModDescription;
    public Action<string, RectTransform>? onModSelected;

    public int ConstraintIndex { get; set; }

    public RectTransform RectTransform => (RectTransform)transform;

    private OptionCollection Collection => ModSettingsManager.OptionCollection[modGuid];

    public void BindData(OptionCollection collection)
    {
        descriptionToken = collection.DescriptionToken;
        token = collection.NameToken;
        modGuid = collection.ModGuid;
        
        UpdateState();
        
        Select();
    }

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        SetDescription();
    }

    private void UpdateState()
    {
        if (nameLabel)
            nameLabel.token = token;

        if (!modIcon)
            modIcon = transform.Find("Icon Area").Find("Mod Icon").gameObject.GetComponent<Image>();

        if (string.IsNullOrWhiteSpace(modGuid))
            return;
        
        if (_prefabIcon)
            DestroyImmediate(_prefabIcon);

        if (modIcon && modIcon!.sprite)
            modIcon.sprite = fallbackIcon;

        // Prefer prefabs to sprite icons
        if (Collection.iconPrefab is not null)
            PrefabIcon();

        if (Collection.icon is not null)
            SpriteIcon();
    }

    // Kind of hacky, but I'm lazy
    private void PrefabIcon()
    {
        if (!modIcon)
            return;
        
        if (string.IsNullOrWhiteSpace(modGuid))
            return;
        
        _prefabIcon = Instantiate(Collection.iconPrefab!, modIcon!.transform.parent);
        
        modIcon.gameObject.SetActive(false);
    }

    private void SpriteIcon()
    {
        if (!modIcon)
            return;
        
        if (string.IsNullOrWhiteSpace(modGuid))
            return;

        modIcon!.sprite = Collection.icon!;
        
        modIcon.gameObject.SetActive(true);
    }

    public override void Start()
    {
        base.Start();

        if (nameLabel)
            nameLabel.token = token;
            
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
        onModSelected?.Invoke(modGuid, RectTransform);
    }

    private void SetDescription()
    {
        onSetModDescription?.Invoke(descriptionToken);
    }

    public void Dispose()
    {
        DestroyImmediate(gameObject);
    }
}