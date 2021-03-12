using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmeroController : MonoBehaviour
{
    public enum TiposDeArmas { PISTOLA, ESCOPETA, METRALLETA, SNIPER, LANZACOHETES };
    public enum TipoDeModificacion { FUEGO, AIRE, ROBO, ELECTRICIDAD, OSCURIDAD };

    public struct Stats
    {
        public int daño;
        public int velocidadDeRecarga;
        public int cargador;
        public int alcance;

    }
    public struct Arma {
        public TiposDeArmas tipoDeArma;
        public TipoDeModificacion tipoDeMod;
        public bool equipado;
        public bool bloqueado;
        public Stats stats;

    }
    public Arma[] arma;


    //GameObjects que se modificaran dependiendo de l informacion de arriba//
    [SerializeField]
    GameObject[] stats;
    GameObject[] bloqued;
    GameObject[] mods;

    void Start()
    {
    }

    void Update()
    {
        
    }
}
