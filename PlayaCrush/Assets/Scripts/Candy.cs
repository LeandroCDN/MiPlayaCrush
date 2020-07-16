using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candy : MonoBehaviour
{

    private static Color selectedColor = new Color(0.5f, 0.5f, 0.5f, 1.0f);
    private static Candy previusSelected = null;

    private SpriteRenderer spriteRenderer;
    private bool isSelected = false;

    public int id;

    //array de vectores
    private Vector2[] adjacentDirections = new Vector2[]
    {
        Vector2.up,
        Vector2.down,
        Vector2.left,
        Vector2.right
    };



    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void SelectCandy()
    {
        isSelected = true;
        spriteRenderer.color = selectedColor;
        previusSelected = gameObject.GetComponent<Candy>();
    }

    private void DeselectCandy()
    {
        isSelected = false;
        spriteRenderer.color = Color.white;
        previusSelected = null;
    }

    private void OnMouseDown()
    {
        if (spriteRenderer.sprite == null || BoardManager.sharedInstance.isShifting)
        {
            return;
        }
        if (isSelected)
        {
            DeselectCandy();
        }
        else
        {
            if(previusSelected == null)
            {
                SelectCandy();
            }
            else
            {
                if (CanSwipe())
                {
                    SwapSprite(previusSelected);
                    previusSelected.FindAllMatches();
                    previusSelected.DeselectCandy();
                    FindAllMatches();
                    GUIManager.sharedInstance.MoveCounter--;
                }
                else
                {
                    previusSelected.DeselectCandy();
                    SelectCandy();
                }
            }
        }
    }

    public void SwapSprite(Candy newCandy)
    {
        if(spriteRenderer.sprite == newCandy.GetComponent<SpriteRenderer>().sprite)
        {
            return;
        }

        //para hacer un swap, nesesitamos un auxiliar.
        Sprite oldCandy = newCandy.spriteRenderer.sprite;
        newCandy.spriteRenderer.sprite = this.spriteRenderer.sprite;
        this.spriteRenderer.sprite = oldCandy;

        int auxId = newCandy.id;
        newCandy.id = this.id;
        this.id = auxId;



    }

    private GameObject GetNeighbor(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(this.transform.position, direction);
        if ( hit.collider != null)
        {
            return hit.collider.gameObject;
        }
        else
        {
            return null;
        }
    }

    private List<GameObject> GetAllNighbor()
    {
        List<GameObject> neighbors = new List<GameObject>();
        foreach(Vector2 direction in adjacentDirections)
        {
            neighbors.Add(GetNeighbor(direction));
        }
        return neighbors;
    }

    private bool CanSwipe()
    {
        return GetAllNighbor().Contains(previusSelected.gameObject);
    }

    private List<GameObject> FindMatch(Vector2 direction)
    {
        List<GameObject> matchingCandies = new List<GameObject>();

        RaycastHit2D hit = Physics2D.Raycast(this.transform.position, direction);
        while(hit.collider != null && hit.collider.GetComponent<SpriteRenderer>().sprite == spriteRenderer.sprite)
        {
            matchingCandies.Add(hit.collider.gameObject);
            hit = Physics2D.Raycast(hit.collider.transform.position, direction);
        }

        return matchingCandies;
    }

    private bool ClearMatch(Vector2 [] directions)
    {
        List<GameObject> matchingCandies = new List<GameObject>();
        foreach (Vector2 direction in directions)
        {
            matchingCandies.AddRange(FindMatch(direction));
        }
        if (matchingCandies.Count >= BoardManager.MIN_CANDY_TO_MATCH)
        {
            foreach (GameObject candy in matchingCandies)
            {
                candy.GetComponent<SpriteRenderer>().sprite = null;
            }
            return true;
        }
        else
        {
            return false;
        }
        
    }
    public void FindAllMatches()
    {
        if (spriteRenderer.sprite == null)
        {
            return;
        }
        bool hMatch = ClearMatch(new Vector2[2] { Vector2.left, Vector2.right });
        bool vMatch = ClearMatch(new Vector2[2] { Vector2.up, Vector2.down });
        if(hMatch || vMatch)
        {
            spriteRenderer.sprite = null;
            StopCoroutine(BoardManager.sharedInstance.FindNullCandies());
            StartCoroutine(BoardManager.sharedInstance.FindNullCandies());
        }
    }
}
