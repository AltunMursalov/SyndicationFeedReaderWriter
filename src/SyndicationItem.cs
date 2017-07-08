﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

namespace Microsoft.SyndicationFeed
{
    sealed class SyndicationItem : ISyndicationItem
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Copyright { get; set; }

        public string Description { get; set; }

        public IEnumerable<ISyndicationPerson> Authors { get; set; }

        public Uri BaseUri { get; set; }

        public IEnumerable<ISyndicationCategory> Categories { get; set; }

        public string Content { get; set; }

        public IEnumerable<ISyndicationPerson> Contributors { get; set; }

        public IEnumerable<ISyndicationLink> Links { get; set; }

        public DateTimeOffset LastUpdatedTime { get; set; }

        public DateTimeOffset PublishDate { get; set; }
    }
}
