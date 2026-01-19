using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PuzzleManager : MonoBehaviour
{
    [Header("Puzzle UI")]
    [SerializeField] private List<Texture2D> imageTextures;
    [SerializeField] private Transform levelSelectPanel;
    [SerializeField] private Image levelSelectPF;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (Texture2D texture in imageTextures) 
        {
            Image image = Instantiate(levelSelectPF, levelSelectPanel);
            image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            image.GetComponent<Button>().onClick.AddListener(delegate {
               // StartPuzzle(texture);
            });
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
