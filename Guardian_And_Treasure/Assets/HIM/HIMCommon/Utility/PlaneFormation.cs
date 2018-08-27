using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[ExecuteInEditMode]
public class PlaneFormation : MonoBehaviour {

	public Vector2 Size = new Vector2(10,10);
	public Vector3 Distance = Vector3.one;
	public Vector3 Offset = Vector3.zero;

	public bool MakingStart;
	void Start()
	{
		
	}
	void Update() 
	{
		if(MakingStart)
		{
			Vector3 Position = Vector3.zero; 
			int Index = 0;
			for(int Row = 0; Row<Size.x; Row++)
			{
				for(int Column = 0; Column<Size.y; Column++)
				{
					Index = Column + Row * (int)Size.y;
                    Position = new Vector3(Row * Distance.x, Distance.y, Column * Distance.z);
                    if(Index < transform.childCount)
                    {
                        Transform land = transform.GetChild(Index);
                        land.localPosition = Position + Offset;
                    }

				}
			}
			MakingStart = false;
		}
	}

}
