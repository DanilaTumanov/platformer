
/// <summary>
/// Интерфейс объекта, который может принять урон
/// </summary>
public interface IDamagable {

    /// <summary>
    /// Нанесение урона
    /// </summary>
    /// <param name="damageDealer">Объект, наносящий урон</param>
    void Damage(IDamageDealer damageDealer);

    /// <summary>
    /// Получение уникального идентификатора экземпляра. Нужен для обработки нанесения периодического урона одним объектом нескольким
    /// </summary>
    int GetInstanceID();

}
