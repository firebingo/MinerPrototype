﻿using MapEnums;
using System.Collections.Generic;

namespace MapBuilderLibCore
{
	public static class PredefBuildings
	{
		public static readonly Dictionary<string, BuildingTile[,]> preBuildings = new Dictionary<string, BuildingTile[,]>()
		{
			{
				/*
				 * [X]
				 */
				"Path",
				new BuildingTile[,]
				{
					//y
					{ new BuildingTile() { section = buildingSection.path, core = true } }
				}
			},
			{
				/*
				 * [x]
				 * [X]
				 */
				"Tool Store",
				new BuildingTile[,]
				{
					//y
					{ new BuildingTile() { section = buildingSection.personTeleport, core = false },
						new BuildingTile() { section = buildingSection.resourceCollect, core = true } }
				}
			},
			{
				/*
				 * [x][ ]
				 * [X][x]
				 */
				"Support Station",
				new BuildingTile[,]
				{
					//y
					{ new BuildingTile() { section = buildingSection.personTeleport, core = false },
						new BuildingTile() { section = buildingSection.building, core = true } }
				}
			},
			{
				/*
				 * [x][ ]
				 * [X][x]
				 */
				"Power Station",
				new BuildingTile[,]
				{
					//y
					{ new BuildingTile() { section = buildingSection.path, core = false },
						new BuildingTile() { section = buildingSection.energyCollect, core = true } },
					//x
					{ new BuildingTile() { section = buildingSection.empty, core = false },
						new BuildingTile() { section = buildingSection.building, core = false } }
				}
			}
		};
	}
}
