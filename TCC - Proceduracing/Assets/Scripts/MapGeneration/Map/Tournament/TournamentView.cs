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

        ClockController.position = 0;
    }

    public void DisplayMap()
    {
        System.Random prgn = new System.Random(GlobalSeed.Instance.TournamentSeed);
        var mapClickables = new List<MapClickable>();

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

                mapClickables.Add(mapClickable);
                ChangeMapIconProperties(room, mapClickable);

                continue;
            }

            if (room.NextRooms.Count > 0)
            {
                var type = MapIconType.RACE;

                if(room.Floor > 0 && room.Floor < Tournament.Floors - 1)
                    if (prgn.Next(0, 100) > 70)
                        type = MapIconType.GEAR;

                var pos = GetPositionWithRandomness(room.PositionOnFloor, 0, room.Floor);

                var newRoom = Instantiate(
                    MapClickables[(int)type].gameObject,
                    pos,
                    Quaternion.identity,
                    transform);

                var mapClickable = newRoom.GetComponent<MapClickable>();
                mapClickable.Init(room, type);

                mapClickables.Add(mapClickable);
                ChangeMapIconProperties(room, mapClickable);
            }    
        }

        foreach (var mapClickable in mapClickables)
        {
            foreach (var nextRooms in mapClickable.room.NextRooms)
            {
                var nextRoom = mapClickables.Find(map => map.room == nextRooms);

                var line = CreateLineRenderer();

                var pos1 = mapClickable.transform.position;
                var pos2 = nextRoom.transform.position;

                pos1.y = -0.1f;
                pos2.y = -0.1f;

                line.SetPositions(new Vector3[] { pos1, pos2 });
            }
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

    public LineRenderer CreateLineRenderer()
    {
        var child = new GameObject();

        LineRenderer lineRenderer = child.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.useWorldSpace = true;
        lineRenderer.startColor = Color.gray;
        lineRenderer.endColor = Color.gray;

        return lineRenderer;
    }
}
