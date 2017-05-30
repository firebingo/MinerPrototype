using MapBuilderLibCore;
using System.Threading.Tasks;

namespace MapBuilderWpf
{
	public class MapBuilderApp
	{
		public Map buildMap { get; private set; }
		int width;
		int height;

		public MapBuilderApp()
		{
		}

		public void initializeMap(int width, int height)
		{
			this.width = width;
			this.height = height;
			buildMap = new Map(width, height);
			buildMap.intializeBlankMap();
		}
		
		public async Task<bool> saveMap(string directory, string fileName)
		{
			return await buildMap.saveMap(directory, fileName);
		}
	}
}
