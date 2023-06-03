//
// Copyright (c) 2023 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karamem0.Commistant.Serialization
{

    public static class JsonSerializer
    {

        private static readonly JsonSerializerSettings settings = new()
        {
            TypeNameHandling = TypeNameHandling.All
        };

        public static T? Deserialize<T>(string? value)
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            return JsonConvert.DeserializeObject<T>(value, settings);
        }

        public static string Serialize<T>(T? value)
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            return JsonConvert.SerializeObject(value, settings);
        }

    }

}
