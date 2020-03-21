using UnityEngine;
using UnityEditor;

//[CustomEditor(typeof(BattlescapeLogic.Unit))]
public class UnitEditor : Editor
{
	static int count = -1;
	public UnitEditor() : base()
	{
		BattlescapeLogic.Unit unit = (BattlescapeLogic.Unit)target;
		if (unit.unitTypeIndex == -1)
		{
			count++;
		}
	}
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		BattlescapeLogic.Unit unit = (BattlescapeLogic.Unit)target;
		if(unit.unitTypeIndex == -1)
		{
			unit.unitTypeIndex = count;
		}
	}
}
