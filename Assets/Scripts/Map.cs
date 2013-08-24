using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Map : MonoBehaviour {
	
	public GameObject wallPrefab;
	public GameObject floorPrefab;
	
	MapHandler handler;
	
	GameObject allWalls;
	GameObject allFloors;
	GameObject minimap;
	GameObject minimapAllWalls;

	// Use this for initialization
	void Start () {
		allWalls = new GameObject("All Walls");
		Utilities.ResetGameObject(allWalls);
		allWalls.transform.parent = transform;
		
		allFloors = new GameObject("All Floors");
		Utilities.ResetGameObject(allFloors);
		allFloors.transform.parent = transform;
		
		minimap = new GameObject("Minimap");
		Utilities.ResetGameObject(minimap);
		minimap.transform.parent = transform;
		
		minimapAllWalls = new GameObject("All Walls");
		Utilities.ResetGameObject(minimapAllWalls);
		minimapAllWalls.transform.parent = minimap.transform.transform;
		
		handler = new MapHandler(200, 200, 45);
		
		StartCoroutine(SpawnMapRoutine());
	}
	
	GameObject CreateWall(int column, int row) {
		GameObject newGO = (GameObject)Instantiate(wallPrefab, Vector3.zero, Quaternion.identity);
		newGO.transform.localPosition = new Vector3(-handler.MapWidth / 2 + column, 0f, -handler.MapHeight / 2 + row);
		MeshRenderer renderer = newGO.GetComponentInChildren<MeshRenderer>();
		if (renderer != null) {
			renderer.transform.localScale = new Vector3(1f, (int)Random.Range(7, 13), 1f);
			renderer.transform.localPosition = new Vector3(0f, renderer.transform.localScale.y / 2, 0f);
		}
		else {
			Logger.LogWarning("MeshRenderer is null");
		}
		return newGO;
	}
	
	IEnumerator SpawnMapRoutine() {
		GameObject newGO;
		
		yield return StartCoroutine(GenerationRoutine(3));
		
		newGO = (GameObject)Instantiate(floorPrefab, Vector3.zero, Quaternion.identity);
		newGO.transform.localScale = new Vector3(handler.MapWidth, 1f, handler.MapHeight);
		newGO.transform.parent = allFloors.transform;
		
		for (int column = 0, row = 0; row < handler.MapHeight; ++row) {
			GameObject newRow = new GameObject("Row " + row);
			Utilities.ResetGameObject(newRow);
			newRow.transform.parent = allWalls.transform;
			
			for (column = 0; column < handler.MapWidth; ++column) {
				if (handler.Map[column, row] == MapHandler.TILE_WALL) {
					newGO = CreateWall(column, row);
					newGO.transform.parent = newRow.transform;
				}
			}
			
			newRow.AddComponent<CombineChildren>();
			yield return null;
			
			MeshCollider collider = newRow.AddComponent<MeshCollider>();
			MeshFilter filter = newRow.GetComponent<MeshFilter>();
			if (filter != null) {
				collider.sharedMesh = filter.mesh;
			}
			else {
				Logger.LogWarning("MeshFilter is null");
			}
			yield return null;
			
			newGO = (GameObject)Instantiate(newRow);
			Utilities.ResetGameObject(newGO);
			newGO.transform.parent = minimapAllWalls.transform;
			Utilities.SetLayerRecursive(newGO, LayerMask.NameToLayer("Minimap"));
			yield return null;
		}
	}
	
	IEnumerator GenerationRoutine(int generations = 1) {
		handler.RandomFillMap();
		
		yield return null;
		
		for (int i = 0; i < generations; ++i) {
			handler.MakeCaverns();
			yield return null;
		}
	}
	
	public class MapHandler { 
		public int[,] Map;
	 
		public int MapWidth { get; set; }
		public int MapHeight { get; set; }
		public int PercentAreWalls { get; set; }
		
		public static int TILE_WALL = 1;
		public static int TILE_FLOOR = 0;
	 
		public MapHandler() {
			MapWidth = 40;
			MapHeight = 21;
			PercentAreWalls = 40;
		}
		
		public MapHandler(int mapWidth, int mapHeight, int percentWalls = 40) {
			MapWidth = mapWidth;
			MapHeight = mapHeight;
			PercentAreWalls = percentWalls;
		}
	 
		public void MakeCaverns() {
			for (int column = 0, row = 0; row < MapHeight; ++row) {
				for (column = 0; column < MapWidth; ++column) {
					Map[column, row] = PlaceWallLogic(column, row);
				}
			}
		}
	 
		public int PlaceWallLogic(int x, int y) {
			int numWalls = GetAdjacentWalls(x, y, 1, 1);
	 
			if (Map[x, y] == 1) {
				if (numWalls >= 4) {
					return TILE_WALL;
				}
				if (numWalls < 2) {
					return TILE_FLOOR;
				}
			}
			else {
				if (numWalls >= 5) {
					return TILE_WALL;
				}
			}
			return TILE_FLOOR;
		}
	 
		public int GetAdjacentWalls(int x,int y,int scopeX,int scopeY)
		{
			int startX = x - scopeX;
			int startY = y - scopeY;
			int endX = x + scopeX;
			int endY = y + scopeY;
	 
			int iX = startX;
			int iY = startY;
	 
			int wallCounter = 0;
	 
			for(iY = startY; iY <= endY; iY++) {
				for(iX = startX; iX <= endX; iX++)
				{
					if(!(iX==x && iY==y))
					{
						if(IsWall(iX,iY))
						{
							wallCounter += 1;
						}
					}
				}
			}
			return wallCounter;
		}
	 
		bool IsWall(int x,int y)
		{
			// Consider out-of-bound a wall
			if( IsOutOfBounds(x,y) )
			{
				return true;
			}
	 
			if( Map[x,y]==TILE_WALL	 )
			{
				return true;
			}
	 
			if( Map[x,y]==TILE_FLOOR	 )
			{
				return false;
			}
			return false;
		}
	 
		bool IsOutOfBounds(int x, int y)
		{
			if( x<0 || y<0 )
			{
				return true;
			}
			else if( x>MapWidth-1 || y>MapHeight-1 )
			{
				return true;
			}
			return false;
		}
	 
		public void PrintMap()
		{
			Logger.Log(MapToString());
		}
	 
		string MapToString()
		{
			string returnString = string.Format("{0} {1} {2} {3} {4} {5} {6}", // Seperator between each element
			                                  "Width:",
			                                  MapWidth.ToString(),
			                                  "\tHeight:",
			                                  MapHeight.ToString(),
			                                  "\t% Walls:",
			                                  PercentAreWalls.ToString(),
			                                  "\n"
			                                 );
	 
			List<string> mapSymbols = new List<string>();
			mapSymbols.Add(".");
			mapSymbols.Add("#");
			mapSymbols.Add("+");
	 
			for(int column=0,row=0; row < MapHeight; row++ ) {
				for( column = 0; column < MapWidth; column++ )
				{
					returnString += mapSymbols[Map[column,row]];
				}
				returnString += "\n";
			}
			return returnString;
		}
	 
		public void BlankMap()
		{
			for(int column=0,row=0; row < MapHeight; row++) {
				for(column = 0; column < MapWidth; column++) {
					Map[column,row] = TILE_FLOOR;
				}
			}
		}
	 
		public void RandomFillMap()
		{
			// New, empty map
			Map = new int[MapWidth,MapHeight];
	 
			int mapMiddle = 0; // Temp variable
			for(int column=0,row=0; row < MapHeight; row++) {
				for(column = 0; column < MapWidth; column++)
				{
					// If coordinants lie on the the edge of the map (creates a border)
					if(column == 0)
					{
						Map[column,row] = TILE_WALL;
					}
					else if (row == 0)
					{
						Map[column,row] = TILE_WALL;
					}
					else if (column == MapWidth-1)
					{
						Map[column,row] = TILE_WALL;
					}
					else if (row == MapHeight-1)
					{
						Map[column,row] = TILE_WALL;
					}
					// Else, fill with a wall a random percent of the time
					else
					{
						mapMiddle = (MapHeight / 2);
	 
						if(row == mapMiddle)
						{
							Map[column,row] = TILE_FLOOR;
						}
						else
						{
							Map[column,row] = RandomPercent(PercentAreWalls);
						}
					}
				}
			}
		}
	 
		int RandomPercent(int percent)
		{
			if(percent>=Random.Range(1,101))
			{
				return TILE_WALL;
			}
			return TILE_FLOOR;
		}
	 
		public MapHandler(int mapWidth, int mapHeight, int[,] map, int percentWalls=40)
		{
			this.MapWidth = mapWidth;
			this.MapHeight = mapHeight;
			this.PercentAreWalls = percentWalls;
			this.Map = new int[this.MapWidth,this.MapHeight];
			this.Map = map;
		}
	}
}
