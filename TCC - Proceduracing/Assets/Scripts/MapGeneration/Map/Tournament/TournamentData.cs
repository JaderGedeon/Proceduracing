using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TournamentData : MonoBehaviour
{
    [SerializeField] private int floors;
    [SerializeField] private int roomsPerFloor;
    [SerializeField] private int seed;

    private TournamentMap tournamentMap;

    private List<TournamentMap.Room> RoomMap => tournamentMap.RoomMap;

    public TournamentData(int floors, int roomsPerFloor)
    {
        this.floors = floors;
        this.roomsPerFloor = roomsPerFloor;
    }

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        GlobalSeed.SetTournamentSeed(seed); // Provisório

        tournamentMap = new TournamentMap(floors, roomsPerFloor);
        tournamentMap.Init(GlobalSeed.TournamentSeed);
        tournamentMap.CreateFinalRoom();
    }

    private void OnDrawGizmos()
    {
        foreach (var room in RoomMap)
        {
            if (room.Floor == floors)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(new Vector3((roomsPerFloor - 1) / 2f, 1f, room.Floor + 1), new Vector3(1.3f, 1.3f, 1.3f)); // Boss
                continue;
            }

            Gizmos.color = room.NextRooms.Count > 0 ? Color.cyan : Color.red;

            if (room.Floor == 0)
                Gizmos.color = Color.yellow;

            Gizmos.DrawCube(new Vector3(room.PositionOnFloor, 1f, room.Floor), new Vector3(0.5f, 0.5f, 0.5f));

            foreach (var nextRoom in room.NextRooms)
            {
                if(nextRoom.Floor == floors)
                    Gizmos.DrawLine(new Vector3(room.PositionOnFloor, 1f, room.Floor), new Vector3((roomsPerFloor - 1) / 2f, 1f, nextRoom.Floor + 1)); // Boss
                else
                    Gizmos.DrawLine(new Vector3(room.PositionOnFloor, 1f, room.Floor), new Vector3(nextRoom.PositionOnFloor, 1f, nextRoom.Floor));
            }
        }
    }
}
