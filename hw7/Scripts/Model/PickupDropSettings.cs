using System;
using UnityEngine;

/// <summary>
/// Структура, отвечающая за настройку дропа подбираемого предмета.
/// </summary>
[Serializable]
public struct PickupDropSettings
{
    /// <summary>
    /// Подбираемый предмет
    /// </summary>
    public Pickup pickup;  
    
    /// <summary>
    /// Вероятность дропа в процентах
    /// </summary>
    public float possibility;  
}
