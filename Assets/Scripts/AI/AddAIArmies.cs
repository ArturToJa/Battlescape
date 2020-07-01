using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.UI;

public class AddAIArmies : MonoBehaviour
{
    [SerializeField] List<AIArmy> theAIArmies;
    PlayerArmy playerArmy;
    public string version;
    [SerializeField] Text versionText;

    private void Start()
    {
        CheckIfFirstTimeOnNewPCAndActAccordingly();
        versionText.text = PlayerPrefs.GetString("Version");
    }
    void CheckIfFirstTimeOnNewPCAndActAccordingly()
    {
        if (Directory.Exists(Application.persistentDataPath + "/Armies/AI") && PlayerPrefs.HasKey("Version") && PlayerPrefs.GetString("Version") == version)
        {
            return;
        }
        if (Directory.Exists(Application.persistentDataPath + "/Armies/AI"))
        {
            Directory.Delete(Application.persistentDataPath + "/Armies/AI", true);
        }
        Directory.CreateDirectory(Application.persistentDataPath + "/Armies/AI");

        BinaryFormatter bf = new BinaryFormatter();
        foreach (AIArmy army in theAIArmies)
        {
            if (Directory.Exists(Application.persistentDataPath + "/Armies/AI/" + army.Points.ToString() + "points") == false)
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/Armies/AI/" + army.Points.ToString() + "points");
            }
            FileStream file = File.Create(Application.persistentDataPath + "/Armies/AI/" + army.Points.ToString() + "points/" + army.Name + ".lemur");
            CreateArmyFromAIArmy(army);
            bf.Serialize(file, playerArmy);
            file.Close();
            PlayerPrefs.SetString("Version", version);
        }
    }

    private void CreateArmyFromAIArmy(AIArmy army)
    {
        playerArmy = new PlayerArmy
        {
            unitIndecies = new List<string>(),
            heroIndex = army.Hero.name,
            race = army.Race

        };
        foreach (var unit in army.Units)
        {
            playerArmy.unitIndecies.Add(unit.name);
        }
    }
}
