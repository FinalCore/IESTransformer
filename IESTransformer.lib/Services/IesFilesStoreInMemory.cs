using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IESTransformer.lib.Data;
namespace IESTransformer.lib.Services
{
    /// <summary>
    /// Класс, отвечающий за хранение ies файлов в памяти (если в последствии источник данных изменится, например, на БД или какие-нибудь файлы, изменения нужно будет внести только в этот класс, поскольку класс IesFileManager обращается к данному классу
    /// </summary>
    public class IesFilesStoreInMemory
    {
        /// <summary>
        /// Метод, возвращающий коллекцию ies файлов, добавленных в текущем сеансе работы приложения
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IesFile> Get() => TestData.IesFiles;
    }
}
