using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BehaviorTriggers.Triggers
{
    public class LengthValidationTrigger : TriggerAction<Entry>
    {
        protected override void Invoke(Entry sender)
        {
            bool isValid = sender.Text.Length > 6;
            sender.BackgroundColor = isValid ? Color.Yellow : Color.Red;
        }
    }
}
