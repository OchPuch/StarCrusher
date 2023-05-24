
using Temperature;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Image))]
public class RicochetButtonUI : MonoBehaviour
{
    public Image buttonImage;
    public RocketScript rocketScript;

    [SerializeField]
    private Color colorDisabled;
    [SerializeField]
    private Color color1;
    [SerializeField]
    private Color color2;
    [SerializeField]
    private Color color3;
    
    
    private void Start()
    { 
        buttonImage = GetComponent<Image>();
        if (!rocketScript)
        {
            rocketScript = GameObject.Find("Player").GetComponent<RocketScript>();
            
        }
    }

    void Update()
    {
        
        
        var temp = rocketScript.GetTemperature();
        var abCost = rocketScript.abilityCost;
        if (rocketScript.GetAbilityTimer() > 0)
        {
            buttonImage.color = colorDisabled;
        }
        else if (temp < abCost)
        {
            buttonImage.color = colorDisabled;
        }
        else if (temp < abCost * 2)
        {
            buttonImage.color = color1;
        }
        else if (temp < abCost * 3)
        {
            buttonImage.color = color2;
        }
        else 
        {
            buttonImage.color = color3;
        }
        
    }
}
