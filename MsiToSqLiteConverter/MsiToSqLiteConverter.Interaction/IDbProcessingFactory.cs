
namespace MsiToSqLiteConverter.Interaction
{
    public interface IDbProcessingFactory
    {
        IDbParser CreateDbParser(string msiPath);

        IDbWriter CreateDbWriter(string msiPath);
    }
}
