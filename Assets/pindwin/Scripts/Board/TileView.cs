using System;
using pindwin.Game;
using UnityEngine;

namespace pindwin.Board
{
	[RequireComponent(typeof(SpriteRenderer))]
	public class TileView : MonoBehaviour
	{
		[SerializeField] private Color _selectedColor;
		
		private SpriteRenderer _spriteRenderer;
		private Action<Tile> _clickTileCommand;
		private Tile _boardCoord;
		public Vector2Int BoardCoord;

		public bool Selected
		{
			set => _spriteRenderer.color = value ? _selectedColor : Color.white;
		}

		private void Awake()
		{
			_spriteRenderer = GetComponent<SpriteRenderer>();
		}

		public void Initialize(Tile boardCoord, Action<Tile> clickTileCommand)
		{
			_boardCoord = boardCoord;
			_clickTileCommand = clickTileCommand;
			BoardCoord = new Vector2Int(boardCoord.X, boardCoord.Y);
		}
		
		public void OnMouseDown()
		{
			_clickTileCommand?.Invoke(_boardCoord);
		}
	}
}