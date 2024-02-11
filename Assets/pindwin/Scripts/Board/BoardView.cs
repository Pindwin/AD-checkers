﻿using UnityEngine;

namespace pindwin.Scripts.Board
{
	public class BoardView : MonoBehaviour
	{
		[SerializeField] private RowView[] _rows;

		private void Awake()
		{
			Debug.Assert(_rows.Length == 8);
		}
		
		public TileView GetTileByBoardCoord(int x, int y)
		{
			Debug.Assert(x >= 0 && x <= 7);
			Debug.Assert(y >= 0 && y <= 7);
			
			return _rows[y].GetTileByBoardCoord(x);
		}
	}
}