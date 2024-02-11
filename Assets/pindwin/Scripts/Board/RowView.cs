﻿using System;
using pindwin.Scripts.Game;
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
		
		public void Initialize(ClickTileCommand clickTileCommand)
		{
			for (int i = 0; i < _tiles.Length; i++)
			{
				_tiles[i].Initialize(new Tile(i * 2 + (_isOdd ? 1 : 0), _rowIndex), clickTileCommand);
			}
		}
	}
}