namespace MsiToSqLiteConverter.MsiProcessing.Domain
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using MsiToSqLiteConverter.MsiProcessing.Domain.Schema;
    using MsiToSqLiteConverter.MsiProcessing.Exceptions;
    using MsiToSqLiteConverter.Schema;

    /// <summary>
    /// The IDT reader.
    /// </summary>
    public class IdtReader
    {
        /// <summary>
        /// Reads the table schema.
        /// </summary>
        /// <param name="idtFilePath">
        /// The IDT file path.
        /// </param>
        /// <returns>
        /// MSI table schema.
        /// </returns>
        /// <exception cref="System.ArgumentException">
        /// IDT file not found. Schema cannot be extracted.
        /// </exception>
        public TableSchema ReadTableSchema(string idtFilePath)
        {
            if (!File.Exists(idtFilePath))
            {
                throw new ArgumentException("Idt file not found. Schema cannot be extracted.");
            }

            string columnNames;
            string columnDefinitions;
            string tableProperties;

            using (var fileReader = new FileStream(idtFilePath, FileMode.Open))
            {
                using (var reader = new StreamReader(fileReader))
                {
                    try
                    {
                        columnNames = reader.ReadLine();
                        columnDefinitions = reader.ReadLine();
                        tableProperties = reader.ReadLine();

                        if (string.IsNullOrWhiteSpace(columnNames) || string.IsNullOrWhiteSpace(columnDefinitions)
                            || string.IsNullOrWhiteSpace(tableProperties))
                        {
                            throw new IdtParseException("IdtFile is corrupted. Schema cannot be extracted.");
                        }
                    }
                    catch (IdtParseException)
                    {
                        throw;
                    }
                    catch (Exception exception)
                    {
                        throw new IdtParseException("Error processing IDT file. Schema cannot be extracted.", exception);
                    }
                }
            }

            var result = new MsiTableSchema(columnNames, columnDefinitions, tableProperties) { IdtFilePath = idtFilePath };
            return result;
        }

        /// <summary>
        /// Gets the content of the table.
        /// </summary>
        /// <param name="tableSchema">The table schema.</param>
        /// <returns>Table rows.</returns>
        /// <exception cref="System.ArgumentNullException">tableSchema data.</exception>
        /// <exception cref="IdtParseException">Table schema is corrupted. Idt file path is not specified. Cannot get table content.</exception>
        public IEnumerable<RowData> GetTableContent(TableSchema tableSchema)
        {

            if (tableSchema == null)
            {
                throw new ArgumentNullException("tableSchema");
            }

            var schema = tableSchema as MsiTableSchema;
            if (schema == null)
            {
                throw new ArgumentException("Only MsiTableSchema object is exptected as argument.");
            }

            if (string.IsNullOrWhiteSpace(schema.IdtFilePath))
            {
                throw new IdtParseException("Table schema is corrupted. Idt file path is not specified. Cannot get table content.");
            }

            if (!File.Exists(schema.IdtFilePath))
            {
                throw new FileNotFoundException("Table content cannot be read, because source IDT file is not exist.");
            }

            var result = new List<RowData>();

            var idtFilePath = schema.IdtFilePath;

            using (var fileReader = new FileStream(idtFilePath, FileMode.Open))
            {
                using (var reader = new StreamReader(fileReader))
                {
                    // Skipping table schema definition lines
                    reader.ReadLine();
                    reader.ReadLine();
                    reader.ReadLine();

                    // Reading row data
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        result.Add(new MsiRowData(schema, line));
                    }
                }
            }

            return result;
        }
    }
}