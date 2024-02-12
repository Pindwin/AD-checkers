using System;
using UnityEngine;

namespace pindwin.Board.View
{
	[RequireComponent(typeof(SpriteRenderer))]
	public class TileView : MonoBehaviour
	{
		[SerializeField] private Color _selectedColor;
		private Tile _boardCoord;
		private Action<Tile> _clickTileCommand;

		private SpriteRenderer _spriteRenderer;

		public bool Selected
		{
			set => _spriteRenderer.color = value ? _selectedColor : Color.white;
		}

		private void Awake()
		{
			_spriteRenderer = GetComponent<SpriteRenderer>();
		}

		public void OnMouseDown()
		{
			_clickTileCommand?.Invoke(_boardCoord);
		}

		public void Initialize(Tile boardCoord, Action<Tile> clickTileCommand)
		{
			_boardCoord = boardCoord;
			_clickTileCommand = clickTileCommand;
		}
	}
}