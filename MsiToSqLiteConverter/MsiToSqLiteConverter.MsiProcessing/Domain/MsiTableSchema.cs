namespace MsiToSqLiteConverter.MsiProcessing.Domain
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;

    using MsiToSqLiteConverter.MsiProcessing.Exceptions;
    using MsiToSqLiteConverter.Schema;

    /// <summary>
    /// The MSI table schema.
    /// </summary>
    public class MsiTableSchema : TableSchema
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MsiTableSchema"/> class.
        /// </summary>
        public MsiTableSchema()
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MsiTableSchema"/> class.
        /// </summary>
        /// <param name="columnNames">The column names.</param>
        /// <param name="columnDefinitions">The column definitions.</param>
        /// <param name="tableProperties">The table properties.</param>
        public MsiTableSchema(string columnNames, string columnDefinitions, string tableProperties)
        {
            if (string.IsNullOrWhiteSpace(columnNames))
            {
                throw new SchemaParseException("Columns information cannot be empty. Schema cannot be created.");
            }

            if (string.IsNullOrWhiteSpace(columnDefinitions))
            {
                throw new SchemaParseException("Columns definition information cannot be empty. Schema cannot be created.");
            }

            if (string.IsNullOrWhiteSpace(tableProperties))
            {
                throw new SchemaParseException("Table properties information cannot be empty. Schema cannot be created.");
            }

            this.Columns = new Dictionary<string, ColumnInfo>();
            this.CreateColumns(columnNames);
            this.FillColumnsData(columnDefinitions);
            this.SetSchemaProperties(tableProperties);
        }

        /// <summary>
        /// Gets or sets the IDT file path.
        /// </summary>
        /// <value>
        /// The IDT file path.
        /// </value>
        public string IdtFilePath { get; set; }

        /// <summary>
        /// Creates the columns.
        /// </summary>
        /// <param name="columnNames">The column names.</param>
        private void CreateColumns(string columnNames)
        {
            var splitedColumns = columnNames.Split('\t');
            foreach (var column in splitedColumns)
            {
                this.Columns.Add(column, new ColumnInfo { Name = column });
            }
        }

        /// <summary>
        /// Fills the columns data.
        /// </summary>
        /// <param name="columnDefinitions">The column definitions.</param>
        private void FillColumnsData(string columnDefinitions)
        {
            var splitedColumns = columnDefinitions.Split('\t');
            if (splitedColumns.Length != this.Columns.Count)
            {
                throw new SchemaParseException("Amount of columns is different from column definitions. Schema cannot be created.");
            }

            var index = 0;
            foreach (var columnInfo in this.Columns.Values)
            {
                try
                {
                    var columnDefinition = splitedColumns[index];
                    this.GetColumnType(columnInfo, columnDefinition);
                }
                catch (Exception exception)
                {
                    throw new SchemaParseException("Error parsing '" + columnInfo.Name + "'. Schema cannot be created.", exception);
                }
                index++;
            }
        }

        /// <summary>
        /// Sets the schema properties.
        /// </summary>
        /// <param name="tableProperties">The table properties.</param>
        /// <exception cref="MsiToSqLiteConverter.MsiProcessing.Exceptions.SchemaParseException">Table definition must contain at least table name and one key. Schema cannot be created.</exception>
        private void SetSchemaProperties(string tableProperties)
        {
            var splitedProperties = tableProperties.Split('\t').ToList();
            if (splitedProperties.Count < 2)
            {
                throw new SchemaParseException("Table definition must contain at least table name and one key. Schema cannot be created.");
            }

            int codePage;
            if (int.TryParse(splitedProperties[0], out codePage))
            {
                splitedProperties.RemoveAt(0);
            }

            this.TableName = splitedProperties[0];
            splitedProperties.RemoveAt(0);

            foreach (var property in splitedProperties)
            {
                this.Columns[property].IsKeyColumn = true;
            }
        }

        /// <summary>
        /// Gets the type of the column.
        /// </summary>
        /// <param name="info">The information.</param>
        /// <param name="columnDefinition">The column definition.</param>
        private void GetColumnType(ColumnInfo info, string columnDefinition)
        {
            var preparedString = columnDefinition.ToLower();
            if (preparedString.Length < 2)
            {
                throw new SchemaParseException("Schema definition has unsupported format.");    
            }

            var firstLetter = preparedString[0];
            var seccondLetter = preparedString[1];

            info.IsNullable = char.IsUpper(firstLetter);

            switch (firstLetter)
            {
                case 's':
                    info.ColumnType = ColumnType.Text;
                    break;
                case 'v':
                    info.ColumnType = ColumnType.Binary;
                    break;
                case 'i':
                    if (seccondLetter == '2')
                    {
                        info.ColumnType = ColumnType.Int16;
                    }
                    else if (seccondLetter == '4')
                    {
                        info.ColumnType = ColumnType.Int32;
                    }
                    else
                    {
                        throw new SchemaParseException("Integers of only type 2 and 4 are supported by MSI schema. Current value '" + seccondLetter + "'");
                    }

                    break;
                default:
                    throw new SchemaParseException(
                        "Unknown column definition type '" + firstLetter + "'");
            }

            int size;
            if (int.TryParse(columnDefinition.Substring(1), out size))
            {
                info.Size = size;
            }
        }
    }
}