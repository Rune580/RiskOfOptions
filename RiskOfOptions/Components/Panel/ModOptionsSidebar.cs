using RiskOfOptions.Components.Animation;
using RiskOfOptions.Containers;
using RiskOfOptions.Utils;
using UnityEngine;
using UnityEngine.EventSystems;
using Debug = RiskOfOptions.Utils.Debug;

namespace RiskOfOptions.Components.Panel;

[RequireComponent(typeof(RectTransform))]
public class ModOptionsSidebar : UIBehaviour
{
    public GameObject modListPanel = null!;
    public RectTransform modListViewTransform = null!;
    public RectTransform modListVerticalLayoutTransform = null!;
    
    public GameObject modSelectedPanel = null!;
    public RectTransform modSelectedTransform = null!;
    public ModListButton modButton = null!;
    public Rect targetModButtonRect;

    public GameObject categoryPanel = null!;
    public RectTransform categoryPanelTransform = null!;

    private Rect _selectedModRect;
    private Vector2 _selectedModPos;

    private RectTransform RectTransform => (RectTransform)transform;

    public View.ViewState CurrentView
    {
        get;
        set
        {
            var prevView = field;
            field = value;
            UpdateState(prevView);
        }
    } = View.ModList();

    public void UpdateState(View.ViewState prevView)
    {
        if (prevView is View.ModListView && CurrentView is View.ModSelectedView modSelected)
        {
            Canvas.ForceUpdateCanvases();
            
            var modListViewRect = RectTransform.GetChildInHierarchyRect(modListViewTransform);
            Debug.Info($"ModListViewRect: {modListViewRect}");

            // Set panel size and position to match mod list
            modSelectedTransform.anchoredPosition = modListViewRect.min;
            modSelectedTransform.sizeDelta = modListViewRect.size / RectTransform.lossyScale;
            
            // Copy data of selected mod list button
            modButton.BindData(modSelected.Mod);
            
            // Store position of selected mod list button
            _selectedModRect = modListVerticalLayoutTransform.GetChildInHierarchyRect(modSelected.Transform);

            _selectedModPos = modListVerticalLayoutTransform.anchoredPosition + modSelected.Transform.anchoredPosition;
            
            // Set starting position and size of copied mod list button.
            var modTransform = modButton.RectTransform;
            modTransform.anchoredPosition = _selectedModPos;
            modTransform.sizeDelta = _selectedModRect.size / modTransform.lossyScale;
            
            modSelectedPanel.SetActive(CurrentView is View.ModSelectedView);
            
            var animator = modButton.GetComponent<AnimateRectTransform>();

            var start = RectTransform.GetChildInHierarchyRect(modButton.RectTransform);
            animator.Animate(new Rect(_selectedModPos, start.size), targetModButtonRect);
        }
        else if (prevView is View.ModSelectedView && CurrentView is View.ModListView)
        {
            Canvas.ForceUpdateCanvases();
            
            var animator = modButton.GetComponent<AnimateRectTransform>();
            animator.Animate(targetModButtonRect, new Rect(_selectedModPos, _selectedModRect.size));
            
            modSelectedPanel.SetActive(CurrentView is View.ModSelectedView); // TODO animation events.
        }
        
        modListPanel.SetActive(CurrentView is View.ModListView);
    }

    public static class View
    {
        public abstract record ViewState;
        public record ModListView : ViewState;
        public record ModSelectedView(OptionCollection Mod, RectTransform Transform) : ViewState;

        public static ViewState ModList() => new ModListView();
        public static ViewState ModSelected(OptionCollection mod, RectTransform transform) => new ModSelectedView(mod, transform);
    }
}