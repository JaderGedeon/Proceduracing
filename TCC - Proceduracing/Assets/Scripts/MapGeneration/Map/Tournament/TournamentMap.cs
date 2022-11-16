using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TournamentMap
{
    private int _floors;
    private int _roomsPerFloor;

    public List<Room> RoomMap { get; private set; }

    private System.Random prgn;

    public TournamentMap(int floors, int roomsPerFloor)
    {
        _floors = floors;
        _roomsPerFloor = roomsPerFloor;
        RoomMap = new List<Room>();

        prgn = new System.Random(GlobalSeed.Instance.TournamentSeed);
    }

    public void Init()
    {
        for (int f = 0; f < _floors; f++)
        {
            for (int r = 0; r < _roomsPerFloor; r++)
            {
                RoomMap.Add(new Room(f, r));
            }
        }

        List<Room> GetRoomsAhead(Room room)
        {
            var roomsAhead = new List<Room>()
                { RoomMap[room.PositionOnFloor + _roomsPerFloor * (room.Floor + 1)] };

            if (room.PositionOnFloor != 0)
                if(!RoomMap[room.PositionOnFloor - 1 + _roomsPerFloor * room.Floor].NextRooms.Exists(_r => _r == roomsAhead[0]))
                    roomsAhead.Add(RoomMap[room.PositionOnFloor - 1 + _roomsPerFloor * (room.Floor + 1)]);

            if (room.PositionOnFloor != _roomsPerFloor - 1)
                if (!RoomMap[room.PositionOnFloor + 1 + _roomsPerFloor * room.Floor].NextRooms.Exists(_r => _r == roomsAhead[0]))
                    roomsAhead.Add(RoomMap[room.PositionOnFloor + 1 + _roomsPerFloor * (room.Floor + 1)]);

            return roomsAhead;
        }

        void PassNextRooms(Room room)
        {
            var nextRooms = GetRoomsAhead(room);

            var nextRoomsQnt = prgn.Next(1, nextRooms.Count);

            switch (prgn.Next(0, 100))
            {
                case int n when (n >= 90):
                    break;
                case int n when (n >= 75):
                    nextRoomsQnt = nextRoomsQnt > 2 ? nextRoomsQnt : 2;
                    break;
                default:
                    nextRoomsQnt = nextRoomsQnt > 1 ? nextRoomsQnt : 1;
                    break;
            }

            for (int i = 0; i < nextRoomsQnt; i++)
            {
                var possibleRooms = nextRooms.Except(room.NextRooms).ToList();
                if(possibleRooms.Count > 0)
                    room.AddNextRoom(possibleRooms[prgn.Next(0, possibleRooms.Count)]);
            }
        }
        
        void PassStartingPoints()
        {
            var startPoints = prgn.Next(2, 3);
            for (int i = 0; i < startPoints; i++)
            {
                while (true)
                {
                    var room = RoomMap[prgn.Next(0, _roomsPerFloor - 1)];

                    if (!room.NextRooms.Any())
                    {
                        PassNextRooms(room);
                        break;
                    }
                }
            }
        }
        PassStartingPoints();

        for (int f = 1; f < _floors - 1; f++)
        {
            for (int rf = 0; rf < _roomsPerFloor; rf++)
            {
                var room = RoomMap[rf + _roomsPerFloor * f];

                if (RoomMap.Exists(r => r.Floor == f - 1 && r.NextRooms.Exists(_r => _r == room)))                
                {
                    PassNextRooms(room);
                }
            }
        }
    }

    public void CreateFinalRoom()
    {
        var finalRoom = new Room(_floors, 0);

        for (int rf = 0; rf < _roomsPerFloor; rf++)
        {
            var room = RoomMap[rf + _roomsPerFloor * (_floors - 1)];

            if (RoomMap.Exists(r => r.Floor == _floors - 2 && r.NextRooms.Exists(_r => _r == room)))
            {
                room.AddNextRoom(finalRoom);
            }
        }

        RoomMap.Add(finalRoom);
    }

    public void PassSeeds()
    {
        foreach (var room in RoomMap)
        {
            room.Seed = prgn.Next(1, 999999);
        }

    }


    public class Room
    {
        public List<Room> PreviousRooms { get; private set; }
        public List<Room> NextRooms { get; private set; }

        public int Floor { get; private set; }
        public int PositionOnFloor { get; private set; }

        public int Seed { get; set; }

        public Room(int floor, int positionOnFloor)
        {
            PreviousRooms = new List<Room>();
            NextRooms = new List<Room>();
            Floor = floor;
            PositionOnFloor = positionOnFloor;
        }

        public void AddPreviousRoom(Room room)
        {
            PreviousRooms.Add(room);
        }

        public void AddNextRoom(Room room)
        {
            NextRooms.Add(room);
        }
    }
}