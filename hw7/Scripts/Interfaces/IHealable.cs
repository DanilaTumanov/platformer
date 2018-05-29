
/// <summary>
/// Интерфейс, определяющий объекты, способные получать лечение
/// </summary>
public interface IHealable
{

    /// <summary>
    /// Лечение объекта
    /// </summary>
    /// <param name="healing">Сила лечения</param>
    /// <returns></returns>
    void Heal(float healing);

}
