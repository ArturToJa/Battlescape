using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using BattlescapeLogic;
using UnityEngine.UI;
using BattlescapeUI;

public class DeleteSaves : MonoBehaviour
{
    [SerializeField] AMScreen_ArmyChoice screen;
    [SerializeField] GameObject Window;
    [SerializeField] Transform ExistingSaves;
    [SerializeField] Text questionText;
    public void CloseWindow()
    {
        UIManager.InstantlyTransitionActivity(Window, false);
    }

    public void Delete()
    {
        CloseWindow();
        SerializationManager.instance.Delete(Global.instance.armySavingManager.armySavePath + "/" + screen.chosenSaveName + "." + Global.instance.armySavingManager.saveExtension);
        foreach (Transform child in ExistingSaves)
        {
            if (child.gameObject.name == screen.chosenSaveName)
            {
                if (Application.isEditor)
                {
                    DestroyImmediate(child.gameObject);
                }
                else
                {
                    Destroy(child.gameObject);
                }
            }
        }
        Global.instance.armySavingManager.ResetCurrentSaveToNull();
        screen.chosenSaveName = null;
    }

    public void OpenWindow()
    {
        UIManager.InstantlyTransitionActivity(Window, true);
    }
}
