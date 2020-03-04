using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IESTransformer.lib.Services
{
    public class IesFilesManager
    {
        private IesFilesStoreInMemory store;
        public IesFilesManager(IesFilesStoreInMemory Store)
        {
            store = Store;
        }

        //public IEnumerable<IesFile> GetAll()
        //{
        //    //return store.Get();
        //}

        public void Add(IesFile newFile) { }

        public void Edit(IesFile file) { }
        public void Delete(IesFile file) { }
       
    }
}
