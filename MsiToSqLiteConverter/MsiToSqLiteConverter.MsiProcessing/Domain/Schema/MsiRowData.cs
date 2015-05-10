namespace MsiToSqLiteConverter.MsiProcessing.Domain.Schema
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using MsiToSqLiteConverter.MsiProcessing.Exceptions;
    using MsiToSqLiteConverter.Schema;

    /// <summary>
    /// The msi row data.
    /// </summary>
    public class MsiRowData : RowData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MsiRowData"/> class.
        /// </summary>
        /// <param name="schema">The schema.</param>
        /// <param name="rowData">The row data.</param>
        /// <exception cref="System.ArgumentNullException">schema data.</exception>
        /// <exception cref="MsiToSqLiteConverter.MsiProcessing.Exceptions.SchemaParseException">Row data connot be empty. Row cannot be created.</exception>
        public MsiRowData(MsiTableSchema schema, string rowData)
        {
            if (schema == null)
            {
                throw new ArgumentNullException("schema");
            }

            if (string.IsNullOrWhiteSpace(rowData))
            {
                throw new SchemaParseException("Row data connot be empty. Row cannot be created.");
            }

            this.Data = new Dictionary<string, CellData>();
            this.RowSchema = schema;
            this.SetDataFromString(rowData);
        }

        /// <summary>
        /// Sets the data from string.
        /// </summary>
        /// <param name="rowData">The row data.</param>
        /// <exception cref="MsiToSqLiteConverter.MsiProcessing.Exceptions.SchemaParseException">
        /// Amount of values in the row is less than amount of non nullable columns. Row data cannot be created.
        /// or
        /// Column marked as not nullable, but row has null for this column.
        /// </exception>
        /// <exception cref="System.IO.FileNotFoundException">IDT file is not exist. Row data cannot be created.</exception>
        private void SetDataFromString(string rowData)
        {
            var schema = (MsiTableSchema)this.RowSchema;
            var parts = rowData.Split('\t');
            if (parts.Length < schema.NotNullableColumnsCount)
            {
                throw new SchemaParseException("Amount of values in the row is less than amount of non nullable columns. Row data cannot be created.");
            }

            if (!File.Exists(schema.IdtFilePath))
            {
                throw new FileNotFoundException("IDT file is not exist. Row data cannot be created.");
            }

            var currentDirectory = Path.GetDirectoryName(schema.IdtFilePath);

            var index = 0;
            foreach (var columnInfo in schema.Columns.Values)
            {
                var currentValue = parts[index];
                if (string.IsNullOrEmpty(currentValue))
                {
                    if (!columnInfo.IsNullable)
                    {
                        throw new SchemaParseException("Column marked as not nullable, but row has null for this column.");
                    }

                    currentValue = null;
                }

                this.SetCellData(columnInfo, currentValue, currentDirectory, schema.TableName);
                index++;
            }
        }

        /// <summary>
        /// Sets the cell data.
        /// </summary>
        /// <param name="columnInfo">The column information.</param>
        /// <param name="currentValue">The current value.</param>
        /// <param name="currentDirectory">The current directory.</param>
        /// <param name="tableName">Name of the table.</param>
        private void SetCellData(ColumnInfo columnInfo, string currentValue, string currentDirectory, string tableName)
        {
            switch (columnInfo.ColumnType)
            {
                case ColumnType.Text:
                    this.Data.Add(
                        columnInfo.Name,
                        new CellData { CellName = columnInfo.Name, Data = currentValue, DataType = ColumnType.Text });
                    break;
                case ColumnType.Binary:
                    this.Data.Add(
                        columnInfo.Name,
                        new CellData
                            {
                                CellName = columnInfo.Name,
                                Data =
                                    Path.Combine(
                                        currentDirectory ?? string.Empty,
                                        tableName,
                                        currentValue ?? string.Empty),
                                DataType = ColumnType.Binary
                            });
                    break;
                case ColumnType.Int16:
                    {
                        object realValue = currentValue;
                        if (realValue != null)
                        {
                            realValue = short.Parse(currentValue);
                        }
                        this.Data.Add(
                            columnInfo.Name,
                            new CellData { CellName = columnInfo.Name, Data = realValue, DataType = ColumnType.Int16 });
                    }
                    break;
                case ColumnType.Int32:
                    {
                        object realValue = currentValue;
                        if (realValue != null)
                        {
                            realValue = int.Parse(currentValue);
                        }
                        this.Data.Add(
                            columnInfo.Name,
                            new CellData { CellName = columnInfo.Name, Data = realValue, DataType = ColumnType.Int32 });
                    }
                    break;
            }
        }
    }
}