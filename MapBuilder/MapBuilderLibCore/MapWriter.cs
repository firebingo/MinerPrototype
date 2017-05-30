using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using System.IO;

namespace MapBuilderLibCore
{
	public class WriteModel
	{
		public MapHeader header;
		public MapTile[,] mapArray;
		public MapBuildings buildings;

		public WriteModel(MapTile[,] ma, MapHeader mh, MapBuildings mb)
		{
			mapArray = ma;
			header = mh;
			buildings = mb;
		}
	}

	public class MapWriter
	{
		string mapInfo;

		public async Task<bool> serializeMap(string directory, string fileName, MapTile[,] mapArray, MapHeader header, MapBuildings buildings)
		{
			try
			{
				var toWrite = new WriteModel(mapArray, header, buildings);
				mapInfo = JsonConvert.SerializeObject(toWrite);
				//IFolder folder = await FileSystem.Current.GetFolderFromPathAsync(directory);
				//if (folder == null)
				//	return false;
				//IFile file = await folder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
				//if (file == null)
				//	return false;
				//await file.WriteAllTextAsync(mapInfo);
				File.WriteAllText($"{directory}{fileName}", mapInfo);
				return true;
			}
			catch (Exception e)
			{
				return false;
			}
		}
	}
}
