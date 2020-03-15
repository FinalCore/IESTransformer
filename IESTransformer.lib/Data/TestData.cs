using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IESTransformer.lib.Data
{
    public static class TestData
    {
        public static List<IesFile> IesFiles { get; } = new List<IesFile> { new IesFile("Test file", 1000, 10) };  
        //public static ObservableCollection<IesFile> IesFiles { get; } = new ObservableCollection<IesFile> { new IesFile("Test file", 1000, 10) };          
    }
}
