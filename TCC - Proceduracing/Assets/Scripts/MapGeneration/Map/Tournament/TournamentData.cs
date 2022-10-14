using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TournamentData : MonoBehaviour
{
    public static TournamentData Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }

    [SerializeField] private int floors;
    [SerializeField] private int roomsPerFloor;
    [SerializeField] private int seed;

    private int currentFloor;
    private TournamentMap.Room currentRoom;

    private List<TournamentMap.Room> passedRooms;

    private TournamentMap tournamentMap;

    public List<TournamentMap.Room> RoomMap => tournamentMap.RoomMap;
    public int Floors { get => floors; private set => floors = value; }
    public int RoomsPerFloor { get => roomsPerFloor; private set => roomsPerFloor = value; }
    public int CurrentFloor { get => currentFloor; set => currentFloor = value; }
    public TournamentMap.Room CurrentRoom { get => currentRoom; set => currentRoom = value; }
    public List<TournamentMap.Room> PassedRooms { get => passedRooms; set => passedRooms = value; }

    public TournamentData(int floors, int roomsPerFloor)
    {
        Floors = floors;
        RoomsPerFloor = roomsPerFloor;
    }

    public void Init()
    {
        CurrentFloor = 0;
        CurrentRoom = null;

        GlobalSeed.Instance.RaceType = RaceType.TOURNAMENT;
        tournamentMap = new TournamentMap(Floors, roomsPerFloor);
        tournamentMap.Init();
        tournamentMap.CreateFinalRoom();
        tournamentMap.PassSeeds();
        PassedRooms = new List<TournamentMap.Room>();
    }

    private void OnDrawGizmos()
    {
        foreach (var room in RoomMap)
        {
            if (room.Floor == Floors)
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
                if(nextRoom.Floor == Floors)
                    Gizmos.DrawLine(new Vector3(room.PositionOnFloor, 1f, room.Floor), new Vector3((roomsPerFloor - 1) / 2f, 1f, nextRoom.Floor + 1)); // Boss
                else
                    Gizmos.DrawLine(new Vector3(room.PositionOnFloor, 1f, room.Floor), new Vector3(nextRoom.PositionOnFloor, 1f, nextRoom.Floor));
            }
        }
    }
}
