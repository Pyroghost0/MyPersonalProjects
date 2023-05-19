using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public static class Data
{
	//data[0] = level 1 completed?		data[1] = level 1 available?		data[2] = level 2...		data[12] = alt level 1 completed?		data[13] = alt level 1 available?		data[14] = alt level 2...
	public static void Save(short[,] terrain, short[] progress)
	{
		//save data
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + "/terrainData.gd");
		bf.Serialize(file, terrain);
		file.Close();
		file = File.Create(Application.persistentDataPath + "/progressData.gd");
		bf.Serialize(file, progress);
		file.Close();
	}
	
	public static void ClearData()
	{
		if (File.Exists(Application.persistentDataPath + "/terrainData.gd"))
		{
			File.Delete(Application.persistentDataPath + "/terrainData.gd");
			File.Delete(Application.persistentDataPath + "/progressData.gd");
		}
	}

	public static bool Exists()
    {
		return File.Exists(Application.persistentDataPath + "/terrainData.gd");
	}

	/*public static void FullUnlock()
	{

		bool[] data = Data.Load() == null ? new bool[24] : Data.Load();
		for (int i = 1; i < 24; i += 2)
		{
			data[i] = true;
		}
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + "/terrainData.gd");
		bf.Serialize(file, data);
		file.Close();
	}*/

	public static short[,] Terrain()
	{
		if (File.Exists(Application.persistentDataPath + "/terrainData.gd"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/terrainData.gd", FileMode.Open);
			short[,] data = (short[,])bf.Deserialize(file);
			file.Close();
			return data;
		}
		else
		{
			return null;
		}
	}

	public static short[] Progress()
	{
		if (File.Exists(Application.persistentDataPath + "/progressData.gd"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/progressData.gd", FileMode.Open);
			short[] data = (short[])bf.Deserialize(file);
			file.Close();
			return data;
		}
		else
		{
			return null;
		}
	}
}
