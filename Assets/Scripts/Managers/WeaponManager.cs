using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{

    /*index
      ########################
      #                      #
      #    ARMAS ACTUALES    #
      #                      #
      ########################
    */

    public enum TiposDeArmas { PISTOLA, ESCOPETA, METRALLETA, SNIPER, LANZACOHETES };
    public enum TipoDeModificacion { FUEGO, AIRE, ROBO, ELECTRICIDAD, OSCURIDAD, NONE };

    public struct Stats
    {
        public int daño;
        public int velocidadDeRecarga;
        public int cargador;
        public int alcance;

    }
    public struct Arma
    {
        public TiposDeArmas tipoDeArma;
        public TipoDeModificacion tipoDeMod;
        public bool equipado;
        public bool bloqueado;
        public Stats stats;

    }
    public Arma[] armas = new Arma[5];

    public void initInitialWeapons()
    {
        for (int i = 0; i < 5; i++)
        {
            armas[i].tipoDeArma = (TiposDeArmas)i;
            armas[i].tipoDeMod = TipoDeModificacion.NONE;
            armas[i].equipado = false;
            armas[i].bloqueado = true;
            armas[i].stats.alcance = new System.Random().Next(20, 100);
            armas[i].stats.cargador = new System.Random().Next(20, 100);
            armas[i].stats.daño = new System.Random().Next(20, 100);
            armas[i].stats.velocidadDeRecarga = new System.Random().Next(20, 100);
        }
        armas[0].equipado = true;
    }



    /*index
      ########################
      #                      #
      #  FUNCIONES DE UNITY  #
      #                      #
      ########################
    */


    // Instanciar WeaponManager
    public static WeaponManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Debug.Log("Warning: multiple " + this + " in scene!");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
