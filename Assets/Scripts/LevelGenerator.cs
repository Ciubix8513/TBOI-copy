using System.Collections;

using System.Collections.Generic;
using UnityEngine;


public static class SeedSetting
{
    public static int seed = 0;
    public static int Depth = 1;
}
class QItem
{
    public int row;
    public int col;
    public int dist;
   public  QItem(int rowX, int colY, int w)       
    {
        row = rowX;
        col = colY;
        dist = w;
    }
};
enum RoomType
{
    Undefined, Normal,Starting, Boss, Item, Secret
}

class DistRoom : System.IComparable<int>, System.IComparable<DistRoom>
{
    public int Distance;
    public int Room;
    public int[,] pathData;
    public int CompareTo(DistRoom obj) => Distance.CompareTo(obj.Distance);
    int System.IComparable<int>.CompareTo(int other) => Distance.CompareTo(other);
}

public class LevelGenerator : MonoBehaviour
{
  
    

    [SerializeField]
    GameObject map;
    [SerializeField]
    GameObject rooms;

    [Header("Debug data")]
    [SerializeField]
    int[] floorplan;
    [SerializeField]
    GameObject room;
    [SerializeField]
    GameObject BossRoom;
    [SerializeField]
    GameObject ItemRoomO;
    [SerializeField]
    GameObject StartRoom;
    [Header("Generation settings")]
    [SerializeField]
    [Range(0,99)]
    int startRoomLocation = 45;
    [SerializeField]
    [Range(1,8)]
    int minDistance = 2;
    [SerializeField]
    [Range(1, 10)]
    int maxDistance = 3;


    [Header("Rooms")]
    [SerializeField]
    int roomSize = 9;
    [SerializeField]
    GameObject[] NormalRooms;
    [SerializeField]
    GameObject[] BossRooms;
    [SerializeField]
    GameObject[] ItemRooms;
    [SerializeField]
    GameObject[] StartingRooms;


    void Awake()
    {
        if (SeedSetting.seed != 0)
            GenerateLevel(SeedSetting.seed, SeedSetting.Depth);
        GenerateLevel(Random.Range(1, 1000000), SeedSetting.Depth);
        BuildFloor();
       

    }
    byte GetClosed(int i)  
    {
       
        var p = new Vector3Int(i % 10, 0, (i - (i % 10)) / 10);
        int o = 0;
        if (p.z - 1 >= 0 && //N
           floorplan[p.x + (p.z - 1)* 10] != 0)
            o = o | 1;
        if (p.z + 1 < 10 && //S
            floorplan[p.x + (p.z +1)* 10] != 0)
            o = o | 2;
        if (p.x - 1 >= 0 && //W
            floorplan[p.x - 1+ p.z * 10] != 0)
            o = o | 8;       
        if (p.x + 1 < 10 && //E
            floorplan[p.x + 1 + p.z * 10] != 0)
            o = o | 4;
        return (byte)o;
    }
    private void BuildFloor()
    {
        
        for (int i = 0; i < floorplan.Length; i++)
        {
            if (floorplan[i] == 1)
            {
                //Map object
                var o = Instantiate(room,map.transform);
                o.transform.localPosition = new Vector3(i % 10, 0, (i - (i % 10)) / 10) / 5;
                //Room
                var r = Instantiate(NormalRooms[Random.Range(0, NormalRooms.Length)],rooms.transform);
                r.transform.localPosition = new Vector3(i % 10, 0, (i - (i % 10)) / 10) * roomSize;
                var rc = r.GetComponent<Room>();
                rc.Opened = GetClosed(i);
            }
            else if (floorplan[i] == 2)
            {
                var o = Instantiate(StartRoom, map.transform);
                o.transform.localPosition = new Vector3(i % 10, 0, (i - (i % 10)) / 10) / 5;
                var r = Instantiate(StartingRooms[Random.Range(0, StartingRooms.Length)], rooms.transform);
                r.transform.localPosition = new Vector3(i % 10, 0, (i - (i % 10)) / 10) * roomSize;
                var rc = r.GetComponent<Room>();
                rc.Opened = GetClosed(i);
            }
            else if (floorplan[i] == 3)
            {
                var o = Instantiate(BossRoom, map.transform);
                o.transform.localPosition = new Vector3(i % 10, 0, (i - (i % 10)) / 10) / 5;
                var r = Instantiate(BossRooms[Random.Range(0, BossRooms.Length)], rooms.transform);
                r.transform.localPosition = new Vector3(i % 10, 0, (i - (i % 10)) / 10) * roomSize;
                var rc = r.GetComponent<Room>();
                rc.Opened = GetClosed(i);
            }
            else if (floorplan[i] == 4)
            {
                var o = Instantiate(ItemRoomO, map.transform);
                o.transform.localPosition = new Vector3(i % 10, 0, (i - (i % 10)) / 10) / 5;
                var r = Instantiate(ItemRooms[Random.Range(0, ItemRooms.Length)], rooms.transform);
                r.transform.localPosition = new Vector3(i % 10, 0, (i - (i % 10)) / 10) * roomSize;
                var rc = r.GetComponent<Room>();
                rc.Opened = GetClosed(i);
            }
        }
        

    }
    bool GeneratePath(int[,] data)
    {
        int X, Y;
        X = startRoomLocation % 10;
        Y = (startRoomLocation - (startRoomLocation % 10)) / 10;
        int iter = 0;
    start:
        if (iter > 101)
            return false;
        // moving right
        if (X + 1 < 10 &&
            data[X + 1, Y] < data[X, Y])
        {
            X++;
            floorplan[X + Y * 10] = 1;
            iter++;
            goto start;
        }
        // moving up
        if (Y - 1 >= 0 &&
                data[X , Y - 1] < data[X, Y])
        {
            Y--;
            floorplan[X + Y * 10] = 1;
            iter++;
            goto start;
        }
        // moving left
        if (X - 1 >= 0 &&
            data[X - 1, Y] < data[X, Y])
        {
            X--;
            floorplan[X + Y * 10] = 1;
            iter++;
            goto start;
        }
        // moving down
        if (Y + 1 < 10 &&
            data[X , Y + 1] < data[X,Y])
        {
            Y++;
            floorplan[X + Y * 10] = 1;
            iter++;
            goto start;
        }        
        return true;
    } 

    //Code *cough* stollen *cough* inspired by code from  https://www.geeksforgeeks.org/shortest-distance-two-cells-matrix-grid/
    //FIXED! I was just being an idiot lol
    int Distance(int dst, int org, out int[,] data)
    {
        QItem source = new QItem(org % 10, (org - (org % 10)) / 10, 0);
        QItem dst1 = new QItem(dst % 10, (dst - (dst % 10)) / 10, 0);

        // To keep track of visited QItems. Marking
        // blocked cells as visited.
        int N = 10;
        int M = 10;
        var visited = new bool[10, 10];

        for (int i = 0; i < N; i++)
            for (int j = 0; j < M; j++)
                if (floorplan[i + j * 10] == 0 || floorplan[i + j * 10] == 2)
                    visited[i, j] = false;
                else
                    visited[i, j] = true;

        // applying BFS on matrix cells starting from source
        Queue<QItem> q = new Queue<QItem>();
        q.Enqueue(source);
        visited[source.row, source.col] = true;
        data = new int[10, 10];
        for (int i = 0; i < 10; i++)
            for (int j = 0; j < 10; j++)            
                data[i, j] = int.MaxValue;
            
        while (q.Count != 0)
        {
            QItem p = q.Dequeue();

            // Destination found;
            if (p.row == dst1.row && p.col == dst1.col)
            {
                //athData.Add(data);
                //data[p.row, p.col] = -1;
                return p.dist;

            }
            // moving up
            if (p.row - 1 >= 0 &&
                visited[p.row - 1, p.col] == false)
            {
                q.Enqueue(new QItem(p.row - 1, p.col, p.dist + 1));
                data[p.row - 1, p.col] = p.dist;
                visited[p.row - 1, p.col] = true;
            }

            // moving down
            if (p.row + 1 < N &&
                visited[p.row + 1, p.col] == false)
            {
                q.Enqueue(new QItem(p.row + 1, p.col, p.dist + 1));
                data[p.row + 1, p.col] = p.dist;
                visited[p.row + 1, p.col] = true;
            }

            // moving left
            if (p.col - 1 >= 0 &&
                visited[p.row, p.col - 1] == false)
            {
                q.Enqueue(new QItem(p.row, p.col - 1, p.dist + 1));
                data[p.row, p.col - 1] = p.dist;
                visited[p.row, p.col - 1] = true;
            }

            // moving right
            if (p.col + 1 < M &&
                visited[p.row, p.col + 1] == false)
            {
                q.Enqueue(new QItem(p.row, p.col + 1, p.dist + 1));
                data[p.row, p.col + 1] = p.dist;
                visited[p.row, p.col + 1] = true;
            }
        }

        return -1;
    }

    void GenerateLevel(int seed, int depth)
    {
        Random.InitState(seed);
        var Endrooms = new List<DistRoom>();

        floorplan = new int[100];
        for (int i = 0; i < floorplan.Length; i++) floorplan[i] = 0;
        floorplan[startRoomLocation] = 2;
        //Generate end rooms
        for (int i = 0; i < (depth == 1 ? 0 : 1) + 3; i++)
        {
            var room = Mathf.FloorToInt(Random.value * 100);
            if (room == startRoomLocation || floorplan[room] != 0) { i--; continue; }
            floorplan[room] = 1;
            DistRoom dist1 = new DistRoom();
            var dist = Distance(startRoomLocation, room, out dist1.pathData);
            if (dist < minDistance || dist > depth + maxDistance) { i--; floorplan[room] = 0; continue; } //Preventing inaccesable rooms and making the less clustered
            else
            {
                Debug.Log(dist);
                Endrooms.Add(dist1);
                Endrooms[i].Room = room;
                Endrooms[i].Distance = dist;
            }
        }
        Endrooms.Sort();
        //Set farthest room to be a boss room
        floorplan[Endrooms[Endrooms.Count - 1].Room] = 3;
        //Set second farthest room to be an item room
        floorplan[Endrooms[Endrooms.Count - 2].Room] = 4;
        //Make all other normal
        for (int i = Endrooms.Count - 3; i >= 0; i--)
            floorplan[Endrooms[i].Room] = 1;


        //Generate rooms in between
        for (int i = 0; i < Endrooms.Count; i++)
            GeneratePath(Endrooms[i].pathData);
        return;
    }
}