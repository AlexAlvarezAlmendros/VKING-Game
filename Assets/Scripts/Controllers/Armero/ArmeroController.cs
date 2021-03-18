using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmeroController : MonoBehaviour
{
    //GameObjects que se modificaran dependiendo de l informacion de arriba//
    [SerializeField]
    public GameObject[] stats;
    [SerializeField]
    public GameObject[] bloqued;
    [SerializeField]
    public GameObject[] mods;

    void Start()
    {
        WeaponManager.Instance.initInitialWeapons();
        fillInfoArmero();
    }

    void Update()
    {
    }
    public void fillInfoArmero() {

        //Fill Wepon Stats
        for (int i = 0; i < 5; i++) { 
            //stats[i].tal = a tal;
        }

        //Set Bloqued Weapons
        for (int i = 0; i < 4; i++)
        {
            if (WeaponManager.Instance.armas[i].bloqueado)
            {
                bloqued[i].SetActive(false);
            }
            else {
                bloqued[i].SetActive(true);
            }
        }

        //Set Weapon Modifications
        for (int i = 0; i < 5; i++)
        {
            stats[i].GetComponent<StatsController>().alcance.SetValueWithoutNotify(WeaponManager.Instance.armas[i].stats.alcance);
            stats[i].GetComponent<StatsController>().daño.SetValueWithoutNotify(WeaponManager.Instance.armas[i].stats.daño);
            stats[i].GetComponent<StatsController>().velocidadDeRecarga.SetValueWithoutNotify(WeaponManager.Instance.armas[i].stats.velocidadDeRecarga);
            stats[i].GetComponent<StatsController>().cargador.SetValueWithoutNotify(WeaponManager.Instance.armas[i].stats.cargador);
        }

    }
}
