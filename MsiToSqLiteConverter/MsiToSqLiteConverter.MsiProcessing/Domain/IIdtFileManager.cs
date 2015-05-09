namespace MsiToSqLiteConverter.MsiProcessing.Domain
{
    using System;

    /// <summary>
    /// The IDT Manager interface.
    /// </summary>
    public interface IIdtFileManager : IDisposable
    {
        /// <summary>
        /// Connects the specified MSI path.
        /// </summary>
        /// <param name="msiPath">The MSI path.</param>
        void Connect(string msiPath);

        /// <summary>
        /// Disconnects the specified is dispose.
        /// </summary>
        /// <param name="isDispose">if set to <c>true</c> [is dispose].</param>
        void Disconnect(bool isDispose);

        /// <summary>
        /// Extracts the table.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="filePath">The file path.</param>
        void ExtractTable(string tableName, string filePath);

        /// <summary>
        /// Imports the table.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        void ImportTable(string filePath);

        /// <summary>
        /// Exports all.
        /// </summary>
        /// <param name="directoryPath">The directory path.</param>
        void ExportAll(string directoryPath);

        /// <summary>
        /// Imports all.
        /// </summary>
        /// <param name="directoryPath">The directory path.</param>
        void ImportAll(string directoryPath);
    }
}