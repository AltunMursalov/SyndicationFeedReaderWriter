﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Xml;

namespace Microsoft.SyndicationFeed
{
    public class Atom10FeedFormatter : ISyndicationFeedFormatter
    {
        public ISyndicationCategory ParseCategory(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            using(XmlReader reader = XmlReader.Create(new StringReader(value)))
            {
                reader.MoveToContent();
                return ParseCategory(reader);
            }
        }

        public ISyndicationImage ParseImage(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            using (XmlReader reader = XmlReader.Create(new StringReader(value)))
            {
                reader.MoveToContent();
                return ParseImage(reader);
            }
        }

        public ISyndicationItem ParseItem(string value)
        {
            throw new NotImplementedException();
        }

        public ISyndicationLink ParseLink(string value)
        {
            throw new NotImplementedException();
        }

        public ISyndicationPerson ParsePerson(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            using (XmlReader reader = XmlReader.Create(new StringReader(value)))
            {
                reader.MoveToContent();
                return ParsePerson(reader);
            }

        }

        public bool TryParseValue<T>(string value, out T result)
        {
            result = default(T);

            Type type = typeof(T);

            //
            // String
            if (type == typeof(string))
            {
                result = (T)(object)value;
                return true;
            }

            //
            // DateTimeOffset
            //if (type == typeof(DateTimeOffset))
            //{
            //    DateTimeOffset dt;
            //    if (DateTimeUtils.TryParse(value, out dt))
            //    {
            //        result = (T)(object)dt;
            //        return true;
            //    }
            //}

            //
            // Todo being added in netstandard 2.0
            //if (type.GetTypeInfo().IsEnum)
            //{
            //    if (Enum.TryParse(typeof(T), value, true, out T o)) {
            //        result = (T)(object)o;
            //        return true;
            //    }
            //}

            //
            // Uri
            if (type == typeof(Uri))
            {
                Uri uri;
                if (UriUtils.TryParse(value, out uri))
                {
                    result = (T)(object)uri;
                    return true;
                }
            }

            //
            // Fall back default
            return (result = (T)Convert.ChangeType(value, typeof(T))) != null;
        }

        private SyndicationPerson ParsePerson(XmlReader reader)
        {
            var person = new SyndicationPerson();

            person.RelationshipType = reader.Name;

            //Read inner elements of person
            reader.ReadStartElement();

            while (reader.IsStartElement())
            {
                switch (reader.LocalName)
                {
                    case Atom10Constants.NameTag:
                        person.Name = reader.ReadElementContentAsString();
                        break;

                    case Atom10Constants.EmailTag:
                        person.Email = reader.ReadElementContentAsString();
                        break;

                    case Atom10Constants.UriTag:
                        person.Uri = reader.ReadElementContentAsString();
                        break;
                }
            }

            reader.ReadEndElement(); //end of author / contributor

            return person;
        }

        private SyndicationImage ParseImage(XmlReader reader)
        {
            //
            // Atom Icon and Logo only contain one string with an Uri.
            string relationshipType = reader.Name;
            Uri uri = null;
            if (!TryParseValue(reader.ReadElementContentAsString(), out uri))
            {
                throw new FormatException("Invalid image url.");
            }

            SyndicationImage image = new SyndicationImage(uri);
            image.RelationshipType = relationshipType;
            return image;
        }

        private SyndicationCategory ParseCategory(XmlReader reader)
        {
            if (!reader.HasAttributes)
            {
                throw new FormatException("The category doesn't contain any attribute.");
            }

            string term = reader.GetAttribute("term");

            if (string.IsNullOrEmpty(term))
            {
                throw new FormatException("The category doesn't contain term attribute.");
            }

            var category = new SyndicationCategory()
            {
                Name = term
            };

            reader.Read();
            return category;
        }

    }
}
