using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MultiCamLibrary.Property
{
    public interface IPropertyContainer
    {
        public string Name { get; }
        public string DisplayName { get; }

        event EventHandler SomePropertyChanged;
        ObservableCollection<PropertyBase> Properties { get; }
        public List<PropertyRecord> GetPropertyRecords()
        {
            var recordList = new List<PropertyRecord>();
            foreach (var property in Properties)
            {
                var propToRecord = property.GetPropertyRecord(Name);
                if (propToRecord != null)
                    recordList.Add(propToRecord);
            }
            return recordList;
        }
    }
}
