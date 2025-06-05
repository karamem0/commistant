//
// Copyright (c) 2022-2025 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karamem0.Commistant.Helpers;

public static class FunctionToolHelper
{

    public static async Task<BinaryData> GetSchemaAsync(string name)
    {
        return BinaryData.FromBytes(
            await File.ReadAllBytesAsync(
                Path.Combine(
                    AppContext.BaseDirectory,
                    "Resources",
                    "FunctionTool",
                    $"{name}.json"
                )
            )
        );
    }

}
