namespace MsiToSqLiteConverter.MsiProcessing.Domain
{
    using System;
    using Microsoft.Deployment.WindowsInstaller;

    /// <summary>
    /// The IDT manager.
    /// </summary>
    public class DtfIdtManager : IIdtManager
    {
        /// <summary>
        /// The current package
        /// </summary>
        private Database currentPackage;

        /// <summary>
        /// Connects the specified MSI path.
        /// </summary>
        /// <param name="msiPath">The MSI path.</param>
        public void Connect(string msiPath)
        {
            this.currentPackage = new Database(msiPath, DatabaseOpenMode.Transact);
        }

        /// <summary>
        /// Disconnects this instance.
        /// </summary>
        /// <param name="isDispose">if set to <c>true</c> [is dispose].</param>
        public void Disconnect(bool isDispose = false)
        {
            if (this.currentPackage != null && !this.currentPackage.IsClosed)
            {
                this.currentPackage.Close();
                if (isDispose)
                {
                    this.currentPackage.Dispose();
                }
            }
        }

        /// <summary>
        /// Extracts the table.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="filePath">The file path.</param>
        public void ExtractTable(string tableName, string filePath)
        {
            this.CheckConnection();
            this.currentPackage.Export(tableName, filePath);
        }

        /// <summary>
        /// Imports the table.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        public void ImportTable(string filePath)
        {
            this.CheckConnection(true);
            this.currentPackage.Import(filePath);
        }

        /// <summary>
        /// Exports all.
        /// </summary>
        /// <param name="directoryPath">The directory path.</param>
        public void ExportAll(string directoryPath)
        {
            this.CheckConnection();
            this.currentPackage.ExportAll(directoryPath);
        }

        /// <summary>
        /// Imports all.
        /// </summary>
        /// <param name="directoryPath">The directory path.</param>
        public void ImportAll(string directoryPath)
        {
            this.CheckConnection(true);
            this.currentPackage.ImportAll(directoryPath);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Disconnect(true);
        }


        /// <summary>
        /// Checks the connection.
        /// </summary>
        /// <param name="isWrite">if set to <c>true</c> [is write].</param>
        /// <exception cref="System.ApplicationException">
        /// Database connection is not present, but operation on database invoked.
        /// or
        /// Database opened in readonly mode, while read/write mode expected.
        /// </exception>
        private void CheckConnection(bool isWrite = false)
        {
            if (this.currentPackage == null || this.currentPackage.IsClosed)
            {
                throw new ApplicationException("Database connection is not present, but operation on database invoked.");
            }

            if (isWrite && this.currentPackage.OpenMode == DatabaseOpenMode.ReadOnly)
            {
                throw new ApplicationException("Database opened in readonly mode, while read/write mode expected.");
            }
        }
    }
}