using System.Collections;
using pindwin.Board;
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
			Vector3 target = boardView.GetTileByBoardCoord(pawn.Position.X, pawn.Position.Y).transform.position;
			gameObject.SetActive(pawn.IsDead == false);
			_spriteRenderer.sprite = pawn.IsWhite ? _whiteSprite : _blackSprite;
			_queenSpriteRenderer.enabled = pawn.IsQueen;
			if (transform.position != target && pawn.IsDead == false)
			{
				StartCoroutine(MovePawn(target, 4.0f));
			}

			//todo set queen sprite
		}

		IEnumerator MovePawn(Vector3 target, float speed)
		{
			Vector3 startPosition = transform.position;
			float time = 0.0f;

			_spriteRenderer.sortingOrder += 1;
			_queenSpriteRenderer.sortingOrder += 1;
			while (time < 1.0f)
			{
				time += Time.deltaTime * speed;
				transform.position = Vector3.Lerp(startPosition, target, time);
				yield return null;
			}
			_spriteRenderer.sortingOrder -= 1;
			_queenSpriteRenderer.sortingOrder -= 1;
		}
	}
}