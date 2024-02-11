using System;
using UnityEngine;

namespace pindwin.Scripts.Board
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
			if (_isOdd)
			{
				x += 1;
			}
			
			return _tiles[x / 2];
		}
	}
}