using pindwin.Board;
using UnityEngine;

namespace pindwin.Pawns
{
	[RequireComponent(typeof(SpriteRenderer))]
	public class PawnView : MonoBehaviour
	{
		[SerializeField] private Sprite _whiteSprite;
		[SerializeField] private Sprite _blackSprite;
		
		SpriteRenderer _spriteRenderer;

		private void Awake()
		{
			_spriteRenderer = GetComponent<SpriteRenderer>();
		}

		public void Refresh(Pawn pawn, BoardView boardView)
		{
			transform.position = boardView.GetTileByBoardCoord(pawn.Position.X, pawn.Position.Y).transform.position;
			_spriteRenderer.sprite = pawn.IsWhite ? _whiteSprite : _blackSprite;
			gameObject.SetActive(pawn.IsDead == false);
			//todo set queen sprite
		}
	}
}