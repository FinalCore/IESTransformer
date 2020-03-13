using GalaSoft.MvvmLight;
using IESTransformer.lib;
using IESTransformer.lib.Services;
using System.Collections.ObjectModel;

namespace IESTransformer.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly IesFilesManager iesFilesManager;
        private string title = "IES Transformer";
        public string Title
        {
            get => title;
            set => Set(ref title, value);
        }

        // ��� ������������� ObservableCollection ������������� ����� ����� � ����� ��������� �� ������� (��������, ���������� ��� �������� �� ���������)
        private ObservableCollection<IesFile> iesFiles; 
        public ObservableCollection<IesFile> IesFiles
        { get => iesFiles;
            set => Set(ref iesFiles, value);
        }

        // ��������� �������� ���� iesFilesManager � �������������� ������������ MainVindowViewModel ����� ���������� � iesFilesManager �� ����, ��� �� ������� ������ 
        public MainWindowViewModel(IesFilesManager iesFilesManager)
        {
            this.iesFilesManager = iesFilesManager;
            // ������� ��������� ies ������ � ��������� �� ����� ����� ������ GetAll 
            iesFiles = new ObservableCollection<IesFile>(iesFilesManager.GetAll());
        }
    }
}