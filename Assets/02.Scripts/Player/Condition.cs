[System.Serializable]
public class Condition
{
    public float maxValue;
    public float curValue;
    
    public float Normalized => curValue / maxValue;
}
