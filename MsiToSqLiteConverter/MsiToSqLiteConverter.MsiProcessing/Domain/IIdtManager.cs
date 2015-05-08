namespace MsiToSqLiteConverter.MsiProcessing.Domain
{
    using System;

    public interface IIdtManager : IDisposable
    {
        void Connect(string msiPath);

        void Disconnect(bool isDispose);

        void ExtactTable(string tableName, string filePath);

        void ImportTable(string filePath);

        void ExportAll(string directoryPath);

        void ImportAll(string directoryPath);
    }
}