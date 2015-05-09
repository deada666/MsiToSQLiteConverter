namespace MsiToSqLiteConverter.MsiProcessing.Domain
{
    using System;
    using System.IO;

    using MsiToSqLiteConverter.MsiProcessing.Exceptions;

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
        public MsiTableSchema ReadTableSchema(string idtFilePath)
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

            var result = new MsiTableSchema(columnNames, columnDefinitions, tableProperties);
            return result;
        }


    }
}