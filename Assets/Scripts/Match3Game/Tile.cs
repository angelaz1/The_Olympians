﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private static Color selectedColor = new Color(.5f, .5f, .5f, 1.0f);
	private static Tile previousSelected = null;

	private SpriteRenderer render;
	private bool isSelected = false;

	private Vector2[] adjacentDirections = new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

    private bool matchFound = false;

	void Awake() {
		render = GetComponent<SpriteRenderer>();
    }

	private void Select() {
		isSelected = true;
		render.color = selectedColor;
		previousSelected = gameObject.GetComponent<Tile>();
	}

	private void Deselect() {
		isSelected = false;
		render.color = Color.white;
		previousSelected = null;
	}

	void OnMouseDown() {
		if (render.sprite == null || BoardManager.bm.isShifting) {
			return;
		}

		if (isSelected) {
			Deselect();
		} else {
			if (previousSelected == null) {
				Select();
			} else {
				if (GetAllAdjacentTiles().Contains(previousSelected.gameObject)) { 
					SwapSprite(previousSelected.render);
                    previousSelected.ClearAllMatches();
					previousSelected.Deselect();
                    ClearAllMatches();
				} else {
					previousSelected.GetComponent<Tile>().Deselect();
					Select();
				}
			}
		}
	}

	public void SwapSprite(SpriteRenderer render2) {
		if (render.sprite == render2.sprite) {
			return;
		}

		Sprite tmp = render2.sprite;
		render2.sprite = render.sprite;
		render.sprite = tmp;
		BoardManager.bm.MakeMove();
	}

	private GameObject GetAdjacent(Vector2 castDir) {
        Vector3 cDir = castDir;
		RaycastHit2D hit = Physics2D.Raycast(transform.position + cDir, castDir);
		if (hit.collider != null) {
			return hit.collider.gameObject;
		}
		return null;
	}

	private List<GameObject> GetAllAdjacentTiles() {
		List<GameObject> adjacentTiles = new List<GameObject>();
		for (int i = 0; i < adjacentDirections.Length; i++) {
			adjacentTiles.Add(GetAdjacent(adjacentDirections[i]));
		}
		return adjacentTiles;
	}

    private List<GameObject> FindMatch(Vector2 castDir) { 
        Vector3 cDir = castDir;
        List<GameObject> matchingTiles = new List<GameObject>(); 
        RaycastHit2D hit = Physics2D.Raycast(transform.position + cDir, castDir); 
        while (hit.collider != null && hit.collider.GetComponent<SpriteRenderer>().sprite == render.sprite) { 
            matchingTiles.Add(hit.collider.gameObject);
            hit = Physics2D.Raycast(hit.collider.transform.position + cDir, castDir);
        }
        return matchingTiles; 
    }

    private void ClearMatch(Vector2[] paths) 
    {
        List<GameObject> matchingTiles = new List<GameObject>(); 
        for (int i = 0; i < paths.Length; i++) {
            matchingTiles.AddRange(FindMatch(paths[i]));
        }
        if (matchingTiles.Count >= 2) {
            for (int i = 0; i < matchingTiles.Count; i++) {
                matchingTiles[i].GetComponent<SpriteRenderer>().sprite = null;
            }
            matchFound = true; 
        }
    }

    public void ClearAllMatches() {
        if (render.sprite == null)
            return;

        ClearMatch(new Vector2[2] { Vector2.left, Vector2.right });
        ClearMatch(new Vector2[2] { Vector2.up, Vector2.down });
        if (matchFound) {
            render.sprite = null;
            matchFound = false;
            StopCoroutine(BoardManager.bm.FindNullTiles());
            StartCoroutine(BoardManager.bm.FindNullTiles());
        }
    }
}
