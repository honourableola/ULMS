using System.Collections.Generic;
using System.Linq;

namespace WebUI.Models.Components
{
    public class DatatableComponentModel
    {
        public string DatatableId { get; set; }
        public string DataLoadUrl { get; set; }
        public string? GeneralFilterPlaceholder { get; set; }
        public bool ServerPaging { get; set; }
        public bool ServerFiltering { get; set; }
        public bool ServerSorting { get; set; }
        public int PageSize { get; set; }

        public IReadOnlyDictionary<string, DatatablePartialBaseModel> FilterControlsPartials { get; set; }
        public IReadOnlyDictionary<string, DatatablePartialBaseModel> SelectedRowsActionButtonsPartials { get; set; }

        public IReadOnlyList<DatatableComponentModelColumn> Columns { get; set; }

        public bool ShouldShowGeneralFilter =>
            GeneralFilterPlaceholder != null;

        public bool ShouldShowExtraFilterControls =>
            FilterControlsPartials.Any();

        public bool SupportsMultiselectActions =>
            SelectedRowsActionButtonsPartials.Any();
    }
}