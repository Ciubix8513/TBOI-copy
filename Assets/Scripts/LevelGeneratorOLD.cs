using System.Collections.Generic;
using UnityEngine;



public class LevelGeneratorOLD : MonoBehaviour
{

    public int[] floorplan;
    int floorplanCount = 0;
    int maxrooms = 15;
    int minrooms = 7;
    List<int> endrooms;
    List<int> cellQueue;

    public int bossl;
    public int ItemRoom;
    public int Shop;
    public int SecretRoom;

    [SerializeField]
    GameObject room;
    [SerializeField]
    GameObject BossRoom;
    [SerializeField]
    GameObject ItemRoomO;
    [SerializeField]
    GameObject ShopRoom;
    [SerializeField]
    GameObject SecretRoomO;
     
    private void Start()
    {
        if (SeedSetting.seed != 0)
            GenerateLevel(SeedSetting.seed, SeedSetting.Depth);
        GenerateLevel(Random.Range(1, 1000000), SeedSetting.Depth);
        BuildFloor();
    }

    void GenerateLevel(int seed, int depth)
    {
        //Random.InitState(seed);        
        var placedSpecial = false;        
        floorplan = new int[101];
        for (int i = 0; i < floorplan.Length; i++) floorplan[i] = 0;
        cellQueue = new List<int>();
        endrooms = new List<int>();
        Visit(45);
       
        while (cellQueue.Count > 0 || !placedSpecial)
        {
            if (cellQueue.Count > 0)
            {
                var i = cellQueue[0];
                cellQueue.RemoveAt(0);
                var x = i % 10;
                var created = false;
                if (x > 1) created = created || Visit(i - 1);
                if (x > 9) created = created || Visit(i + 1);
                if (i > 20) created = created || Visit(i - 10);
                if (i> 70) created = created || Visit(i + 10);
                if (!created)
                {
                    endrooms.Add(i);
                    Debug.Log("Added an end room");
                }
            }
            else if (!placedSpecial)
            {
                if (floorplanCount < minrooms)
                {                 
                    Visit(45);
                    continue;
                }
                placedSpecial = true;
                bossl = endrooms[endrooms.Count - 1];
                Debug.Log("Trying to create bossroom" + endrooms.Count);
                endrooms.RemoveAt(endrooms.Count - 1);
                try
                {
                     ItemRoom = RandomEndRoom();
                     Shop = RandomEndRoom();
                     SecretRoom = pickSecret();                 
                }
                catch (System.Exception)
                {
                    placedSpecial = false;
                    Visit(45);
                    continue;
                }
            }            
        }
    }
    int pickSecret()
    {
        for (int i = 0; i < 900; i++)
        {
            var x = (int)Mathf.Floor(Random.value * 9) + 1;
            var y = (int)Mathf.Floor(Random.value * 9) + 1;
            var e = y * 10 + x;
            if (floorplan[e] == 1)
                continue;
            if (bossl == e - 1 || bossl == e + 1 || bossl == e + 10 || bossl == e - 10)
                continue;
            if (ncount(e) >= 3)
                return e;
            if (i > 300 && ncount(e) >= 2)
                return e;
            if (i > 600 && ncount(e) >= 1)
                return e;
        }
        return 99;
    }
    int RandomEndRoom()
    {
        if (endrooms.Count == 0)
        {
            Debug.LogError("No endrooms to pick from");
            return 0;
        }

        int index = (int)Mathf.Floor(Random.value * endrooms.Count);
        var i = endrooms[index];

        endrooms.RemoveAt(index);
        return i;
    }
    int ncount(int i) => floorplan[i - 10] + floorplan[i - 1] + floorplan[i + 1] + floorplan[i + 10];
    bool Visit(int i)
    {
        if (i != 45)
        {
            if (floorplan[i] != 0)
                return false;
            var neighbours = ncount(i);
            if (neighbours > 1)
                return false;
            if (floorplanCount >= maxrooms)
                return false;
            if (Random.value < 0.5f && i != 45)
                return false;
        }
        cellQueue.Add(1);
        floorplan[i] = 1;
        floorplanCount++;
        Debug.Log("New room is added");
        return true;
    }
    
    void BuildFloor() 
    {
        for (int i = 0; i < floorplan.Length; i++)
        {
            if (floorplan[i] != 0)
            {
                var o = Instantiate(room);
                o.transform.position = new Vector3(i % 10, 0, (i  - (i % 10)) / 10) / 10;
            }
        }
    }

}

