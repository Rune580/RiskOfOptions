using RoR2;
using UnityEngine;
using UnityEngine.Events;

namespace RiskOfOptions.Components.Options
{
    /// <summary>
    /// This component, when enabled, disables the games default behaviour when `Escape` is pressed.
    /// Also, if set, invokes an event when `Escape` is pressed.
    /// </summary>
    public class RooEscapeRouter : MonoBehaviour
    {
        public UnityEvent escapePressed = new();
        
        public void Awake()
        {
            DisablePause();
        }

        public void OnEnable()
        {
            DisablePause();
        }

        public void OnDisable()
        {
            EnablePause();
        }

        public void OnDestroy()
        {
            RoR2Application.unscaledTimeTimers.CreateTimer(0.1f, EnablePause);
        }

        private void OnGUI()
        {
            if (!Event.current.isKey || Event.current.type is not (EventType.KeyDown or EventType.KeyUp))
                return;

            if (Event.current.keyCode != KeyCode.Escape)
                return;
            
            escapePressed.Invoke();
        }

        private static void EnablePause()
        {
            ModSettingsManager.disablePause = false;
        }

        private static void DisablePause()
        {
            ModSettingsManager.disablePause = true;
        }
    }
}