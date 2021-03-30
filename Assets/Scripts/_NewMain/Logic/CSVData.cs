using System.Collections.Generic;
using UnityEngine;


public struct CSVData
{
    public string[][] data;
    public Dictionary<string, int> names;

    public CSVData(string[][] _data, Dictionary<string, int> _names)
    {
        data = _data;
        names = _names;
    }

   public string[] GetRightRow(string name)
    {
        for (int i = 0; i < data.Length; i++)
        {            
            if (data[i][0] == name)
            {                
                return data[i];
            }
        }
        Debug.LogError("No data found for name: " + name);
        return null;
    }
}
