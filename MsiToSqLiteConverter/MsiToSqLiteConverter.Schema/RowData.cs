using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MsiToSqLiteConverter.Schema
{
    // TODO: Add custom validation attribute to match schema and data.
    public class RowData
    {
        [Required]
        public TableSchema RowSchema { get; set; }

        [Required]
        public IDictionary<string, CellData> Data { get; set; }
    }
}
