namespace Temperature
{
    public class StarScript : StrObject
    {
        public float speedLimit = 3f;
        new void Update()
        {
            base.Update();
            if (temperature >= temperatureLimit)
            {
                temperature = temperatureLimit;
            }
            
            if (Rb.velocity.magnitude > speedLimit)
            {
                Rb.velocity = Rb.velocity.normalized * speedLimit;
            }

        }
        
    }
}
