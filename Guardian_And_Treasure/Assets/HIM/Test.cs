using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

/// <summary>
/// 线程测试方案
/// </summary>
public class Test : MonoBehaviour
{
    Thread testThread;
    private Queue<List<int>> sendOutQueue = new Queue<List<int>>();
    object syncRoot = new object();
    object syncRoot2 = new object();
    public int Total = 0;
    public bool Action = false;
    void Start()
    {
        testThread = new Thread(TestUpdate);
        testThread.Start();
    }
    void TestUpdate()
    {
        for (int i = 0; i < 100; i++)
        {
            List<int> sender = new List<int>();
            for (int count = 0; count < 1000000; count++)
            {
                sender.Add(count);
            }
            sendOutQueue.Enqueue(sender);
        }
    }
    // Update is called once per frame
    void LateUpdate()
    {
        if (sendOutQueue.Count > 0)
        {
            List<int> temp = sendOutQueue.Dequeue();
            int Count = 0;
            for (int i = 0; i < temp.Count; i++)
            {
                Count++;
            }
            Debug.Log(" 当前 -------------------------> " + Count);
            Total += Count;
            temp.Clear();
        }
    }
    private void OnApplicationQuit()
    {
        Debug.Log("final ----------------------> " + Total);
        
    }
}
