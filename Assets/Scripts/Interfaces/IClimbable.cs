
/// <summary>
/// Интерфейс для объектов, которые могут забираться по лестницам и тд.
/// </summary>
public interface IClimbable {

    /// <summary>
    /// Устанавливает активную лестницу
    /// </summary>
    void SetLadder(Ladder ladder);

    /// <summary>
    /// Удаляет лестницу, когда объект ушел с нее
    /// </summary>
    void RemoveLadder();

}
