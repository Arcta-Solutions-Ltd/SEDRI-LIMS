using arc.app.Config.Views.DiaryViews;
using arc.domain.Configuration.ViewConfig.Common;

namespace arc.app.Config
{
    public class AllBaseViewConfigFactory : IAllBaseViewConfigFactory
    {
        private readonly IListViewConfigFactory _listViewConfigFactory;
        private readonly IRecordViewConfigFactory _recordViewConfigFactory;
        private readonly IDiaryFactory _diaryFactory;

        public AllBaseViewConfigFactory(IListViewConfigFactory listViewConfigFactory, IRecordViewConfigFactory recordViewConfigFactory, IDiaryFactory diaryFactory)
        {
            _listViewConfigFactory = listViewConfigFactory;
            _recordViewConfigFactory = recordViewConfigFactory;
            _diaryFactory = diaryFactory;
        }

        public BaseViewConfig Get(string viewName)
        {
            var view = (BaseViewConfig)_listViewConfigFactory.GetViewAsync(viewName).Result;
            view ??= _recordViewConfigFactory.GetViewAsync(viewName).Result;
            view ??= _diaryFactory.Create(viewName);

            return view;
        }
    }
}
