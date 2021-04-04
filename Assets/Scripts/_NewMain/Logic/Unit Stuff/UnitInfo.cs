using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitInfo
{
    [SerializeField] string _unitName;
    public string unitName
    {
        get
        {
            return _unitName;
        }
        private set
        {
            _unitName = value;
        }
    }
    [Multiline]
    [SerializeField] string _fluffText;
    public string fluffText
    {
        get
        {
            return _fluffText;
        }
        private set
        {
            _fluffText = value;
        }
    }
}
