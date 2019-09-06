using System;
using System.Collections;
using System.Collections.Generic;


#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

namespace UnityEngine.Tilemaps
{
	[Serializable]
	[CreateAssetMenu(fileName = "New Directional Terrain Tile", menuName = "Tiles/Directional Terrain Tile")]
	public class DirectionalTerrainTile : TileBase
	{
		[SerializeField]
		public Sprite[] m_Sprites;

		public override void RefreshTile(Vector3Int location, ITilemap tileMap)
		{
			for (int yd = -1; yd <= 1; yd++)
				for (int xd = -1; xd <= 1; xd++)
				{
					Vector3Int position = new Vector3Int(location.x + xd, location.y + yd, location.z);
					if (TileValue(tileMap, position))
						tileMap.RefreshTile(position);
				}
		}

		public override void GetTileData(Vector3Int location, ITilemap tileMap, ref TileData tileData)
		{
			UpdateTile(location, tileMap, ref tileData);
		}

		private void UpdateTile(Vector3Int location, ITilemap tileMap, ref TileData tileData)
		{
			tileData.transform = Matrix4x4.identity;
			tileData.color = Color.white;

			int mask = TileValue(tileMap, location + new Vector3Int(0, 1, 0)) ? 1 : 0;
			mask += TileValue(tileMap, location + new Vector3Int(1, 1, 0)) ? 2 : 0;
			mask += TileValue(tileMap, location + new Vector3Int(1, 0, 0)) ? 4 : 0;
			mask += TileValue(tileMap, location + new Vector3Int(1, -1, 0)) ? 8 : 0;
			mask += TileValue(tileMap, location + new Vector3Int(0, -1, 0)) ? 16 : 0;
			mask += TileValue(tileMap, location + new Vector3Int(-1, -1, 0)) ? 32 : 0;
			mask += TileValue(tileMap, location + new Vector3Int(-1, 0, 0)) ? 64 : 0;
			mask += TileValue(tileMap, location + new Vector3Int(-1, 1, 0)) ? 128 : 0;

			byte original = (byte)mask;
			if ((original | 254) < 255) { mask = mask & 125; }
			if ((original | 251) < 255) { mask = mask & 245; }
			if ((original | 239) < 255) { mask = mask & 215; }
			if ((original | 191) < 255) { mask = mask & 95; }

			int index = GetIndex((byte)mask);
			if (index >= 0 && index < m_Sprites.Length && TileValue(tileMap, location))
			{
				tileData.sprite = m_Sprites[index];
				tileData.transform = Matrix4x4.identity; //GetTransform((byte)mask);
				tileData.color = Color.white;
				tileData.flags = TileFlags.LockTransform | TileFlags.LockColor;
				tileData.colliderType = Tile.ColliderType.Sprite;
			}
		}

		private bool TileValue(ITilemap tileMap, Vector3Int position)
		{
			TileBase tile = tileMap.GetTile(position);
			return (tile != null && tile == this);
		}

		private int GetIndex(byte mask)
		{
			switch (mask)
			{
				case 0: return 0;
				case 16: return 1;
				case 64: return 2;
				case 1: return 3;
				case 4: return 4;
				case 5:
				case 7: return 5;
				case 20:
				case 28: return 6;
				case 80:
				case 112: return 8;
				case 65:
				case 193: return 7;
				case 84:
				case 92:
				case 116:
				case 124: return 9;
				case 21:
				case 23:
				case 29:
				case 31: return 10;
				case 69:
				case 197:
				case 71:
				case 199: return 11;
				case 81:
				case 113:
				case 209:
				case 241: return 12;
				// surounded 7
				case 223: return 13;
				case 127: return 14;
				case 247: return 15;
				case 253: return 16;

				// surounded 4
				case 85: 
				// surounded 5
				case 87:
				case 93:
				case 117:
				case 213:
				// surounded 6
				case 95:
				case 125:
				case 245:
				case 215:
				// surounded 6 diagonals
				case 119:
				case 221:
				// surounded 8
				case 255: return 17;
				
				case 17: return 18;
				case 68: return 19;
			}
			return -1;
		}
	}

#if UNITY_EDITOR
	[CustomEditor(typeof(DirectionalTerrainTile))]
	public class DirectionalTerrainTileEditor : Editor
	{
		private DirectionalTerrainTile tile { get { return (target as DirectionalTerrainTile); } }

		public void OnEnable()
		{
			if (tile.m_Sprites == null || tile.m_Sprites.Length != 20)
			{
				tile.m_Sprites = new Sprite[20];
				EditorUtility.SetDirty(tile);
			}
		}


		public override void OnInspectorGUI()
		{
			EditorGUILayout.LabelField("Place sprites shown based on the contents of the sprite.");
			EditorGUILayout.Space();

			float oldLabelWidth = EditorGUIUtility.labelWidth;
			EditorGUIUtility.labelWidth = 210;

			EditorGUI.BeginChangeCheck();
			tile.m_Sprites[0] = (Sprite) EditorGUILayout.ObjectField("Filled", tile.m_Sprites[0], typeof(Sprite), false, null);

			tile.m_Sprites[1] = (Sprite) EditorGUILayout.ObjectField("TopCap", tile.m_Sprites[1], typeof(Sprite), false, null);
			tile.m_Sprites[2] = (Sprite) EditorGUILayout.ObjectField("LeftCap", tile.m_Sprites[2], typeof(Sprite), false, null);
			tile.m_Sprites[3] = (Sprite) EditorGUILayout.ObjectField("BottomCap", tile.m_Sprites[3], typeof(Sprite), false, null);
			tile.m_Sprites[4] = (Sprite) EditorGUILayout.ObjectField("RightCap", tile.m_Sprites[4], typeof(Sprite), false, null);
			
			tile.m_Sprites[5] = (Sprite) EditorGUILayout.ObjectField("Bottom Left", tile.m_Sprites[5], typeof(Sprite), false, null);
			tile.m_Sprites[6] = (Sprite) EditorGUILayout.ObjectField("Top Left", tile.m_Sprites[6], typeof(Sprite), false, null);
			tile.m_Sprites[7] = (Sprite) EditorGUILayout.ObjectField("Bottom Right", tile.m_Sprites[7], typeof(Sprite), false, null);
			tile.m_Sprites[8] = (Sprite) EditorGUILayout.ObjectField("Top Right", tile.m_Sprites[8], typeof(Sprite), false, null);
			
			tile.m_Sprites[9] = (Sprite) EditorGUILayout.ObjectField("Empty Top", tile.m_Sprites[9], typeof(Sprite), false, null);
			tile.m_Sprites[10] = (Sprite) EditorGUILayout.ObjectField("Empty Left", tile.m_Sprites[10], typeof(Sprite), false, null);
			tile.m_Sprites[11] = (Sprite) EditorGUILayout.ObjectField("Empty Bottom", tile.m_Sprites[11], typeof(Sprite), false, null);
			tile.m_Sprites[12] = (Sprite) EditorGUILayout.ObjectField("Empty Right", tile.m_Sprites[12], typeof(Sprite), false, null);
			
			tile.m_Sprites[13] = (Sprite) EditorGUILayout.ObjectField("Bottom Left Corner", tile.m_Sprites[13], typeof(Sprite), false, null);
			tile.m_Sprites[14] = (Sprite) EditorGUILayout.ObjectField("Top Left Corner", tile.m_Sprites[14], typeof(Sprite), false, null);
			tile.m_Sprites[15] = (Sprite) EditorGUILayout.ObjectField("Bottom Right Corner", tile.m_Sprites[15], typeof(Sprite), false, null);
			tile.m_Sprites[16] = (Sprite) EditorGUILayout.ObjectField("Top Right Corner", tile.m_Sprites[16], typeof(Sprite), false, null);
			
			tile.m_Sprites[17] = (Sprite) EditorGUILayout.ObjectField("Empty", tile.m_Sprites[17], typeof(Sprite), false, null);
			
			tile.m_Sprites[18] = (Sprite) EditorGUILayout.ObjectField("Vertical", tile.m_Sprites[18], typeof(Sprite), false, null);
			tile.m_Sprites[19] = (Sprite) EditorGUILayout.ObjectField("Horizontal", tile.m_Sprites[19], typeof(Sprite), false, null);
			if (EditorGUI.EndChangeCheck())
				EditorUtility.SetDirty(tile);

			EditorGUIUtility.labelWidth = oldLabelWidth;
		}
	}
#endif
}
