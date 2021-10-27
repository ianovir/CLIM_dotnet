
namespace CLIM.models.streams
{
    /// <summary>
    /// Stream interface. See <see cref="InputStream"/> and <see cref="IOutputStream"/>
    /// </summary>
    /// <author>Sebastiano Campisi (ianovir)</author>
    public interface IStream
    {
        void Open();
        void Close();

    }
}
