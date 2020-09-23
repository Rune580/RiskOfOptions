using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RiskOfOptions
{
    public class ModPanel
    {
        public GameObject panel { get; private set; }

        public GameObject backButton;
        public string modGUID { get; private set; }
        public string modName { get; private set; }

        public string longName;

        public ModPanel(GameObject _panel, string _modGUID, string _modName)
        {
            panel = _panel;
            modGUID = _modGUID;
            modName = _modName;
        }

        public static bool operator ==(ModPanel a, ModPanel b)
        {
            if (a.modGUID == b.modGUID)
            {
                return true;
            }
            return false;
        }

        public static bool operator !=(ModPanel a, ModPanel b)
        {
            if (a.modGUID != b.modGUID)
            {
                return true;
            }
            return false;
        }
    }
}
