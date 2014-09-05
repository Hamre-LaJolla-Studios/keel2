using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Provides basic console variable (cvar) support
//
// Each client (or server) must maintain an instance of Console


public class Console {

	enum CvarType
	{
		INT = 0,
		STR = 1
	}
	
	private class Cvar
	{
		private string name_;
		
		public string name
		{
			get { return name_; }
			set { if (name_.CompareTo("") != 0) name_ = value; }
		}
		
		//virtual 
	}
	
	private class IntCvar : Cvar
	{
	
	}
	
	private class StrCvar : Cvar
	{
	
	}
	
	private List<Cvar> cvars;

	void RegisterCvar(string name, CvarType cvarType)
	{
		bool cvarExists = false;
	
		foreach (Cvar cvar in cvars)
		{
			if (cvar.name.CompareTo(name) != 0)
			{
				cvarExists = true;
				Debug.LogWarning("CVAR " + cvar + " already exists!");
			}
		}
		
		if (!cvarExists)
		{
			switch (cvarType)
			{
				case CvarType.INT:
					IntCvar newVar = new IntCvar();
					newVar.name = name;
				break;
			}
		}
	}

}
