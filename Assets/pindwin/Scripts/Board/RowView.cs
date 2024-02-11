using System;
using pindwin.Game;
using UnityEditor;
using UnityEngine;
using IndexOutOfRangeException = System.IndexOutOfRangeException;

namespace pindwin.Board
{
	public class RowView : MonoBehaviour
	{
		[SerializeField] private bool _isOdd;
		[SerializeField] private TileView[] _tiles;
		
		[SerializeField] int _rowIndex;

		private void Awake()
		{
			Debug.Assert(_tiles.Length == 4);
			_rowIndex = transform.GetSiblingIndex();
			name = $"Row {_rowIndex}";
		}

		public TileView GetTileByBoardCoord(int x)
		{
			return _tiles[x / 2];
		}
		
		public void Initialize(Action<Tile> clickTileCommand)
		{
			for (int i = 0; i < _tiles.Length; i++)
			{
				try
				{
					_tiles[i].Initialize(new Tile(i * 2 + (_isOdd ? 1 : 0), _rowIndex), clickTileCommand);
				}
				catch (IndexOutOfRangeException)
				{
					throw;
				}
			}
		}
		
		[ContextMenu("Assign Tiles")]
		private void DoSomething()
		{
			_tiles = GetComponentsInChildren<TileView>();
			#if UNITY_EDITOR
			EditorUtility.SetDirty(this);
			#endif
		}
	}
}