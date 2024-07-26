using CommunityToolkit.Mvvm.ComponentModel;
using MapsterMapper;
using NestingAssistant.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NestingAssistant.ViewModels
{
    public abstract class BasicViewModel : ObservableValidator
    {
        private readonly IMessageBoxService _messageBox;

        private readonly INotificationService _notification;

        private IMapper _mapper;

        public IMessageBoxService MessageBox => _messageBox;

        public INotificationService Notification => _notification;

        public IMapper Mapper => _mapper;

        protected BasicViewModel(IMessageBoxService messageBox, INotificationService notification, IMapper mapper)
        {
            _messageBox = messageBox;
            _notification = notification;
            _mapper = mapper;
        }
    }
}