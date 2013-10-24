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
            return ReadItem(new Tokenizer(new StreamReader(filePath)));
        }

        private bool ReadItem(Tokenizer reader)
        {
            try
            {
                SyncItem.ReadItem(reader);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}