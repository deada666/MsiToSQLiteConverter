using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MsiToSqLiteConverter.Schema
{
    public class TableSchema
    {
        [Required]
        public string TableName { get; set; }

        [Required]
        public IDictionary<string, ColumnInfo> Columns { get; set; }
    }
}
