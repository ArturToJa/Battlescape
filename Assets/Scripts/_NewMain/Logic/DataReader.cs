using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataReader
{
    public static CSVData Read(TextAsset inputData)
    {
        string[] data = inputData.text.Split(new char[] { '\n' });

        string[][] array = new string[data.Length - 2][];
        for (int i = 1; i < data.Length - 1; i++)
        {
            string[] row = data[i].Split(new char[] { ';' });
            row[row.Length - 1] = row[row.Length - 1].Trim();
            array[i - 1] = row;
        }
        Dictionary<string, int> names = new Dictionary<string, int>();

        string[] firstRow = data[0].Split(new char[] { ';' });
        firstRow[firstRow.Length - 1] = firstRow[firstRow.Length - 1].Trim();
        for (int i = 0; i < firstRow.Length; i++)
        {         
            names.Add(firstRow[i], i);
        }

        return new CSVData(array, names);
    }
}
