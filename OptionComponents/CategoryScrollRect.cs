using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
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

        private int _currentPage = 0;
        private int _pages = 0;
        private IEnumerator _animator;

        public float value = 0f;

        private int _categories;

        private bool Initialized => _leftButton && _rightButton;

        public void Init()
        {
            if (!Initialized)
                InitializeButtons();

            RefreshButtons();
        }

        private void InitializeButtons()
        {
            _leftButton = transform.Find("Previous Category Page Button(Clone)").GetComponent<HGButton>();
            _rightButton = transform.Find("Next Category Page Button(Clone)").GetComponent<HGButton>();

            if (!Initialized)
                return;

            _leftButton.disablePointerClick = false;
            _rightButton.disablePointerClick = false;

            _leftButton.onClick.RemoveAllListeners();
            _rightButton.onClick.RemoveAllListeners();

            _leftButton.onClick.AddListener(Previous);
            _rightButton.onClick.AddListener(Next);
        }

        private void CreateIndicator()
        {

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

        internal void SetPage(int page)
        {
            _currentPage = page;

            if (_animator != null)
                StopCoroutine(_animator);

            _animator = AnimMove(_currentPage / ((float)_pages - 1));
            StartCoroutine(_animator);

            RefreshButtons();
        }

        private IEnumerator AnimMove(float newPos)
        {
            float remainingCategories = Mathf.Abs(4 - ((_pages * 4) - (_categories)));

            float max = remainingCategories switch
            {
                3 => 0.145f,
                2 => 0.335f,
                1 => 0.6025f,
                _ => 0f
            };

            float remappedPos = newPos.Remap(0, 1, 0, 1 + value);

            float fistVisibleCategory = newPos.Remap(0, 1, 1, (_pages * 4) - remainingCategories);

            Debug.Log($"remapped: {remappedPos}, input: {newPos}, max: {max}, first visible: {fistVisibleCategory}");


            while (Mathf.Abs(horizontalNormalizedPosition - remappedPos) > 0.000001f)
            {
                horizontalNormalizedPosition = Mathf.Lerp(horizontalNormalizedPosition, remappedPos, 6f * Time.deltaTime);

                yield return new WaitForEndOfFrame();
            }

            horizontalNormalizedPosition = remappedPos;
        }
    }
}
