namespace CityTraffic.Tests.Unit
{
    public class SimpleTests
    {
        [Theory]
        [InlineData("POINT (56.36694 58.11474)")]
        [InlineData("56.36694 58.11474")]
        public void Location_ParseCoordinatesTest_ReturnTupleDouble(string location)
        {
            var (Lat, Lon) = Models.Entities.Location.ParseCoordinates(location);

            Assert.True(Lat == 56.36694 && Lon == 58.11474);
        }
    }
}
