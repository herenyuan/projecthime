using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class RenderCollection : MonoBehaviour
{
    public bool Check = false;
    public PlaneFormation formation;
    public GameObject Root;
    public void Start()
    {
        GameObject.DontDestroyOnLoad(this);
        //TEX2D = Resources.Load<Texture2D>("Src/effects/Princess_Stun/Princess_Stun_tex");
        //IMG.sprite = Sprite.Create(TEX2D, new Rect(0, 0, TEX2D.width, TEX2D.height), Vector2.zero);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            this.Load();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            this.Unload();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            this.ReloadScene2();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            this.ReloadScene3();
        }
    }
    public void Load()
    {
        GameObject[] originals = Resources.LoadAll<GameObject>("Prefab");
        for (int i = 0; i < originals.Length; i++)
        {
            GameObject clone = GameObject.Instantiate(originals[i]);
            clone.transform.SetParent(Root.transform);
        }
        formation.MakingStart = true;
    }
    public void Unload()
    {
        for(int i = 0;i< formation.transform.childCount;i++)
        {
            GameObject temp = formation.transform.GetChild(i).gameObject;
            GameObject.Destroy(temp);
        }
      
        Resources.UnloadUnusedAssets();
    }
    
    public void ReloadScene2()
    {
        SceneManager.LoadScene("Test2");
        Resources.UnloadUnusedAssets();
    }
    public void ReloadScene3()
    {
        SceneManager.LoadScene("Test3");
        Resources.UnloadUnusedAssets();
    }
}
