using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RiskOfOptions.OptionComponents
{
    public class CategoryScrollRect : ScrollRect
    {
        private bool _forced = false;
        private float _forcedValue = 0f;

        public int pages = 0;

        //protected override void Awake()
        //{
        //    base.Awake();
        //    if (pages > 0)
        //    {
        //        Debug.Log("doing Awake header stuff");
        //        SetPos(0);
        //    }
        //}

        //protected void OnValidate()
        //{
        //    if (pages > 0)
        //    {
        //        Debug.Log("doing Validate header stuff");
        //        SetPos(0);
        //    }
        //}

        //protected override void Start()
        //{
        //    base.Start();

        //    if (pages > 0)
        //    {
        //        Debug.Log("doing Start header stuff");
        //        //ForceSet(0);
        //    }
        //}

        //protected override void OnEnable()
        //{
        //    base.OnEnable();
        //    if (pages > 0)
        //    {
        //        Debug.Log("doing OnEnable header stuff");
        //        SetPos(0);
        //    }
        //}

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

        protected override void LateUpdate()
        {
            if (!_forced)
            {
                base.LateUpdate();
            }
        }

        private void Update()
        {
            if (_forced)
            {
                base.horizontalNormalizedPosition = _forcedValue;
                base.verticalNormalizedPosition = 0;
            }
        }

        public void ForceSet(float value)
        {
            _forcedValue = value / pages;
            _forced = true;
        }

        public void Release()
        {
            _forced = false;
        }

        protected override void SetNormalizedPosition(float value, int axis)
        {
            Debug.Log($"value: {value}, axis: {axis}");
            base.SetNormalizedPosition(value, axis);
        }

        public void SetPos(float value)
        {
            value /= (pages - 1);

            Release();
            SetNormalizedPosition(value, 0);
        }
    }
}
