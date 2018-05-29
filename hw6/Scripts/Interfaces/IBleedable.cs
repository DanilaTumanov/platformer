using UnityEngine;

/// <summary>
/// Интерфейс, определяющий объекты с кровью
/// </summary>
public interface IBleedable
{
    /// <summary>
    /// Получение объекта, отвечающего за точку кровотечения
    /// </summary>
    /// <returns></returns>
    GameObject GetBloodGO();

    /// <summary>
    /// Получение системы частиц, анимирующей кровотечение
    /// </summary>
    /// <returns></returns>
    ParticleSystem GetBloodPS();


    void StopBleed();
}
