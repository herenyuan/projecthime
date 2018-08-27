using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[ExecuteInEditMode]
public class LineArrage : MonoBehaviour {

	public enum Direction
	{
		Right,
		Left,
	}
	Direction direction = Direction.Right;
	public float Distance = 1;
	public Vector3 Offset = Vector3.zero;
	
	public bool MakingStart;
	void Start()
	{
		
	}
	
	void Update() 
	{
		if(MakingStart)
		{
			for(int i =0;i<transform.childCount;i++)
			{
				if(direction == Direction.Right)
				{
					transform.GetChild(i).localPosition = Vector3.right * Distance * i + Offset;
				}
				if(direction == Direction.Left)
				{
					transform.GetChild(i).localPosition = Vector3.left * Distance * i + Offset;
				}
				//transform.GetChild(i).name += i.ToString();
			}
			MakingStart = false;
		}
	}

}
