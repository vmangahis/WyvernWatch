using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WyvernWatch.Models
{
   public record CommitAuthor(
    [property: JsonPropertyName("name")]
    string CommitAuthorName,

    [property: JsonPropertyName("date")]
    DateTime dateCommit
       
   );
}
