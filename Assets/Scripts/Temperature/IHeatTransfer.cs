namespace DefaultNamespace
{
    public interface IHeatTransfer
    {
        void HeatTransfer(IHeatTransfer heatTransfer, float temperature);
        void SetTemperature(float temperature);
        void Explode();
        void Freeze();
        float GetTemperature();
    }
}