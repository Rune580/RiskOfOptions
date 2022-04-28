using System.Collections;
using System.Collections.Generic;
using RoR2.UI;
using RoR2.UI.SkinControllers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RiskOfOptions.Components.Panel
{
    public class CategoryScrollRect : ScrollRect
    {
        public int Categories
        {
            get => _categories;
            set
            {
                _pages = Mathf.CeilToInt(value / 4f);
                _categories = value;
                SetPage(0);
            }
        }

        public HGButton leftButton;
        public HGButton rightButton;
        public GameObject indicatorPrefab;
        public GameObject emptyPrefab;
        public Transform categoryTransform;
        public GameObject outline;

        public List<GameObject> categoryButtons = new();

        private GameObject _layoutGroup;
        private GameObject[] _indicators;
        
        private int _currentPage;
        private int _pages;
        private int _categories;
        
        private IEnumerator _pageAnimator;
        private IEnumerator _indicatorAnimator;
        private IEnumerator _colorAnimator;
        
        private bool _lateInit;

        public const int Spacing = -8;
        public const int DotScale = 25;

        private static readonly Color InactiveColor = new Color(0.3f, 0.3f, 0.3f, 1);

        public override void Start()
        {
            if (!_layoutGroup)
                _layoutGroup = transform.Find("Indicators").Find("LayoutGroup").gameObject;
            
            AddIndicators();
            RefreshButtons();
            StartIndicators();
            
            //UpdateOutline(0);
            SetPage(0);
        }

        public override void LateUpdate()
        {
            base.LateUpdate();
            
            if (_lateInit)
                return;
            
            //UpdateOutline(0);
            SetPage(0);
            _lateInit = true;
        }

        public void Reload()
        {
            if (_pageAnimator != null)
                StopCoroutine(_pageAnimator);
            
            _pageAnimator = AnimMove(0);
            StartCoroutine(_pageAnimator);
            
            if (!_layoutGroup || !indicatorPrefab || _indicators == null)
                return;
            
            AddIndicators();
            SetPage(0);
        }

        private void AddIndicators()
        {
            if (_indicators != null)
            {
                foreach (var indicator in _indicators)
                    DestroyImmediate(indicator);
            }
            
            _indicators = new GameObject[_pages];

            for (int i = 0; i < _pages; i++)
            {
                var indicator = Instantiate(indicatorPrefab, _layoutGroup.transform);

                var button = indicator.GetComponent<Button>();

                int page = i;
                button.onClick.AddListener(delegate
                {
                    SetPage(page);
                });
                
                indicator.SetActive(true);

                _indicators[i] = indicator;
            }
        }

        private void RefreshButtons()
        {
            leftButton.interactable = _currentPage - 1 >= 0;
            rightButton.interactable = _currentPage + 1 <= _pages - 1;
        }

        public void Previous()
        {
            if (_currentPage - 1 >= 0)
                SetPage(_currentPage - 1);
        }

        public void Next()
        {
            if (_currentPage + 1 <= _pages - 1)
                SetPage(_currentPage + 1);
        }

        private void StartIndicators()
        {
            for (int i = 0; i < _indicators.Length; i++)
            {
                var image = _indicators[i].GetComponent<Image>();

                image.color = i == 0 ? Color.white : InactiveColor;
            }
            
            //outline.SetActive(true);
            //UpdateOutline(0);
        }

        internal void SetPage(int page)
        {
            if (page >= _pages)
                return;
            
            _currentPage = page;

            if (_pageAnimator != null)
                StopCoroutine(_pageAnimator);

            if (_indicatorAnimator != null)
                StopCoroutine(_indicatorAnimator);

            if (_colorAnimator != null)
                StopCoroutine(_colorAnimator);

            _pageAnimator = AnimMove(page);
            StartCoroutine(_pageAnimator);

            _indicatorAnimator = SetIndicator(page);
            StartCoroutine(_indicatorAnimator);

            _colorAnimator = IndicatorColor(page);
            StartCoroutine(_colorAnimator);
            
            RefreshButtons();
        }

        private void UpdateOutline(int page)
        {
            var outlineTransform = outline.GetComponent<RectTransform>();
            var indicatorTransform = _indicators[page].GetComponent<RectTransform>();
            
            outline.transform.SetParent(_indicators[page].transform);
            outlineTransform.localPosition = Vector3.zero;
            
            outlineTransform.pivot = indicatorTransform.pivot;
            outlineTransform.sizeDelta = new Vector2(DotScale, DotScale);

            outlineTransform.position = indicatorTransform.position;
        }

        private IEnumerator AnimMove(int page)
        {
            float newPos = _pages == 1 ? page / 1f : page / ((float)_pages - 1);
            
            while (Mathf.Abs(horizontalNormalizedPosition - newPos) > 0.001f)
            {
                horizontalNormalizedPosition = Mathf.Lerp(horizontalNormalizedPosition, newPos, 6f * Time.unscaledDeltaTime);

                yield return new WaitForEndOfFrame();
            }

            horizontalNormalizedPosition = newPos;
        }

        private IEnumerator SetIndicator(int page)
        {
            if (page >= _indicators.Length)
                yield break;
            
            var image = _indicators[page].GetComponent<Image>();

            //var outlineTransform = outline.GetComponent<RectTransform>();
            //var indicatorTransform = _indicators[page].GetComponent<RectTransform>();

            //var sizeDelta = indicatorTransform.sizeDelta;
            //outlineTransform.sizeDelta = new Vector2(sizeDelta.x, sizeDelta.x);
            
            //var newPos = indicatorTransform.position;

            while (!ExtensionMethods.CloseEnough(image.color, Color.white) )
            {
                //outlineTransform.position = Vector2.Lerp(outlineTransform.position, newPos, 10f * Time.unscaledDeltaTime);
                image.color = Color.Lerp(image.color, Color.white, 10f * Time.unscaledDeltaTime);
                
                yield return new WaitForEndOfFrame();
            }
            
            //outlineTransform.position = newPos;
        }

        private IEnumerator IndicatorColor(int ignore)
        {
            Color[] colors = new Color[_indicators.Length];
            while (!ExtensionMethods.CloseEnough(colors, InactiveColor))
            {
                for (int i = 0; i < _indicators.Length; i++)
                {
                    if (i == ignore)
                        continue;

                    if (i >= colors.Length)
                        yield break;
                
                    var image = _indicators[i].GetComponent<Image>();

                    colors[i] = Color.Lerp(image.color, InactiveColor, 10f * Time.unscaledDeltaTime);
                    image.color = colors[i];
                }

                yield return new WaitForEndOfFrame();
            }
        }

        internal void FixExtra()
        {
            int extraButtons = _pages * 4 - _categories;

            while (extraButtons != 0)
            {
                GameObject blank = Instantiate(emptyPrefab, categoryTransform);
                
                blank.transform.SetAsLastSibling();
                blank.AddComponent<LayoutElement>().preferredWidth = 200;
                
                DestroyImmediate(blank.GetComponent<HGButton>());
                DestroyImmediate(blank.GetComponent<LanguageTextMeshController>());
                DestroyImmediate(blank.GetComponent<Image>());
                DestroyImmediate(blank.GetComponent<ButtonSkinController>());
                
                for (int i = 0; i < blank.transform.childCount; i++)
                    blank.transform.GetChild(i).gameObject.SetActive(false);
                
                categoryButtons.Add(blank);
                
                extraButtons--;
            }
        }

        public override void OnEnable()
        {
            base.OnEnable();
            
            AddIndicators();
            RefreshButtons();
            StartIndicators();
            
            //UpdateOutline(0);
            SetPage(0);
        }

        public override void OnDisable()
        {
            base.OnDisable();

            foreach (var indicator in _indicators)
                DestroyImmediate(indicator);
        }
        
        public override void OnBeginDrag(PointerEventData eventData)
        {

        }

        public override void OnDrag(PointerEventData eventData)
        {
            
        }

        public override void OnEndDrag(PointerEventData eventData)
        {

        }

        public override void OnScroll(PointerEventData eventData)
        {

        }
    }
}