using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Microsoft.Practices.Prism.ViewModel;

namespace FilterDemo
{
    public class MainWindowViewModel: NotificationObject
    {
        public MainWindowViewModel()
        {
            
        }
        
        private ObservableCollection<ProdPrepareDrawingData> _ProdPrepareDrawingDataList = new ObservableCollection<ProdPrepareDrawingData>();
        public ObservableCollection<ProdPrepareDrawingData> ProdPrepareDrawingDataList
        {
            get
            {
                return _ProdPrepareDrawingDataList;
            }
            set
            {
                _ProdPrepareDrawingDataList = value;
                RaisePropertyChanged("ProdPrepareDrawingDataList");
            }
        }

        private ProdPrepareDrawingData _SelectedProdPrepareDrawingData;
        public ProdPrepareDrawingData SelectedProdPrepareDrawingData
        {
            get
            {
                return _SelectedProdPrepareDrawingData;
            }
            set
            {
                _SelectedProdPrepareDrawingData = value;
                RaisePropertyChanged("SelectedProdPrepareDrawingData");
            }
        }

        private void UpdateIndex()
        {
            foreach (var each in _ProdPrepareDrawingDataList)
            {
                each.Index = _ProdPrepareDrawingDataList.IndexOf(each) + 1;
            }
        }

        public void UpdateDataGrid(DataGrid dataGrid)
        {
            dataGrid.BeginInit();
            UpdateIndex();
            dataGrid.CurrentColumn = dataGrid.Columns[0];
            dataGrid.ScrollIntoView(dataGrid.SelectedItem, dataGrid.CurrentColumn);
            dataGrid.EndInit();
        }
    }

    public class ProdPrepareDrawingData
    {
        public int Index { get; set; }
        public string DrawingName { get; set; }
        public string TaskName { get; set; }
        public string ProcessName { get; set; }
        public string ProductionCount { get; set; }
        public string DrawingSize { get; set; }
        public string ProdcutionStatus { get; set; }
        public string Sort { get; set; }
    }
}
