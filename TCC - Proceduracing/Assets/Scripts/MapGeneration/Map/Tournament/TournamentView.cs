using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TournamentView : MonoBehaviour
{
    private int currentFloor = 0;
    private TournamentMap.Room currentRoom;

    [SerializeField] private TournamentData Tournament;

    [SerializeField] private List<MapClickable> MapClickables;

    private List<GameObject> rooms;

    [SerializeField] private Vector3 mapScale;
    [SerializeField] private float randomnessScale;

    private System.Random random;

    private void Start()
    {
        rooms = new List<GameObject>();
        random = new System.Random(GlobalSeed.TournamentSeed);
    }

    public void DisplayMap()
    {
        System.Random prgn = new System.Random(GlobalSeed.TournamentSeed);

        foreach (var room in Tournament.RoomMap)
        {
            if (room.Floor == Tournament.Floors)
            {
                var pos = GetPositionWithRandomness((Tournament.RoomsPerFloor - 1) / 2f, 0, room.Floor + 1);

                var newRoom = Instantiate(
                    MapClickables[(int)MapIconType.BOSS].gameObject,
                    pos,
                    Quaternion.identity,
                    transform
                );
                rooms.Add(newRoom);
                continue;
            }

            if (room.NextRooms.Count > 0)
            {
                var pos = GetPositionWithRandomness(room.PositionOnFloor, 0, room.Floor);

                var newRoom = Instantiate(
                    MapClickables[(int)MapIconType.RACE].gameObject,
                    pos,
                    Quaternion.identity,
                    transform);
                rooms.Add(newRoom);
            }
            
            /*
            foreach (var nextRoom in room.NextRooms)
            {
                if (nextRoom.Floor == Tournament.Floors)

                    Gizmos.DrawLine(new Vector3(room.PositionOnFloor, 1f, room.Floor), new Vector3((Tournament.RoomsPerFloor - 1) / 2f, 1f, nextRoom.Floor + 1)); // Boss
                else
                    Gizmos.DrawLine(new Vector3(room.PositionOnFloor, 1f, room.Floor), new Vector3(nextRoom.PositionOnFloor, 1f, nextRoom.Floor));
            }
            */
        }
    }

    public Vector3 GetPositionWithRandomness(float x, float y, float z)
    {
        x += random.Next(-100, 100) * randomnessScale;
        //y += random.Next(-100, 100) * randomnessScale;
        //z += random.Next(-100, 100) * randomnessScale;

        return Vector3.Scale(new Vector3(x,y,z), mapScale);
    }

    public float MultiplyFloat(float a, float b) => a * b;
}
