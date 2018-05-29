
/// <summary>
/// Интерфейс, определяющий объекты, способные наносить периодический урон
/// </summary>
public interface IPeriodicDamageDealer : IDamageDealer {

    /// <summary>
    /// Метод должен возвращать частоту периодического урона
    /// </summary>
    /// <returns></returns>
    float GetDamagePeriod();

}
