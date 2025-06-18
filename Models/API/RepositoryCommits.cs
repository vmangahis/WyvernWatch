using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WyvernWatch.Models.API
{
    public record RepositoryCommits(

        [property: JsonPropertyName("url")]
        string ProjectUrl


    );
}
