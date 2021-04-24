using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using RiskOfOptions.Resources;
using RoR2.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RiskOfOptions.OptionComponents
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
                
            }
        }

        private HGButton _leftButton;
        private HGButton _rightButton;

        private GameObject _indicatorHolder;
        private GameObject _indicatorDotPrefab;
        private GameObject _indicatorOutlinePrefab;

        private GameObject[] _indicators;

        private int _currentPage = 0;
        private int _pages = 0;
        
        private IEnumerator _pageAnimator;
        private IEnumerator _indicatorAnimator;
        private IEnumerator _colorAnimator;

        private const int Spacing = 4;
        private const int DotScale = 8;
        private const int OutlineScale = 12;

        private static readonly Color InActiveColor = new Color(0.3f, 0.3f, 0.3f, 1);

        //public float value = 0f;

        private int _categories;

        private bool Initialized => ButtonsInitialized && IndicatorsInitialized;
        
        private bool ButtonsInitialized => _leftButton && _rightButton;
        private bool IndicatorsInitialized => _indicatorHolder && _indicatorDotPrefab && _indicatorOutlinePrefab;

        public void Init()
        {
            if (!Initialized)
            {
                InitializeButtons();
                CreateIndicatorPrefabs();
            }
            
            AddIndicators();
            RefreshButtons();
            StartIndicators();
        }

        private void InitializeButtons()
        {
            _leftButton = transform.Find("Previous Category Page Button(Clone)").GetComponent<HGButton>();
            _rightButton = transform.Find("Next Category Page Button(Clone)").GetComponent<HGButton>();

            if (!ButtonsInitialized)
                return;

            _leftButton.disablePointerClick = false;
            _rightButton.disablePointerClick = false;

            _leftButton.onClick.RemoveAllListeners();
            _rightButton.onClick.RemoveAllListeners();

            _leftButton.onClick.AddListener(Previous);
            _rightButton.onClick.AddListener(Next);
        }

        private void CreateIndicatorPrefabs()
        {
            _indicatorHolder = new GameObject("Indicators", typeof(RectTransform), typeof(HorizontalLayoutGroup));
            _indicatorHolder.transform.SetParent(transform);

            var holderRectTransform = _indicatorHolder.GetComponent<RectTransform>();

            holderRectTransform.anchoredPosition = new Vector2(0, -36);
            holderRectTransform.sizeDelta = Vector2.zero;

            var horizontalLayoutGroup = _indicatorHolder.GetComponent<HorizontalLayoutGroup>();

            horizontalLayoutGroup.spacing = Spacing;

            _indicatorDotPrefab = new GameObject("Indicator Dot", typeof(RectTransform), typeof(LayoutElement), typeof(CanvasRenderer), typeof(Image));
            _indicatorDotPrefab.transform.SetParent(transform);
            _indicatorDotPrefab.SetActive(false);

            _indicatorDotPrefab.GetComponent<Image>().sprite = Assets.Load<Sprite>("assets/RiskOfOptions/IndicatorDot.png");

            var dotRectTransform = _indicatorDotPrefab.GetComponent<RectTransform>();
            
            dotRectTransform.pivot = Vector2.zero;

            var dotLayoutElement = _indicatorDotPrefab.GetComponent<LayoutElement>();

            dotLayoutElement.minWidth = DotScale;
            dotLayoutElement.minHeight = DotScale;

            _indicatorOutlinePrefab = new GameObject("Indicator Outline", typeof(RectTransform), typeof(LayoutElement), typeof(CanvasRenderer), typeof(Image));
            _indicatorOutlinePrefab.transform.SetParent(_indicatorHolder.transform);
            _indicatorOutlinePrefab.SetActive(true);
            
            _indicatorOutlinePrefab.GetComponent<Image>().sprite = Assets.Load<Sprite>("assets/RiskOfOptions/IndicatorOutline.png");

            var outlineRectTransform = _indicatorOutlinePrefab.GetComponent<RectTransform>();
            
            outlineRectTransform.pivot = Vector2.zero;
            outlineRectTransform.sizeDelta = new Vector2(OutlineScale, OutlineScale);

            var outlineLayoutElement = _indicatorOutlinePrefab.GetComponent<LayoutElement>();

            outlineLayoutElement.ignoreLayout = true;
        }

        private void AddIndicators()
        {
            float offset = 0;

            _indicators = new GameObject[_pages];

            for (int i = 0; i < _pages; i++)
            {
                var indicator = GameObject.Instantiate(_indicatorDotPrefab, _indicatorHolder.transform);
                
                indicator.SetActive(true);

                _indicators[i] = indicator;

                offset = (Spacing + DotScale) * i;
            }

            var holderRectTransform = _indicatorHolder.GetComponent<RectTransform>();

            holderRectTransform.anchoredPosition = new Vector2(-offset, holderRectTransform.anchoredPosition.y);
        }

        private void RefreshButtons()
        {
            _leftButton.interactable = _currentPage - 1 >= 0;
            _rightButton.interactable = _currentPage + 1 <= _pages - 1;
        }

        public void Previous()
        {
            if (_currentPage - 1 >= 0)
            {
                SetPage(_currentPage - 1);
            }
        }

        public void Next()
        {
            if (_currentPage + 1 <= _pages - 1)
            {
                SetPage(_currentPage + 1);
            }
        }

        private void StartIndicators()
        {
            _indicatorOutlinePrefab.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -8);
            _indicatorOutlinePrefab.transform.SetAsLastSibling();

            for (int i = 0; i < _indicators.Length; i++)
            {
                var image = _indicators[i].GetComponent<Image>();

                image.color = i == 0 ? Color.white : InActiveColor;
            }
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

        private void SetPage(int page)
        {
            _currentPage = page;

            if (_pageAnimator != null)
                StopCoroutine(_pageAnimator);

            if (_indicatorAnimator != null)
                StopCoroutine(_indicatorAnimator);

            if (_colorAnimator != null)
                StopCoroutine(_colorAnimator);

            _pageAnimator = AnimMove(_currentPage / ((float)_pages - 1));
            StartCoroutine(_pageAnimator);

            _indicatorAnimator = SetIndicator(page);
            StartCoroutine(_indicatorAnimator);

            _colorAnimator = IndicatorColor(page);
            StartCoroutine(_colorAnimator);
            
            RefreshButtons();
        }

        private IEnumerator AnimMove(float newPos)
        {
            double incrementPerAllPages = 1 / _pages - 1;
            double incrementPerActualPages = 1 / (((float) _categories / 4) - 1);
            
            double differenceBetweenPages = ExtensionMethods.Abs(incrementPerAllPages - incrementPerActualPages);

            double compensatedMax = ((incrementPerAllPages + differenceBetweenPages) * (_pages - 1)).RoundUpToDecimalPlace(3);
            
            //Debug.Log(compensatedMax);

            float remappedPos = newPos.Remap(0, 1, 0, (float) compensatedMax);
            
            while (Mathf.Abs(horizontalNormalizedPosition - remappedPos) > 0.000001f)
            {
                horizontalNormalizedPosition = Mathf.Lerp(horizontalNormalizedPosition, remappedPos, 6f * Time.deltaTime);

                yield return new WaitForEndOfFrame();
            }

            horizontalNormalizedPosition = remappedPos;
        }

        private IEnumerator SetIndicator(int page)
        {
            var image = _indicators[page].GetComponent<Image>();

            var indicatorRectTransform = _indicatorOutlinePrefab.GetComponent<RectTransform>();

            var newPos = new Vector2((Spacing + DotScale) * page, -8);

            while (!ExtensionMethods.CloseEnough(indicatorRectTransform.anchoredPosition, newPos) && !ExtensionMethods.CloseEnough(image.color, Color.white))
            {
                indicatorRectTransform.anchoredPosition = Vector2.Lerp(indicatorRectTransform.anchoredPosition, newPos, 10f * Time.deltaTime);
                image.color = Color.Lerp(image.color, Color.white, 10f * Time.deltaTime);
                
                yield return new WaitForEndOfFrame();
            }
            
            indicatorRectTransform.anchoredPosition = newPos;
        }

        private IEnumerator IndicatorColor(int ignore)
        {
            Color[] colors = new Color[_indicators.Length];
            while (!ExtensionMethods.CloseEnough(colors, InActiveColor))
            {
                
                for (int i = 0; i < _indicators.Length; i++)
                {
                    if (i == ignore)
                        continue;
                
                    var image = _indicators[i].GetComponent<Image>();

                    colors[i] = Color.Lerp(image.color, InActiveColor, 10f * Time.deltaTime);
                    image.color = colors[i];
                }

                yield return new WaitForEndOfFrame();
            }
        }
    }
}