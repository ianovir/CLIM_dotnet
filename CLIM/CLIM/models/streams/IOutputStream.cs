namespace CLIM.models.streams
{
    /// <summary>
    /// Output Stream interface for the <see cref="Engine"/>
    /// </summary>
    /// <author>Sebastiano Campisi (ianovir)</author>
    public interface IOutputStream : IStream
    {
        void Put(string msg);
    }
}
