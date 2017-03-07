namespace MapEnums
{
	public enum terrainType
	{
		empty,
		floor,
		roof,
		softrock,
		looserock,
		hardrock,
		solidrock,
		water,
		lava
	}

	public enum buildingType
	{
		none,
		toolstore,
		unknown
	}

	public enum orientation
	{
		north,
		east,
		south,
		west
	}

	public enum buildingSection
	{
		building,
		path,
		empty,
		personTeleport,
		resourceCollect,
		landTeleport,
		waterTeleport,
		energyCollect,
		energyRecharge
	}
}
