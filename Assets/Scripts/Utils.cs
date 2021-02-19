using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public static class Utils
{
    // :: GENERIC FUNCTIONS ::

    // Returns the float number with X decimals.
    public static float RoundFloat(float number, int decimals)
    {
        float mult = Mathf.Pow(10.0f, decimals);
        float rounded = Mathf.Round(number * mult) / mult;
        return rounded;
    }
    // Returns a given float percentage Range(0~1) in number float with decimals to represent in UI. Ex: (0,23456789, 3) -> returns 23,456
    public static float GetPercentage(float _num, int _decimals)
    {
        float number;

        number = RoundFloat(_num * 100, _decimals);

        return number;
    }

    //Recibe un float de tiempo, retorna una string en formato mm:ss:f donde f es la cantidad de digitos _miliseconds especificada
    public static string GetTimeFormat(float _number, int _miliseconds)
    {
        TimeSpan time;
        time = TimeSpan.FromSeconds(_number);
        string format = "";

        for (int i = 0; i < _miliseconds; i++)
        {
            format += "f";
        }
        format = "mm':'ss':'" + format;

        return time.ToString(format);
    }

    public enum WeaponsPlayer
    {
        BASICRIFLE,
        PARTYSHOTGUN,
        PEWPEWRIFLE
    }

    public enum PowerUps
    {
        VIKINGSHIELD,
        PR1N7
    }

    // :: MUSIC ENUMS ::

    // Define el tipo de audio, para controlar volumenes
    public enum AudioType
    {
        NONE,
        SFX,
        MUSIC,
        VOICE
    };

    // SubCategorias de SFX
    public enum SFX
    {
        NONE,
        DRONETALK,
        EXPLOSION,
        BASICRIFLE,
        DAMAGEHUMAN,
        ENEMYSHOTGUN,
        DEATHHUMAN,
        DEATHDRONE,
        DAMAGEBOSS
    };

    // SubCategorias de MUSIC
    public enum Music
    {
        NONE,
        MAINMENU,
        STORE,
        INGAME
    };

    // SubCategorias de VOICE
    public enum Voice
    {
        NONE
    };

    // :: CUSTOM FOR PROJECT ::

}