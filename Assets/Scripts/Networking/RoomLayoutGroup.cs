using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomLayoutGroup : MonoBehaviour
{
    [SerializeField]
    GameObject _roomPrefab;
    public GameObject RoomPrefab
    {
    get
        {
            return _roomPrefab;
        }
    }


    // code below is some fuckery from internet. I am not skilled enough to understand this bullshit, but it just works so i do not care.
    List<RoomOnTheList> _rooms = new List<RoomOnTheList>();
    List<RoomOnTheList> Rooms
    {
        get
        {
            return _rooms;
        }
    }

    void OnReceivedRoomListUpdate()
    {
        RoomInfo[] rooms = PhotonNetwork.GetRoomList();
        foreach (RoomInfo room in rooms)
        {
            RoomReceived(room);
        }
        RemoveOldRooms();
    }

    void RoomReceived(RoomInfo room)
    {
        int index = Rooms.FindIndex(x => x.RoomName == room.Name);
        if (index == -1)
        {
            if (room.IsVisible && room.PlayerCount < room.MaxPlayers)
            {
                GameObject r = Instantiate(RoomPrefab);
                r.transform.SetParent(transform, false);
                RoomOnTheList rotl = r.GetComponent<RoomOnTheList>();
                Rooms.Add(rotl);
                index = (Rooms.Count - 1);
                rotl.IsUpdated = true;
            }
        }
        if (index != -1)
        {            
            RoomOnTheList rotl = Rooms[index];
            rotl.SetRoomNameText(room.Name);
            rotl.IsUpdated = true;
        }
    }
    void RemoveOldRooms()
    {
        List<RoomOnTheList> removeRooms = new List<RoomOnTheList>();
        foreach (RoomOnTheList r in Rooms)
        {
            if (r.IsUpdated == false)
            {
                removeRooms.Add(r);
            }
            else
            {
                r.IsUpdated = false;
            }
        }
        foreach (RoomOnTheList r in removeRooms)
        {
            GameObject roomObject = r.gameObject;
            Debug.Log(r.GetComponent<RoomOnTheList>());
            Rooms.Remove(r);
            Destroy(roomObject);
        }
    }

    
}
