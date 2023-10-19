//
// Copyright (c) 2023 karamem0
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

    public class ConversationPropertyArgumentsValue
    {

        public ConversationPropertyArgumentsValue()
        {
        }

        public bool Enabled { get; set; }

        public int Schedule { get; set; } = -1;

        public string? Message { get; set; }

        public string? Url { get; set; }

    }

}
