﻿using System;

namespace RiskOfOptions.OptionConfigs
{
    public class BaseOptionConfig
    {
        /// <summary>
        /// When set, overrides the Name of the ConfigEntry
        /// </summary>
        public string name = "";
        
        /// <summary>
        /// When set, overrides the Description of the ConfigEntry
        /// </summary>
        public string description = "";
        
        /// <summary>
        /// When set, overrides the Category or Section of the ConfigEntry
        /// </summary>
        public string category = "";
        
        /// <summary>
        /// When true, the user will be notified that a restart is required if the ConfigEntry value gets changed.
        /// </summary>
        public bool restartRequired = false;
        
        /// <summary>
        /// Not yet implemented
        /// </summary>
        [Obsolete("Not yet implemented")]
        public bool hidden = false;

        public IsDisabledDelegate checkIfDisabled;

        public delegate bool IsDisabledDelegate();
    }
}