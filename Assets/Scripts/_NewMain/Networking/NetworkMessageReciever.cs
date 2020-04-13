using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MessageCode
{

}

public class NetworkMessageReciever : MonoBehaviour
{
    private static Dictionary<MessageCode, List<InetworkMessageRecieverClient>> _networkMessageRecievers = new Dictionary<MessageCode, List<InetworkMessageRecieverClient>>();

    public static void RegisterReciver(InetworkMessageRecieverClient client, params MessageCode[] messageCodesInterested)
    {
        for (int i = 0; i < messageCodesInterested.Length; i++)
        {
            if (_networkMessageRecievers.TryGetValue(messageCodesInterested[i], out var list))
            {
                if (list != null)
                {
                    list.Add(client);
                }
            }
        }
    }

    public static void UnRegisterReciever(InetworkMessageRecieverClient clientToUnregister)
    {
        foreach (var reciever in _networkMessageRecievers)
        {
            for (int i = reciever.Value.Count - 1; i >= 0; i--)
            {
                if (reciever.Value[i] == clientToUnregister)
                {
                    reciever.Value.RemoveAt(i);
                    break;
                }
            }
        }
    }

    public static void UnregisterAllRecievers()
    {
        foreach (var reciever in _networkMessageRecievers)
        {
            reciever.Value.Clear();
        }
    }

    public void BroadCastMessageToClients(int messageIndex, object message)
    {
        if (_networkMessageRecievers.TryGetValue((MessageCode)messageIndex, out var list))
        {
            if (list != null)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    list[i].ConsumeNetworkData(messageIndex, message);
                }
            }
        }
    }
}
