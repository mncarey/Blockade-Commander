using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FortImageDisplay : MonoBehaviour
{
    [SerializeField] private Image uiImage;
    [SerializeField] private TMP_Text nameTextRef;
    [SerializeField] private TMP_Text contentTextRef;
    
    //[SerializeField] public Sprite newSprite;
     

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        
            
        uiImage = GetComponent<Image>();
        
        
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeImage(Sprite newSprite, string name, string content)
    {
        
        uiImage.sprite = newSprite;
        nameTextRef.text = name;
        contentTextRef.text = content;
        
    }
}
