using RoR2;
using RoR2.ConVar;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RiskOfOptions
{
    class KeyConVar : BaseConVar
    {
        public KeyCode value { get; protected set; }
        public KeyConVar(string name, ConVarFlags flags, string defaultValue, string helpText) : base(name, flags, defaultValue, helpText)
        {

        }

        public override string GetString()
        {
            return TextSerialization.ToStringInvariant((int)this.value);
        }

        public override void SetString(string newValue)
        {
            int value;
            if (TextSerialization.TryParseInvariant(newValue, out value))
            {
                this.value = (KeyCode)value;
            }
        }
    }
}
