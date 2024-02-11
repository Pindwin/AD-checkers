using pindwin.Scripts.Game;
using UnityEngine;

namespace pindwin.Scripts.Board
{
	[RequireComponent(typeof(SpriteRenderer))]
	public class TileView : MonoBehaviour
	{
		[SerializeField] private Color _selectedColor;
		
		private SpriteRenderer _spriteRenderer;
		private ClickTileCommand _clickTileCommand;
		private Tile _boardCoord;

		public bool Selected
		{
			set => _spriteRenderer.color = value ? _selectedColor : Color.white;
		}

		private void Awake()
		{
			_spriteRenderer = GetComponent<SpriteRenderer>();
		}

		public void Initialize(Tile boardCoord, ClickTileCommand clickTileCommand)
		{
			_boardCoord = boardCoord;
			_clickTileCommand = clickTileCommand;
		}
		
		public void OnMouseDown()
		{
			_clickTileCommand?.Execute(_boardCoord);
		}
	}
}