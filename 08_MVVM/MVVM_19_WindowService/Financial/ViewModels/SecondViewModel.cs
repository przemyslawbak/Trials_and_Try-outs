using Financial.Events;

namespace Financial.ViewModels
{
    //http://www.nichesoftware.co.nz/2015/08/16/wpf-event-aggregates.html
    public class SecondViewModel
    {
        private readonly IEventAggregator _eventAggregator;
        public SecondViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;

            _eventAggregator.RegisterHandler<SelectionChangedEvent>(OnSelectionChangedHandler);
            //_eventAggregator.UnregisterHandler<SelectionChangedEvent>(OnSelectionChangedHandler);
        }

        private void OnSelectionChangedHandler(SelectionChangedEvent obj)
        {
            var message = obj.Message;
            //do stuff
        }
    }
}
