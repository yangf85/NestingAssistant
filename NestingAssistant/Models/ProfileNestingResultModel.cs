using NestingAssistant.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NestingAssistant.Models
{
    public class ProfileNestingResultModel
    {
        public List<UsageProfileMaterialModel> Materials { get; set; } = [];

        public List<ProfileNestingSummaryModel> Summaries { get; set; } = [];
    }
}