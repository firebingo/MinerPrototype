using MapEnums;
using System.Collections.Generic;

namespace MapBuilderLibWindows
{
	public static class PredefBuildings
	{
		public static readonly Dictionary<buildingType, BuildingTile[,]> preBuildings = new Dictionary<buildingType, BuildingTile[,]>()
		{
			{
				/*
				 * [x]
				 * [X]
				 */
				buildingType.toolstore,
				new BuildingTile[,]
				{
					//y
					{ new BuildingTile() { section = buildingSection.personTeleport, core = false, relativePos = new Vector2<int>(0, 0) },
						new BuildingTile() { section = buildingSection.resourceCollect, core = true, relativePos = new Vector2<int>(1, 1) } }
				}
			}
		};
	}
}
