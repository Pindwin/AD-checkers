using System.Collections;
using pindwin.Board.View;
using UnityEngine;

namespace pindwin.Pawns
{
	[RequireComponent(typeof(SpriteRenderer))]
	public class PawnView : MonoBehaviour
	{
		[SerializeField] private Sprite _whiteSprite;
		[SerializeField] private Sprite _blackSprite;
		[SerializeField] private SpriteRenderer _queenSpriteRenderer;
		
		SpriteRenderer _spriteRenderer;

		private void Awake()
		{
			_spriteRenderer = GetComponent<SpriteRenderer>();
		}

		public void Refresh(Pawn pawn, BoardView boardView)
		{
			if (pawn.IsDead)
			{
				Destroy(gameObject);
				return;
			}
			
			Vector3 target = boardView.GetTileByBoardCoord(pawn.Position.X, pawn.Position.Y).transform.position;
			_spriteRenderer.sprite = pawn.IsWhite ? _whiteSprite : _blackSprite;
			_queenSpriteRenderer.enabled = pawn.IsQueen;
			if (transform.position != target && pawn.IsDead == false)
			{
				StartCoroutine(MovePawn(target, 0.25f));
			}
		}

		IEnumerator MovePawn(Vector3 target, float duration)
		{
			Vector3 startPosition = transform.position;
			float time = 0.0f;

			_spriteRenderer.sortingOrder += 1;
			_queenSpriteRenderer.sortingOrder += 1;
			while (time < duration)
			{
				time += Time.deltaTime;
				transform.position = Vector3.Lerp(startPosition, target, time / duration);
				yield return null;
			}
			_spriteRenderer.sortingOrder -= 1;
			_queenSpriteRenderer.sortingOrder -= 1;
		}
	}
}