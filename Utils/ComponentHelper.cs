using UnityEngine;

namespace RiskOfOptions.Utils
{
    public static class ComponentHelper
    {
        public static void CopyComponentFrom<Tto,Tfrom>(this GameObject gameObject, Tfrom component) where Tto : MonoBehaviour where Tfrom : MonoBehaviour
        {
            Tto clone = gameObject.AddComponent<Tto>();

            foreach (var fieldInfo in component.GetType().GetFields())
            {
                var value = fieldInfo.GetValue(component);
                
                clone.GetType().GetField(fieldInfo.Name).SetValue(clone, value);
            }

            foreach (var propertyInfo in component.GetType().GetProperties())
            {
                var value = propertyInfo.GetValue(component)
            }
        }
    }
}