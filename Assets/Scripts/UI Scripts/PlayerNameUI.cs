using UnityEngine.UI;
using UnityEngine;

public class PlayerNameUI : MonoBehaviour
{
    [SerializeField] Text playerName;
    void Update()
    {
        playerName.text = PlayerPrefs.GetString("MyPlayerName");
    }
}
