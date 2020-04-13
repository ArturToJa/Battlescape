using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface InetworkMessageRecieverClient
{
    void ConsumeNetworkData(int messageCode, object data);
}
