[System.Serializable]
public class Condition
{
    public enum ConditionType
    {
        HP,
        Stamina
    }
    public ConditionType type;

    public float maxValue;
    public float curValue;
    public float CurValue
    {
        get => curValue;
        set
        {
            curValue = value;
            switch (type)
            {
                case ConditionType.HP:
                    PlayerEvent.OnHpChanged?.Invoke(curValue);
                    break;
                case ConditionType.Stamina:
                    PlayerEvent.OnStaminaChanged?.Invoke(curValue);
                    break;
            }
        }
    }

    public float Normalized => curValue / maxValue;
}
