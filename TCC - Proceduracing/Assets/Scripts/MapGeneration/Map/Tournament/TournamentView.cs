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

    [SerializeField] private Vector3 mapScale;
    [SerializeField] private float randomnessScale;

    private System.Random random;

    private void Start()
    {
        random = new System.Random(GlobalSeed.Instance.TournamentSeed);
        Tournament = TournamentData.Instance;
        DisplayMap();
    }

    public void DisplayMap()
    {
        System.Random prgn = new System.Random(GlobalSeed.Instance.TournamentSeed);
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
                var mapClickable = newRoom.GetComponent<MapClickable>();
                mapClickable.Init(room, MapIconType.BOSS);

                ChangeMapIconProperties(room, mapClickable);

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

                var mapClickable = newRoom.GetComponent<MapClickable>();
                mapClickable.Init(room, MapIconType.RACE);

                ChangeMapIconProperties(room, mapClickable);
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

    public void ChangeMapIconProperties(TournamentMap.Room room, MapClickable mapClickable)
    {
        if (TournamentData.Instance.CurrentRoom == null)
        {
            if (room.Floor == 0)
            {
                mapClickable.ChangeColor(MapIconColor.POSSIBLE);
                mapClickable.IsClickable = true;
            }
            else
            {
                mapClickable.ChangeColor(MapIconColor.LOCKED);
                mapClickable.IsClickable = false;
            }
            return;
        }
        else
        {
            if (TournamentData.Instance.CurrentRoom.NextRooms.Exists(r => r == room))
            {
                mapClickable.ChangeColor(MapIconColor.POSSIBLE);
                mapClickable.IsClickable = true;
                return;
            }

            mapClickable.IsClickable = false;

            if (TournamentData.Instance.CurrentRoom == room)
            {
                mapClickable.ChangeColor(MapIconColor.CURRENT);
                return;
            }

            if (TournamentData.Instance.PassedRooms.Exists(r => r == room))
            {
                mapClickable.ChangeColor(MapIconColor.DONE);
                return;
            }

            mapClickable.ChangeColor(MapIconColor.LOCKED);
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
