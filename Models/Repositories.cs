using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WyvernWatch.Models
{
    public record class Repositories([property: JsonPropertyName("name")] string Name);

}
