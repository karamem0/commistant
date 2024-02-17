//
// Copyright (c) 2022-2024 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karamem0.Commistant.Models
{

    public class ConversationPropertyArguments
    {

        public ConversationPropertyArguments()
        {
        }

        public string? Type { get; set; }

        public ConversationPropertyArgumentsValue? Value { get; set; }

    }

}
