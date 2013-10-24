using System;
using System.IO;
using Sitecore.Data.Serialization.ObjectModel;
using Sitecore.Serialization.Core;

namespace Sitecore.Serialization.Infrastructure
{
    public class ItemValidator : IItemValidator
    {
        public bool IsValid(string filePath)
        {
            using (var streamReader = new StreamReader(filePath))
            {
                return ReadItem(new Tokenizer(streamReader));                
            }
        }

        private static bool ReadItem(Tokenizer reader)
        {
            try
            {
                SyncItem.ReadItem(reader);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}