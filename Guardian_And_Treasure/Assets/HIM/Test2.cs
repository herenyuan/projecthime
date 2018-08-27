using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test2 : MonoBehaviour {

    // Use this for initialization
    public bool Active = false;
    public bool Find = false;
	void Start () {

  
    }

    // Update is called once per frame
    void Update()
    {
        if (Active)
        {
            Active = false;
            for (int i = 0; i < 100000; i++)
            {
                int seed = Random.Range(0, 3);
                switch (seed)
                {
                    case 0:
                        Scene scene = new Scene();
                        break;
                    case 1:
                        Character character = new Character();
                        break;
                    case 2:
                        EventTrigger eventTrigger = new EventTrigger();
                        break;
                }
            }
        }
        if (Find)
        {
            Find = false;
            List<Scene> datas;
            HIMEDataBase.Get<Scene>(100000, out datas);
            Debug.Log("一共 ---------------------> " + datas.Count);
        }
    }
}
