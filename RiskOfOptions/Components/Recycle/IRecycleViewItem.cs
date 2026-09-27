using System;
using UnityEngine;

namespace RiskOfOptions.Components.Recycle;

public interface IRecycleViewItem<in TData> : IDisposable
{
    public int ConstraintIndex { get; set; }
    
    public RectTransform RectTransform { get; }

    public void BindData(TData data);
}