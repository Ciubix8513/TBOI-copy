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
   public  QItem(int x, int y, int w)       
    {
        row = x;
        col = y;
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
    public int CompareTo(DistRoom obj) => Distance.CompareTo(obj.Distance);
    int System.IComparable<int>.CompareTo(int other) => Distance.CompareTo(other);
}

public class LevelGenerator : MonoBehaviour
{
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
    [SerializeField]
    GameObject ShopRoom;
    [SerializeField]
    GameObject SecretRoomO;

    void Awake()
    {
        if (SeedSetting.seed != 0)
            GenerateLevel(SeedSetting.seed, SeedSetting.Depth);
        GenerateLevel(Random.Range(1, 1000000), SeedSetting.Depth);
        BuildFloor();
    }


    private void BuildFloor()
    {
        for (int i = 0; i < floorplan.Length; i++)
        {
            if (floorplan[i] == 1)
            {
                var o = Instantiate(room);
                o.transform.position = new Vector3(i % 10, 0, (i - (i % 10)) / 10) / 5;
            }
            else if (floorplan[i] == 2)
            {
                var o = Instantiate(StartRoom);
                o.transform.position = new Vector3(i % 10, 0, (i - (i % 10)) / 10) / 5;
            }
            else if (floorplan[i] == 3)
            {
                var o = Instantiate(BossRoom);
                o.transform.position = new Vector3(i % 10, 0, (i - (i % 10)) / 10) / 5;
            }
            else if (floorplan[i] == 4)
            {
                var o = Instantiate(ItemRoomO);
                o.transform.position = new Vector3(i % 10, 0, (i - (i % 10)) / 10) / 5;
            }
        }
    }
    int GeneratePath(int org, int dst)
    {
        return 0;
    }

    //Code *cough* stollen *cough* inspired by code from  https://www.geeksforgeeks.org/shortest-distance-two-cells-matrix-grid/
    int Distance(int dst, int org)
    {
        QItem source = new QItem(org % 10, org / 10, 0);

        // To keep track of visited QItems. Marking
        // blocked cells as visited.
        int N = 10;
        int M = 10;
        var visited = new bool[10, 10];

        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < M; j++)
            {
                if (floorplan[i + j * 10] == 0)
                    visited[i, j] = true;
                else
                    visited[i, j] = false;               
            }
        }

        // applying BFS on matrix cells starting from source
        Queue<QItem> q = new Queue<QItem>();
        q.Enqueue(source);
        visited[source.row, source.col] = true;
        while (q.Count != 0)
        {
            QItem p = q.Dequeue();

            // Destination found;
            if (p.row + p.col * 10 == dst)
            {

                return p.dist ;
            }
            // moving up
            if (p.row - 1 >= 0 &&
                visited[p.row - 1, p.col] == false)
            {
                q.Enqueue(new QItem(p.row - 1, p.col, p.dist + 1));
                visited[p.row - 1, p.col] = true;
            }

            // moving down
            if (p.row + 1 < N &&
                visited[p.row + 1, p.col] == false)
            {
                q.Enqueue(new QItem(p.row + 1, p.col, p.dist + 1));
                visited[p.row + 1, p.col] = true;
            }

            // moving left
            if (p.col - 1 >= 0 &&
                visited[p.row, p.col - 1] == false)
            {
                q.Enqueue(new QItem(p.row, p.col - 1, p.dist + 1));
                visited[p.row, p.col - 1] = true;
            }

            // moving right
            if (p.col + 1 < M &&
                visited[p.row, p.col + 1] == false)
            {
                q.Enqueue(new QItem(p.row, p.col + 1, p.dist + 1));
                visited[p.row, p.col + 1] = true;
            }
        }
        return -1;
    }

    void GenerateLevel(int seed, int depth)
    {
        Random.InitState(seed);
        var Endrooms = new List<DistRoom>();//;
                                                    
        floorplan = new int[100];
        for (int i = 0; i < floorplan.Length; i++) floorplan[i] = 0;
        floorplan[45] = 2;
        //Generate end rooms
        for (int i = 0; i < (depth == 1 ? 1 : 0) + 3; i++)
        {
            var room = Mathf.FloorToInt(Random.value * 100);
            if (room == 45 || floorplan[room] != 0 ) { i--; continue; }
            floorplan[room] = 10;
            //if (i == 0) floorplan[room] = 3; 
            //else if (i == 1) floorplan[room] = 4; 
            //else floorplan[room] = 1;
            var dist = Distance(45, room);
            Debug.Log(dist);
          //  if(dist == -1) { i--; continue; }
            Endrooms.Add(new DistRoom());
            Endrooms[i].Room = room;
            Endrooms[i].Distance = dist;
            
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
        //for (int i = 0; i < Endrooms.Length; i++)        
        //if (!GeneratePath(Endrooms[i], 45))
        return;
    }
}