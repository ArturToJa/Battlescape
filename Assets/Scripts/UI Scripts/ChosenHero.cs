using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChosenHero : MonoBehaviour
{
    public Sprite theHero;
    [SerializeField] Color colorNull;
    [SerializeField] Color colorFull;

    [SerializeField] Image thisHeroImage;

   
    private void Update()
    {
        thisHeroImage.sprite = theHero;
        if (thisHeroImage.sprite == null)
        {
            thisHeroImage.color = colorNull;
        }
        else
        {
            thisHeroImage.color = colorFull;
        }
    }

    public void ChoseHero(Sprite hero)
    {
        theHero = hero;
    }
}
