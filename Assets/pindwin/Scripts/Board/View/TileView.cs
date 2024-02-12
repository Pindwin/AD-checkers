using System;
using UnityEngine;

namespace pindwin.Board.View
{
	[RequireComponent(typeof(SpriteRenderer))]
	public class TileView : MonoBehaviour
	{
		[SerializeField] private Color _selectedColor;
		
		private SpriteRenderer _spriteRenderer;
		private Action<Tile> _clickTileCommand;
		private Tile _boardCoord;

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
		}
		
		public void OnMouseDown()
		{
			_clickTileCommand?.Invoke(_boardCoord);
		}
	}
}