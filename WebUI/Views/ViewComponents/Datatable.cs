using WebUI.Models.Components;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Column = WebUI.Models.Components.DatatableComponentModelColumn;
using ComponentModel = WebUI.Models.Components.DatatableComponentModel;

namespace IDH.Web.ViewComponents
{
    public class Datatable : ViewComponent
    {
        public IViewComponentResult Invoke (string datatableId,
                                           string url,
                                           int pageSize,
                                           bool serverFiltering,
                                           bool serverPaging,
                                           bool serverSorting,
                                           string? filterPlaceholder,
                                           IReadOnlyList<Column> columns,
                                           IReadOnlyDictionary<string, DatatablePartialBaseModel>?
                                               actionControls,
                                           IReadOnlyDictionary<string, DatatablePartialBaseModel>?
                                               filterControls)
        {
            if (string.IsNullOrWhiteSpace(filterPlaceholder))
            {
                filterPlaceholder = null;
            }

            var model = new ComponentModel
            {
                DatatableId = datatableId,
                DataLoadUrl = url,
                PageSize = pageSize,
                ServerFiltering = serverFiltering,
                ServerPaging = serverPaging,
                ServerSorting = serverSorting,
                GeneralFilterPlaceholder = filterPlaceholder,
                Columns = columns,
                FilterControlsPartials = filterControls ?? new Dictionary<string, DatatablePartialBaseModel>(),
                SelectedRowsActionButtonsPartials = actionControls ??
                                                    new Dictionary<string, DatatablePartialBaseModel>()
            };

            return View(model);
        }
    }
}