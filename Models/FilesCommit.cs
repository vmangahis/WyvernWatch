using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WyvernWatch.Models
{
    public record FilesCommit(
        [property: JsonPropertyName("filename")]
        string File,
        
        [property: JsonPropertyName("changes")]
        int Changes,

        [property: JsonPropertyName("status")]
        string FileStatus
     );
}
