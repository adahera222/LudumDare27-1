using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Map : LazySingleton<Map> {
	
	public Transform thisTransform;
	public GameObject wallPrefab;
	public GameObject floorPrefab;
	
	public MapHandler handler;
	
	public GameObject allWalls;
	public GameObject allFloors;
	public GameObject minimap;
	public GameObject minimapAllWalls;

	void Awake() {
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
		
		// Floor
		GameObject newGO = (GameObject)Instantiate(floorPrefab, Vector3.zero, Quaternion.identity);
		newGO.transform.localScale = new Vector3(handler.MapWidth + 1, 1f, handler.MapHeight + 1);
		newGO.transform.parent = allFloors.transform;
		
		newGO = (GameObject)Instantiate(floorPrefab, Vector3.zero, Quaternion.identity);
		newGO.transform.localPosition = new Vector3(0f, 13f, 0f);
		newGO.transform.localScale = new Vector3(handler.MapWidth + 1, 1f, handler.MapHeight + 1);
		Utilities.SetLayerRecursive(newGO, LayerMask.NameToLayer("Hidden"));
		newGO.transform.parent = allFloors.transform;
		
		// Boundary walls
		MeshRenderer renderer;
		newGO = (GameObject)Instantiate(wallPrefab, Vector3.zero, Quaternion.identity);
		Utilities.ResetGameObject(newGO);
		newGO.transform.parent = thisTransform;
		newGO.transform.localPosition = new Vector3(0f, 0f, (-handler.MapHeight - 1) / 2);
		renderer = newGO.GetComponentInChildren<MeshRenderer>();
		if (renderer != null) {
			renderer.transform.localScale = new Vector3(handler.MapWidth + 1, 13f, 1f);
			renderer.transform.localPosition = new Vector3(0f, renderer.transform.localScale.y / 2, 0f);
		}
		else {
			Logger.LogWarning("MeshRenderer is null");
		}
		renderer.gameObject.AddComponent<BoxCollider>();
		
		newGO = (GameObject)Instantiate(wallPrefab, Vector3.zero, Quaternion.identity);
		Utilities.ResetGameObject(newGO);
		newGO.transform.parent = thisTransform;
		newGO.transform.localPosition = new Vector3(0f, 0f, (handler.MapHeight + 1) / 2);
		renderer = newGO.GetComponentInChildren<MeshRenderer>();
		if (renderer != null) {
			renderer.transform.localScale = new Vector3(handler.MapWidth + 1, 13f, 1f);
			renderer.transform.localPosition = new Vector3(0f, renderer.transform.localScale.y / 2, 0f);
		}
		else {
			Logger.LogWarning("MeshRenderer is null");
		}
		renderer.gameObject.AddComponent<BoxCollider>();
		
		newGO = (GameObject)Instantiate(wallPrefab, Vector3.zero, Quaternion.identity);
		Utilities.ResetGameObject(newGO);
		newGO.transform.parent = thisTransform;
		newGO.transform.localPosition = new Vector3((-handler.MapWidth - 1) / 2, 0f, 0f);
		renderer = newGO.GetComponentInChildren<MeshRenderer>();
		if (renderer != null) {
			renderer.transform.localScale = new Vector3(1f, 13f, handler.MapHeight + 1);
			renderer.transform.localPosition = new Vector3(0f, renderer.transform.localScale.y / 2, 0f);
		}
		else {
			Logger.LogWarning("MeshRenderer is null");
		}
		renderer.gameObject.AddComponent<BoxCollider>();
		
		newGO = (GameObject)Instantiate(wallPrefab, Vector3.zero, Quaternion.identity);
		Utilities.ResetGameObject(newGO);
		newGO.transform.parent = thisTransform;
		newGO.transform.localPosition = new Vector3((handler.MapWidth + 1) / 2, 0f, 0f);
		renderer = newGO.GetComponentInChildren<MeshRenderer>();
		if (renderer != null) {
			renderer.transform.localScale = new Vector3(1f, 13f, handler.MapHeight + 1);
			renderer.transform.localPosition = new Vector3(0f, renderer.transform.localScale.y / 2, 0f);
		}
		else {
			Logger.LogWarning("MeshRenderer is null");
		}
		renderer.gameObject.AddComponent<BoxCollider>();
	}
	
	GameObject CreateWall(int column, int row) {
		GameObject newGO = (GameObject)Instantiate(wallPrefab, Vector3.zero, Quaternion.identity);
		Utilities.ResetGameObject(newGO);
		newGO.transform.localPosition = new Vector3(-handler.MapWidth / 2 + column, 0f, handler.MapHeight / 2 - row);
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
	
	IEnumerator SpawnRowRoutine(int row) {
		GameObject newGO;
		GameObject newRow = new GameObject("Row " + row);
		Utilities.ResetGameObject(newRow);
		newRow.transform.parent = allWalls.transform;
		
		for (int column = 0; column < handler.MapWidth; ++column) {
			if (handler.Map[column, row] == MapHandler.TILE_WALL) {
				newGO = CreateWall(column, row);
				newGO.transform.parent = newRow.transform;
			}
		}
		
		Utilities.SetLayerRecursive(newRow, LayerMask.NameToLayer("Hidden"));
		
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
		
		// Minimap
		newGO = (GameObject)Instantiate(newRow);
		Utilities.ResetGameObject(newGO);
		newGO.transform.parent = minimapAllWalls.transform;
		Utilities.SetLayerRecursive(newGO, LayerMask.NameToLayer("Minimap"));
		MeshRenderer renderer = newGO.GetComponent<MeshRenderer>();
		if (renderer != null) {
			renderer.castShadows = false;
			renderer.receiveShadows = false;
		}
		collider = newGO.GetComponent<MeshCollider>();
		if (collider != null) {
			collider.enabled = false;
		}
		
		yield return null;
		
		Utilities.SetLayerRecursive(newRow, LayerMask.NameToLayer("Default"));
		
		newRow.transform.localPosition = new Vector3(0f, 10f, 0f);
		iTween.MoveTo(newRow, new Hashtable() {
			{ "position", Vector3.zero },
			{ "time", 1f },
			{ "easeType", iTween.EaseType.easeInExpo } });
	}
	
	public void DestroyRow(object row) {
		Transform t = allWalls.transform.FindChild("Row " + row);
		MeshFilter filter = t.GetComponent<MeshFilter>();
		if (filter != null) {
			Destroy(filter.mesh);
		}
		GameObject.Destroy(t.gameObject);
	}
	
	IEnumerator DespawnRowRoutine(int row) {
		Transform t = allWalls.transform.FindChild("Row " + row);
		if (t == null) {
			yield break;
		}
		GameObject GO = t.gameObject;
		iTween.MoveTo(GO, new Hashtable() {
			{ "position", new Vector3(0f, -20f, 0f) },
			{ "time", 1f },
			{ "easeType", iTween.EaseType.easeOutExpo },
			{ "onComplete", "DestroyRow" },
			{ "onCompleteTarget", gameObject },
			{ "onCompleteParams", row } });
		
		yield return null;
		
		GO = minimapAllWalls.transform.GetChild(0).gameObject;
		MeshFilter filter = GO.GetComponent<MeshFilter>();
		if (filter != null) {
			Destroy(filter.mesh);
		}
		GameObject.Destroy(GO);
		
		yield return null;
	}
	
	public void SpawnMap() {
		StartCoroutine(SpawnMapRoutine());
	}
	
	IEnumerator SpawnMapRoutine() {
		yield return StartCoroutine(GenerationRoutine(3));
		
		for (int row = 0; row < handler.MapHeight; ++row) {
			yield return StartCoroutine(SpawnRowRoutine(row));
		}
	}
	
	public void DespawnMap() {
		StartCoroutine(DespawnMapRoutine());
	}
	
	IEnumerator DespawnMapRoutine() {
		for (int row = 0; row < handler.MapHeight; ++row) {
			yield return StartCoroutine(DespawnRowRoutine(row));
		}
		System.GC.Collect();
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
	 
			for (iY = startY; iY <= endY; ++iY) {
				for (iX = startX; iX <= endX; ++iX) {
					if (!(iX == x && iY == y)) {
						if (IsWall(iX,iY)) {
							wallCounter += 1;
						}
					}
				}
			}
			return wallCounter;
		}
	 
		bool IsWall(int x,int y) {
			// Consider out-of-bound a wall
			if (IsOutOfBounds(x, y)) {
				return true;
			}
	 
			if (Map[x, y] == TILE_WALL) {
				return true;
			}
	 
			if (Map[x,y] == TILE_FLOOR) {
				return false;
			}
			return false;
		}
	 
		bool IsOutOfBounds(int x, int y) {
			if (x < 0 || y < 0) {
				return true;
			}
			else if (x > MapWidth - 1 || y > MapHeight - 1) {
				return true;
			}
			return false;
		}
	 
		public void PrintMap() {
			Logger.Log(MapToString());
		}
	 
		string MapToString() {
			string returnString = string.Format("{0} {1} {2} {3} {4} {5} {6}", // Seperator between each element
			                                  "Width:",
			                                  MapWidth.ToString(),
			                                  "\tHeight:",
			                                  MapHeight.ToString(),
			                                  "\t% Walls:",
			                                  PercentAreWalls.ToString(),
			                                  "\n");
	 
			List<string> mapSymbols = new List<string>();
			mapSymbols.Add("-");
			mapSymbols.Add("#");
			mapSymbols.Add("+");
	 
			for (int column = 0, row = 0; row < MapHeight; ++row) {
				for (column = 0; column < MapWidth; ++column) {
					returnString += mapSymbols[Map[column,row]];
				}
				returnString += "\n";
			}
			return returnString;
		}
	 
		public void BlankMap() {
			for (int column = 0, row = 0; row < MapHeight; ++row) {
				for (column = 0; column < MapWidth; ++column) {
					Map[column,row] = TILE_FLOOR;
				}
			}
		}
	 
		public void RandomFillMap() {
			// New, empty map
			Map = new int[MapWidth,MapHeight];
	 
			int mapMiddle = 0; // Temp variable
			for (int column = 0,row = 0; row < MapHeight; ++row) {
				for (column = 0; column < MapWidth; ++column)
				{
					// If coordinants lie on the the edge of the map (creates a border)
					if(column == 0) {
						Map[column,row] = TILE_WALL;
					}
					else if (row == 0) {
						Map[column,row] = TILE_WALL;
					}
					else if (column == MapWidth-1) {
						Map[column,row] = TILE_WALL;
					}
					else if (row == MapHeight-1) {
						Map[column,row] = TILE_WALL;
					}
					// Else, fill with a wall a random percent of the time
					else
					{
						mapMiddle = (MapHeight / 2);
	 
						if (row == mapMiddle) {
							Map[column,row] = TILE_FLOOR;
						}
						else {
							Map[column,row] = RandomPercent(PercentAreWalls);
						}
					}
				}
			}
		}
	 
		int RandomPercent(int percent) {
			if (percent >= Random.Range(1,101)) {
				return TILE_WALL;
			}
			return TILE_FLOOR;
		}
	 
		public MapHandler(int mapWidth, int mapHeight, int[,] map, int percentWalls = 40) {
			this.MapWidth = mapWidth;
			this.MapHeight = mapHeight;
			this.PercentAreWalls = percentWalls;
			this.Map = new int[this.MapWidth,this.MapHeight];
			this.Map = map;
		}
	}
}
