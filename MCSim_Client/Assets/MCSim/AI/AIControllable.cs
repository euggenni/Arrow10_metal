public interface AIControllable {
    /// <summary>
    /// Создает AI-агента на текущем объекте
    /// </summary>
    void InitializeAI();

    /// <summary>
    /// AI-агент
    /// </summary>
    AIUnit AIUnit { get; }
}