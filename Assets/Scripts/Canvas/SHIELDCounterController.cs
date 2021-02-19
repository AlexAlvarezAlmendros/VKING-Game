using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SHIELDCounterController : MonoBehaviour
{
    public GameObject shieldPointPrefab;
    List<GameObject> shieldPointsList = new List<GameObject>();
    public int maxShieldUnits;
    private int actualShieldUnits;
    public bool awakeWithFullShields;
    private int remaintingSlotsShieldUnits;

    public bool hasShield;

    public GameObject shieldImpactFX;

    private GameObject rootImage;
    private Image[] imageList;


    void changeColor(int id, bool active)
    {
        Image shieldPoint = shieldPointsList[id].GetComponent<Image>();

        Color actualColor = shieldPoint.color;
        
        if(active) { actualColor.a = 1f; }
        else { actualColor.a = 0.33f; }

        shieldPoint.color = actualColor;
    }

    public void AddShieldPoint()
    {
        if(remaintingSlotsShieldUnits > 0)
        {
            //SoundManager.Instance.PlaySfx("ShieldGain");
            remaintingSlotsShieldUnits--;
            changeColor(remaintingSlotsShieldUnits, true);
            hasShield = true;
            SetImages(true);
        }
    }

    public void RemoveShieldPoint()
    {
        if (remaintingSlotsShieldUnits < maxShieldUnits)
        {
            changeColor(remaintingSlotsShieldUnits, false);
            remaintingSlotsShieldUnits++;

            if(remaintingSlotsShieldUnits == maxShieldUnits)
            {
                hasShield = false;
                SetImages(false);
            }
            else
            {
                hasShield = true;
                SetImages(true);
            }

            //Feedback visual
            GameObject newShieldImpactFX = Instantiate(shieldImpactFX, transform.position, transform.rotation);
            //newShieldImpactFX.transform.localPosition += Vector3.up;
            Destroy(newShieldImpactFX, 2f);

        }
    }


    private void SetImages(bool state)
    {

        rootImage.GetComponent<Image>().enabled = state;
        foreach (var image in imageList)
        {
            image.enabled = state;
        }

    }


    
    void Start()
    {
        rootImage = gameObject;
        actualShieldUnits = 0;
        remaintingSlotsShieldUnits = maxShieldUnits;

        int id;
        for (int i = 0; i < maxShieldUnits; i++)
        {
            GameObject newShieldPoint = Instantiate(shieldPointPrefab, transform);
            id = i + 1;
            newShieldPoint.name = "ShieldPoint_" + id;
            shieldPointsList.Add(newShieldPoint);

            changeColor(i, false);
        }

        imageList = rootImage.GetComponentsInChildren<Image>();
        
        SetImages(false);

        if (awakeWithFullShields)
        {
            for (int i = 0; i < maxShieldUnits; i++)
            {
                AddShieldPoint();
            }
        }

    }


}
